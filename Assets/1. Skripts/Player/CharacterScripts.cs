using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CharacterScripts : MonoBehaviour
{
    private static CharacterScripts instance =null;

    [SerializeField] private float initialMoveSpeed = 5f; // 초기 이동 속도
    [SerializeField] private float maxMoveSpeed = 13f; // 최대 이동 속도
    private float currentMoveSpeed; // 현재 이동 속도
    [SerializeField] private float JumpPulse = 10f;

    [NonSerialized]
    public Vector3 MoveTo; // 캐릭터 이동 방향을 나타내는 벡터
    private float Horizontal_Move; // 수평 입력 (좌우)
    private float Vertical_Move; // 수직 입력 (전후)
    private int AttackCount;

    //캐릭터 키 관리
    private bool RunDown;
    private bool isJump;
    private bool JumpDown;
    private bool DefendDown;
    private bool AttackDown;
    private bool Attack2Down;
    private bool interactionDown;

    //캐릭터 사운드 관리
    //public AudioClip AttackSound;
    //캐릭터 상태 관리
    private  bool isMove = true; // 이동 중인지 여부
    private bool isAttack = true; // 공격 중인지 여부
    [NonSerialized] public bool isDefend = true; // 방어 중인지 여부
    bool isDamage; // 피해를 입었는지 여부
    bool isBorder; // 캐릭터가 지면 경계에 있는지 여부
    bool isAttackReady = true; // 공격이 준비된 상태인지 여부
    float AttackDelay;

    MeshRenderer[] meshs; // 캐릭터 메쉬 렌더러 배열
    public SkinnedMeshRenderer Player;
    private Camera mainCamera;
    public Weapon equipWeapon; // 장비한 무기
    private GameObject nearObject; // 주변에 있는 객체
    private Rigidbody rigid;
    private GameObject enemy; // 적(Enemy) 객체
    public Animator anim;
    private Combat combat;
    private GameObject Character;
    private AtkPetSkill petSkill;
    public float healingDuration = 5f;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>(); // 자식 오브젝트로부터 무기(Weapon) 컴포넌트 얻기
        meshs = GetComponentsInChildren<MeshRenderer>(); // 모든 하위 메쉬 렌더러를 배열로 얻기
        combat = GetComponent<Combat>();
        petSkill = FindObjectOfType<AtkPetSkill>();

        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static CharacterScripts Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Start()
    {
        
        // 카메라 스크립트를 사용하기 위해 메인 카메라를 찾음
        mainCamera = Camera.main;
        DontDestroyOnLoad(gameObject);
        // 게임 시작 시 GameManager에 대한 참조를 설정
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.SetCharacterScripts(this);
        }
    }

    void Update()
    {
        GetInput();
        move();
        Jump();
        //Attack();
        Defend();
    }

    void GetInput()
    {
        JumpDown = Input.GetButtonDown("Jump");
        RunDown = Input.GetButton("Run");
        AttackDown = Input.GetButton("Fire1");
        Attack2Down = Input.GetButton("Fire2");
        DefendDown = Input.GetButton("Defend");
        interactionDown = Input.GetButton("interaction");
    }

    void move()
    {
        if (!combat.isAttacking)
        {
            Horizontal_Move = Input.GetAxisRaw("Horizontal"); // 수평 입력 (좌우)
            Vertical_Move = Input.GetAxisRaw("Vertical"); // 수직 입력 (전후)

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0; // 수직 방향 이동 방지
            cameraForward.Normalize();

            Vector3 MoveTo = cameraForward * Vertical_Move + Camera.main.transform.right * Horizontal_Move;
            MoveTo.Normalize(); // 이동 방향을 정규화

            if (!isAttackReady || isDefend)
                MoveTo = Vector3.zero;

            // 이동 속도 조절
            currentMoveSpeed = Mathf.Lerp(initialMoveSpeed, maxMoveSpeed, Time.time / 10f);

            if (!isBorder)
                transform.position += MoveTo * Time.deltaTime * (RunDown ? 2f : 1f) * currentMoveSpeed;

            transform.LookAt(transform.position + MoveTo);

            anim.SetBool("isWalk", MoveTo != Vector3.zero);
            anim.SetBool("isRun", RunDown);
        }
        else
        {
            // 움직임 중지
            MoveTo = Vector3.zero;
            // 애니메이션 재생
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);

            // 공격 애니메이션 레이어의 가중치를 최대로 조절
            //anim.SetLayerWeight(1, 1.0f);

            // 움직임의 포지션은 변하지 않도록
            transform.position += Vector3.zero;

            // 움직임을 정지하고 싶지 않다면 코드 사용 transform.position += MoveTo * Time.deltaTime * (RunDown ? 2f : 1f) * currentMoveSpeed;
        }
    }

    void Jump()
    {
        if (JumpDown && !isJump && !isDefend)
        {
            anim.SetTrigger("doJump");
            rigid.AddForce(Vector3.up * JumpPulse, ForceMode.Impulse);
            isJump = true;
        }
    }

    void WalkJump()
    {
        if (JumpDown && !isJump && !isDefend)
        {
            anim.SetTrigger("doWalkJump");
            rigid.AddForce(Vector3.up * JumpPulse, ForceMode.Impulse);
            isJump = true;
        }
    }
    void Defend()
    {
        if (DefendDown)
        {
            isDefend = true;
            anim.SetBool("isShield", true);
            isDamage = true; // 공격 받을 때 방어 중에만 데미지를 받도록 변경
            isMove = true;
        }
        else
        {
            anim.SetBool("isShield", false);
            isDefend = false;
            isDamage = false;
            isMove = false;
        }
    }

    void Interation()
    {
        if (interactionDown && nearObject != null && !isJump)//!isDodge)
        {
            if(nearObject.tag== "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);//자기 자신이 접근할땐 this
            }
        }
    }
    void Attack()
    {
        if (equipWeapon == null)
            return;

        isAttack = true;
        AttackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < AttackDelay;

        
        if (AttackDown && isAttackReady && !isDefend)
        {
            equipWeapon.Use();
            AttackDelay = 0;
            anim.SetTrigger("doAttack");
        }
        else if (Attack2Down && isAttackReady && !isDefend)
        {
            equipWeapon.Use();
            anim.SetTrigger("doAttack2");
            AttackDelay = 0;
        }

        
    }


    void FreezeRoation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        FreezeRoation();
    }

    private void TriggerEnemyWeapon(Collider other)
    {
        if (!isDamage)
        {
            EnemyWeapon enemyWeapon = other.GetComponent<EnemyWeapon>();
            BossRock bossrock = other.GetComponent<BossRock>();
            if (enemyWeapon)
                GameManager.Instance.curHealth = GameManager.Instance.curHealth + GameManager.Instance.curArmor - enemyWeapon.damage;
            else if (bossrock)
                GameManager.Instance.curHealth = GameManager.Instance.curHealth + GameManager.Instance.curArmor - bossrock.damage;

            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);

            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamaged(reactVec));
        }
    }

    IEnumerator OnDamaged(Vector3 reactVec)
    {
        if (GameManager.Instance.curHealth > 0)
        {
            isDamage = true;
            foreach (MeshRenderer mesh in meshs)
            {
                anim.SetTrigger("doDamage");
                Player.material.color = Color.red;

            }
            yield return new WaitForSeconds(1.2f);

            isDamage = false;
            foreach (MeshRenderer mesh in meshs)
            {
                Player.material.color = Color.white;
            }
        }
        else
        {
            isDamage = false;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            Player.material.color = Color.gray;
            gameObject.layer = 13;
            anim.SetTrigger("doDie");
            rigid.AddForce(reactVec * 2f, ForceMode.Impulse);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            Die();
        }
    }

    public void ActivateGameObject()
    {
        // GameManager에서 호출되었을 때 실행되는 로직 추가
        gameObject.SetActive(true);
    }
    public void Die()
    {
        int curHeart;
        int addHeart;
        curHeart = GameManager.Instance.curHeart;
        addHeart = GameManager.Instance.addHeart;


        // 캐릭터 스폰 스크립트를 찾아 스폰 메서드 호출
        CharacterSpawnPoint spawner = FindObjectOfType<CharacterSpawnPoint>();

        if (spawner != null)
        {
            // 스폰 포인트에서 플레이어를 텔레포트하고 활성화
            spawner.TeleportCharacter(gameObject);
        }

        if (GameManager.Instance.RevivePanel != null)
        {

            GameManager.Instance.RevivePanel.SetActive(true);
            if (curHeart >= 1 || addHeart >= 1)
            {
                if (curHeart >= 1)
                    GameManager.Instance.heartRevivePanel.SetActive(true);
                else if (addHeart >= 1)
                    GameManager.Instance.addHeartRevivePanel.SetActive(true);
            }
            else
            {
                GameManager.Instance.heartRevivePanel.SetActive(false);
                GameManager.Instance.addHeartRevivePanel.SetActive(false);
            }

        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            TriggerEnemyWeapon(other);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag =="Shop")
            nearObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
        else if(other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false;
        }
    }
}
