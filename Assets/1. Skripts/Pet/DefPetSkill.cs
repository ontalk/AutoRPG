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
    private float lastExecutionTime; // 이전 실행 시간 기록
    private float lastBuffTime; // 이전에 버프를 실행한 시간을 저장하는 변수 추가

    public string type;
    public float per;
    public float duration;
    public Sprite icon;

    void Awake()
    {
        anim = GetComponent<Animator>();
        petFollow = FindObjectOfType<PetFollow>();
        lastExecutionTime = Time.time; // 초기화
    }
    private async void Start() //비동기
    {
        //가져오기
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("EnemyData").Document("1001");//컬렉션에있는 EnemyData에서 문서인 1001를 가져온다.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//컬렉션에 있는 모든 문서를 가져와라.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> CoolTime = documentSnapshot.ToDictionary();// string key값 , object value값 //object 는 더 큰 형변환 

        Debug.Log((long)CoolTime["Damage"]);//가져올때 형변환해서 가져와야됨.
                                            //Debug.Log((string)x["Name"]);
    }

    void Update()
    {
            StrBuff();
    }


    private void StrBuff()
    {
        // 현재 버프가 실행 중이 아니고, 피가 90 이하일 때 실행
        if (!isBuffActive && GameManager.Instance.curHealth <= 90)
        {
            StartCoroutine(StartStrBuff());

        }
    }

    

    private IEnumerator StartStrBuff()
    {
        // 버프가 활성화되었음을 표시
        isBuffActive = true;

        anim.SetTrigger("doBuff");
        BuffMgr.instance.CreateBuff(type, per, duration, icon);

        yield return new WaitForSeconds(180f); //재사용 대기시간.

        // 버프 비활성화
        isBuffActive = false;

        #region 60초 기다리는 시간
        /*yield return new WaitForSeconds(buffDuration); // 60초 동안 기다립니다.

        // CoolDownTime 동안은 원래 데미지로 돌아가지 않습니다.
        yield return new WaitForSeconds(coolDownTime - buffDuration);

        // CoolTime 이후에도 피가 90 이상인 경우에만 데미지 복귀
        if (GameManager.Instance.curHealth >= 90)
        {
            GameManager.Instance.curDamage = GameManager.Instance.MaxDamage;
            // 버프가 해제되었음을 표시
            isBuffActive = false;
        }
        else
        {
            // 피가 90 미만인 경우 CoolDownTime이 끝난 후에도 원래 데미지로 돌아가지 않도록 플래그 해제
            isBuffActive = false;
        }*/
        #endregion
    }
}
