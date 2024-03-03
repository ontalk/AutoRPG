using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : SkillDamage
{
    private Animator anim;
    public Weapon equipWeapon; // 장비한 무기
    public GameObject healingPrefab;
    private GameObject currentHealingObject;
    private Rigidbody rigid;
    private bool HealDown;
    private bool GroundSwardSlashDown;
    private bool SwardSlashDown;
    private bool UltimateSwardSlashDown;

    bool isAttackReady = true; // 공격이 준비된 상태인지 여부
    public bool isSkill;
    public bool isHeal;
    private bool isAttack = true; // 공격 중인지 여부

    float AttackDelay;
    public float healingDuration = 5f;

    float Healing;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private async void Start()
    {
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("SkillData").Document("3003");//컬렉션에있는 EnemyData에서 문서인 1001를 가져온다.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//컬렉션에 있는 모든 문서를 가져와라.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> x = documentSnapshot.ToDictionary();// string key값 , object value값 //object 는 더 큰 형변환 

        Healing = (long)x["Heal"];
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        SwardSkill();
        HealSkill();
    }

    void GetInput()
    {
        GroundSwardSlashDown = Input.GetButton("GroundSwardSlash");
        SwardSlashDown = Input.GetButton("SwardSlash");
        UltimateSwardSlashDown = Input.GetButton("UltimateSwardSlash");
        HealDown = Input.GetButton("Heal");
    }
    void SwardSkill()
    {
        if (equipWeapon == null)
            return;

        isAttack = true;
        isSkill = true;
        AttackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < AttackDelay;

        if (GroundSwardSlashDown && isAttackReady)
        {
            equipWeapon.Use();
            anim.SetTrigger("doGroundSwardSlash");
            AttackDelay = 2;
        }
        else if (SwardSlashDown && isAttackReady)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwardSlash");
            AttackDelay = 3;
        }
        else if (UltimateSwardSlashDown && isAttackReady)
        {
            equipWeapon.Use();
            anim.SetTrigger("doUltimateSwardSlash");
            AttackDelay = 4;
        }
        isSkill = false;
    }

    void HealSkill()
    {
        if (HealDown && !isHeal)
        {
             
            isHeal = true;
            anim.SetBool("isHeal", true);

            // Start the Coroutine to handle the healing effect
            StartCoroutine(HealingCoroutine());
        }
        else if (!HealDown && isHeal)
        {
            isHeal = false;
            anim.SetBool("isHeal", false);

            // Stop the Coroutine if healing is interrupted
            StopCoroutine(HealingCoroutine());

            // Destroy the current healing object if it exists
            DestroyHealingObject();
        }
    }

    IEnumerator HealingCoroutine()
    {
        // Instantiate the healing object
        currentHealingObject = Instantiate(healingPrefab, transform.position, Quaternion.identity);

        // Wait for the specified duration
        yield return new WaitForSeconds(healingDuration);
        while (isHeal)
        {
          /*  float curHealth = GameManager.Instance.curHealth;
            curHealth += 5f;
*/
            GameManager.Instance.curHealth += Healing;
            yield return new WaitForSeconds(1f);
        }

        // Destroy the healing object after the duration
        DestroyHealingObject();
    }

    void DestroyHealingObject()
    {
        // Check if the healing object exists before trying to destroy it
        if (currentHealingObject != null)
        {
            Destroy(currentHealingObject);
        }
    }
}
