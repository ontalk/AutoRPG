using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //���Ͱ� ������ ��ġ�� ���� �迭
    public Transform[] points;
    //���� �������� �Ҵ��� ����\
    public GameObject monsterPrefab;

    //���͸� �߻���ų �ֱ�
    public float createTime;
    //������ �ִ� �߻� ����
    public int maxMonster = 10;
    //���� ���� ���� ����
    
    //GameManager gameManager;  bool �� ���� ��������
    // Use this for initialization
    void Start()
    {
        
        //Hierarchy View�� Spawn Point�� ã�� ������ �ִ� ��� Transform ������Ʈ�� ã�ƿ�
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        //gameManager = FindObjectOfType<GameManager>(); //GameManager ��ũ��Ʈ�� �ν��Ͻ��� �޾ƿ�
        
        if (points.Length > 0)
        {
            //���� ���� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(this.CreateMonster());
        }
    }

    IEnumerator CreateMonster()
    {

        bool isGameOver = GameManager.Instance.isGameOver;
        //bool isGameOver = gameManager.isGameOver; ���ӸŴ����� bool �� ��������
        float timer = Time.deltaTime;

        //���� ���� �ñ��� ���� ����
        while (!isGameOver)
        {
            //���� ������ ���� ���� ����
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;


            if (monsterCount < maxMonster)
            {
                //������ ���� �ֱ� �ð���ŭ ���
                yield return new WaitForSeconds(createTime);

                //�ұ�Ģ���� ��ġ ����
                int idx = Random.Range(1, points.Length);
                //������ ���� ����
                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }


        }
    }
}


