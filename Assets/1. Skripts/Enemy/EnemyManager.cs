using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance { get { return _instance; } }

    private DatabaseReference _databaseReference;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // Firebase 데이터베이스 참조 초기화
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Firebase에서 적의 체력과 공격력을 가져오는 메서드
    public void GetEnemyData(string enemyId, System.Action<EnemyData> callback)
    {
        _databaseReference.Child("enemies").Child(enemyId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to get enemy data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();
                EnemyData enemy = JsonUtility.FromJson<EnemyData>(json);

                // 데이터를 가져온 후 callback을 통해 반환
                callback(enemy);
            }
        });
    }

    // Firebase에 적의 체력과 공격력을 업데이트하는 메서드
    public void SetEnemyData(string enemyId, EnemyData enemyData)
    {
        string json = JsonUtility.ToJson(enemyData);
        _databaseReference.Child("enemies").Child(enemyId).SetRawJsonValueAsync(json);
    }
}