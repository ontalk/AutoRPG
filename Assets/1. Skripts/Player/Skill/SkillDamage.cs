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
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("SkillData").Document("3003");//�÷��ǿ��ִ� EnemyData���� ������ 1001�� �����´�.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//�÷��ǿ� �ִ� ��� ������ �����Ͷ�.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> x = documentSnapshot.ToDictionary();// string key�� , object value�� //object �� �� ū ����ȯ 

        //Damage= (long)x["Damage"];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
