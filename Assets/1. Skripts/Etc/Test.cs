using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;

public class Test : MonoBehaviour
{
    private async void Start() //�񵿱�
    {
        //��������
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("EnemyData").Document("1001");//�÷��ǿ��ִ� EnemyData���� ������ 1001�� �����´�.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//�÷��ǿ� �ִ� ��� ������ �����Ͷ�.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> x = documentSnapshot.ToDictionary();// string key�� , object value�� //object �� �� ū ����ȯ 

            Debug.Log((long)x["Damage"]);//�����ö� ����ȯ�ؼ� �����;ߵ�.
            //Debug.Log((string)x["Name"]);


        //����
        //await documentRef.SetAsync(new Dictionary<string, object> { { "Name", "Slime" } },SetOptions.MergeAll);//�ι�° �Ű������� �ƹ��͵� ���������� �����Ͱ� �� ���󰣴�. SetOptions.OverWrite�� ������
        
    }



}
