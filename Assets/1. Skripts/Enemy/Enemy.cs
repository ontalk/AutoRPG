using JetBrains.Annotations;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class Enemy : MonoBehaviour
{
    
    public Rigidbody rigid;
    public BoxCollider boxcollider;
    //MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;
    public enum Type { A, Boss };
    public Type enemyType;
    public float curHealth;
    public float maxHealth;
    public float moveSpeed;
    private Vector3 lastPosition;

    public Slider HpSlider; // 추가된 부분: 체력을 나타낼 Slider
    public BoxCollider Area;
    public GameObject bullet;
    public SkinnedMeshRenderer Slime;
    public Transform Target;
    public bool isTracking;
    public bool isAttack;
    public bool isLook;
    public bool isDead;
    bool isDamaged;
    bool isCritical;

    // 데미지를 받을 때 호출되는 이벤트
    public event Action<float> OnDamageTaken;
    float CriticalChance = 0.5f;//20퍼센트 확률로 치명타 데미지

    public int kill;
    public int exp;
    public int coin;
    [NonSerialized]
    public Vector3 lookVec;
    Vector3 trackingVec;
    public Transform objectToLock;



    // Start is called before the first frame update
    void Awake()
    {
        
        rigid = GetComponent<Rigidbody>();
        boxcollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        FindAndSetTarget();

        EnemyHpBar hpBarScript = FindObjectOfType<EnemyHpBar>();
        if (hpBarScript != null)
        {
            hpBarScript.AddEnemy(this);
        }


    }
  

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            FindAndSetTarget();
        }
    }

    public void Tracking()
    {
        if (nav.enabled && enemyType != Type.Boss)
        {
            if (enemyType != Type.Boss)
                anim.SetBool("isWalk", true);
            nav.SetDestination(Target.position);
            nav.isStopped = !isTracking;
        }
        else if(Target == null)
        {
            if (enemyType != Type.Boss)
                anim.SetBool("isWalk", false);
        }

    }


    private void FixedUpdate()
    {
        Tracking();
        Targeting();
        FreezeVelocity();
    }

    /*IEnumerator Track()
    {
        trackingVec = Target.position + lookVec;
        isLook = false;
        //nav.isStopped = false;
        if(enemyType != Type.Boss) 
            anim.SetBool("isWalk", true);
        yield return new WaitForSeconds(3f);
        isLook = true;

        StartCoroutine(Think());
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Track());
    }*/

    public void FindAndSetTarget()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Target = playerObject.transform;
            isTracking = true; // 타겟을 찾았을 때 추적을 활성화
        }
        
    }


    void Targeting()
    {
        if (!isDead & enemyType != Type.Boss) { 
            float targetRadius = 1.5f;
            float targetRange = 0.5f;

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
                StartCoroutine(Attack());
         }
    }

    IEnumerator Attack()
    {

        isTracking = false;
        isAttack = true;
        anim.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.2f);
        Area.enabled = true;

        yield return new WaitForSeconds(1f);
        Area.enabled = false;

        yield return new WaitForSeconds(1f);
        isAttack = false;
        isTracking = true;


    }


    void FreezeVelocity()
    {
        if (isTracking)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            if (transform.position == lastPosition &&  enemyType != Type.Boss)
            {
                anim.SetBool("isWalk", false);
            }

            lastPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isCritical = UnityEngine.Random.value <= CriticalChance;

        if (other.tag == "Weapon")
        {
            Weapon weaponDamage = other.GetComponent<Weapon>();
            float damageAmount = isCritical ? weaponDamage.CriticalDamage : weaponDamage.damage;

            curHealth -= damageAmount;
            // 데미지를 받을 때 이벤트 호출
            OnDamageTaken?.Invoke(damageAmount);
            UpdateHpSlider(); // 체력이 변경될 때마다 Slider 업데이트

            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamaged(reactVec));
        }
        else if (other.tag == "Skill")
        {
            SkillDamage skills = other.GetComponent<SkillDamage>();
            curHealth -= skills.Damage;
            UpdateHpSlider(); // 체력이 변경될 때마다 Slider 업데이트

        }
    }
    void UpdateHpSlider()
    {

        // HpSlider가 설정되어 있고, 최대 체력이 0 이상일 때
        if (HpSlider != null && maxHealth > 0)
        {
            // 현재 체력을 최대 체력으로 나눈 비율을 Slider에 설정
            HpSlider.value = curHealth / maxHealth;
        }
    }
    IEnumerator OnDamaged(Vector3 reactVec)
    {


        // 여기서부터 데미지 처리 코드
        reactVec = reactVec.normalized;

        if (curHealth > 0 && !isCritical)
        {
            if (enemyType != Type.Boss)
                anim.SetTrigger("doDamage");

            reactVec = reactVec.normalized;
            isTracking = false;
            nav.enabled = false;
            Slime.material.color = Color.yellow;
            reactVec += Vector3.back;
            rigid.AddForce(reactVec * 1f, ForceMode.Impulse);
            yield return new WaitForSeconds(0.3f);
            Slime.material.color = new Color(1, 1, 1, 1);
            isTracking = true;
            nav.enabled = true;
        }
        else if (curHealth > 0 && isCritical)
        {
            if(enemyType != Type.Boss)
                anim.SetTrigger("doCriticalDamage");
            reactVec = reactVec.normalized;
            isTracking = false;
            nav.enabled = false;
            Slime.material.color = Color.red;
            reactVec += Vector3.back;
            rigid.AddForce(reactVec * 2f, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);
            Slime.material.color = new Color(1, 1, 1, 1);
            isTracking = true;
            nav.enabled = true;

        }
        else if (curHealth <= 0)
        {
            isDead = true;
            GameManager.Instance.GetExp(exp);
            GameManager.Instance.GetCoin(coin);
            GameManager.Instance.PortalOpen(kill);
            isTracking = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            Slime.material.color = Color.gray;
            gameObject.layer = 11;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 2.5f, ForceMode.Impulse);
            Destroy(gameObject, 1f);
        }

    }
}