using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;
using Firebase.Firestore;

public class AtkPetSkill : MonoBehaviour
{
    Animator anim;
    PetFollow petFollow;
    private bool isBuffActive = false;
    private float buffDuration = 60f;
    private float coolDownTime = 180f;
    public string type;
    public float per;
    public float duration;
    public Sprite icon;
    private Effect effect;

    void Awake()
    {
        anim = GetComponent<Animator>();
        effect = FindObjectOfType<Effect>();
    }

    private async void Start()
    {
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("EnemyData").Document("1001");
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> CoolTime = documentSnapshot.ToDictionary();

        Debug.Log((long)CoolTime["Damage"]);
    }

    void Update()
    {
        StrBuff();
    }

    private void StrBuff()
    {

        if (!isBuffActive && GameManager.Instance.curHealth <= 90)
        {

            StartCoroutine(StartStrBuff());
        }
    }

    private IEnumerator StartStrBuff()
    {
        isBuffActive = true;


        anim.SetTrigger("doBuff");
        BuffMgr.instance.CreateBuff(type, per, duration, icon);
        effect.StartAtkCoroutine();

        yield return new WaitForSeconds(180f);
        isBuffActive = false;
    }
}
