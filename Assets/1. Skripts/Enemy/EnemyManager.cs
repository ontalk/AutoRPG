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

        // Firebase �����ͺ��̽� ���� �ʱ�ȭ
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Firebase���� ���� ü�°� ���ݷ��� �������� �޼���
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

                // �����͸� ������ �� callback�� ���� ��ȯ
                callback(enemy);
            }
        });
    }

    // Firebase�� ���� ü�°� ���ݷ��� ������Ʈ�ϴ� �޼���
    public void SetEnemyData(string enemyId, EnemyData enemyData)
    {
        string json = JsonUtility.ToJson(enemyData);
        _databaseReference.Child("enemies").Child(enemyId).SetRawJsonValueAsync(json);
    }
}