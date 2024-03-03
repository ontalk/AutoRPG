using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine.UIElements.Experimental;

public class SkillDamage : MonoBehaviour
{
    public float Damage;
    // Start is called before the first frame update
    async void Start()
    {
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("SkillData").Document("3003");//컬렉션에있는 EnemyData에서 문서인 1001를 가져온다.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//컬렉션에 있는 모든 문서를 가져와라.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> x = documentSnapshot.ToDictionary();// string key값 , object value값 //object 는 더 큰 형변환 

        //Damage= (long)x["Damage"];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
