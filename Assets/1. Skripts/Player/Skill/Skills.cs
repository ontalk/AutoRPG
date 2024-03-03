using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : SkillDamage
{
    private Animator anim;
    public Weapon equipWeapon; // ����� ����
    public GameObject healingPrefab;
    private GameObject currentHealingObject;
    private Rigidbody rigid;
    private bool HealDown;
    private bool GroundSwardSlashDown;
    private bool SwardSlashDown;
    private bool UltimateSwardSlashDown;

    bool isAttackReady = true; // ������ �غ�� �������� ����
    public bool isSkill;
    public bool isHeal;
    private bool isAttack = true; // ���� ������ ����

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
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("SkillData").Document("3003");//�÷��ǿ��ִ� EnemyData���� ������ 1001�� �����´�.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//�÷��ǿ� �ִ� ��� ������ �����Ͷ�.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> x = documentSnapshot.ToDictionary();// string key�� , object value�� //object �� �� ū ����ȯ 

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
