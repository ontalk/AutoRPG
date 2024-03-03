using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;
using Firebase.Firestore;

public class DefPetSkill : MonoBehaviour
{
    Animator anim;
    PetFollow petFollow;
    private bool isBuffActive = false;
    private float buffDuration = 60f;
    private float coolDownTime = 180f;
    private float lastExecutionTime; // ���� ���� �ð� ���
    private float lastBuffTime; // ������ ������ ������ �ð��� �����ϴ� ���� �߰�

    public string type;
    public float per;
    public float duration;
    public Sprite icon;

    void Awake()
    {
        anim = GetComponent<Animator>();
        petFollow = FindObjectOfType<PetFollow>();
        lastExecutionTime = Time.time; // �ʱ�ȭ
    }
    private async void Start() //�񵿱�
    {
        //��������
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("EnemyData").Document("1001");//�÷��ǿ��ִ� EnemyData���� ������ 1001�� �����´�.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//�÷��ǿ� �ִ� ��� ������ �����Ͷ�.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> CoolTime = documentSnapshot.ToDictionary();// string key�� , object value�� //object �� �� ū ����ȯ 

        Debug.Log((long)CoolTime["Damage"]);//�����ö� ����ȯ�ؼ� �����;ߵ�.
                                            //Debug.Log((string)x["Name"]);
    }

    void Update()
    {
            StrBuff();
    }


    private void StrBuff()
    {
        // ���� ������ ���� ���� �ƴϰ�, �ǰ� 90 ������ �� ����
        if (!isBuffActive && GameManager.Instance.curHealth <= 90)
        {
            StartCoroutine(StartStrBuff());

        }
    }

    

    private IEnumerator StartStrBuff()
    {
        // ������ Ȱ��ȭ�Ǿ����� ǥ��
        isBuffActive = true;

        anim.SetTrigger("doBuff");
        BuffMgr.instance.CreateBuff(type, per, duration, icon);

        yield return new WaitForSeconds(180f); //���� ���ð�.

        // ���� ��Ȱ��ȭ
        isBuffActive = false;

        #region 60�� ��ٸ��� �ð�
        /*yield return new WaitForSeconds(buffDuration); // 60�� ���� ��ٸ��ϴ�.

        // CoolDownTime ������ ���� �������� ���ư��� �ʽ��ϴ�.
        yield return new WaitForSeconds(coolDownTime - buffDuration);

        // CoolTime ���Ŀ��� �ǰ� 90 �̻��� ��쿡�� ������ ����
        if (GameManager.Instance.curHealth >= 90)
        {
            GameManager.Instance.curDamage = GameManager.Instance.MaxDamage;
            // ������ �����Ǿ����� ǥ��
            isBuffActive = false;
        }
        else
        {
            // �ǰ� 90 �̸��� ��� CoolDownTime�� ���� �Ŀ��� ���� �������� ���ư��� �ʵ��� �÷��� ����
            isBuffActive = false;
        }*/
        #endregion
    }
}
