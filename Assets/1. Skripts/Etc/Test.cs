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
    private async void Start() //비동기
    {
        //가져오기
        DocumentReference documentRef = FirebaseFirestore.DefaultInstance.Collection("EnemyData").Document("1001");//컬렉션에있는 EnemyData에서 문서인 1001를 가져온다.
        //var documentRef = FirebaseFirestore.DefaultInstance.Collection("Test").GetSnapshotAsync();//컬렉션에 있는 모든 문서를 가져와라.
        DocumentSnapshot documentSnapshot = await documentRef.GetSnapshotAsync();
        Dictionary<string, object> x = documentSnapshot.ToDictionary();// string key값 , object value값 //object 는 더 큰 형변환 

            Debug.Log((long)x["Damage"]);//가져올때 형변환해서 가져와야됨.
            //Debug.Log((string)x["Name"]);


        //쓰기
        //await documentRef.SetAsync(new Dictionary<string, object> { { "Name", "Slime" } },SetOptions.MergeAll);//두번째 매개변수에 아무것도 넣지않으면 데이터가 다 날라간다. SetOptions.OverWrite가 덮어씌우기
        
    }



}
