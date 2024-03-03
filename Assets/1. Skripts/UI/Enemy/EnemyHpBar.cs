using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private Slider hpbarPrefab; // 체력바 프리팹 추가
    private Enemy enemy;
    private float curHealth;
    private float maxHealth;
    List<Transform> m_objectList = new List<Transform>();
    List<GameObject> m_hpBarList = new List<GameObject>();

    Camera m_cam = null;

    private void Awake()
    {
        m_cam = Camera.main;
        GameObject[] t_objects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < t_objects.Length; i++)
        {
            m_objectList.Add(t_objects[i].transform);

            // Instantiate health bar prefab and add it to the list
            GameObject t_hpbar = Instantiate(hpbarPrefab.gameObject, t_objects[i].transform.position, Quaternion.identity, transform);
            m_hpBarList.Add(t_hpbar);
        }
    }

    void Update()
    {
        for (int i = 0; i < m_objectList.Count; i++)
        {
            //체력바 관련 리스트
            if (m_objectList[i] != null)
            {
                // Update the position of the health bar
                m_hpBarList[i].transform.position = m_cam.WorldToScreenPoint(m_objectList[i].position + new Vector3(0, 5f, 0));
            }
            else
            {
                // Destroy the health bar if the enemy is dead
                Destroy(m_hpBarList[i]);
                m_hpBarList.RemoveAt(i);
                m_objectList.RemoveAt(i);
            }
        }
    }
    public void AddEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            // Enemy 정보를 받아서 HP Bar 생성 및 연결
            m_objectList.Add(enemy.transform);

            //체력바 생성 
            GameObject hpBar = Instantiate(hpbarPrefab.gameObject, enemy.transform.position, Quaternion.identity);
            hpBar.transform.SetParent(transform);
            m_hpBarList.Add(hpBar);

            float initialHpRatio = enemy.curHealth / enemy.maxHealth;
            hpBar.GetComponent<Slider>().value = initialHpRatio;

            // 이벤트 등록: Enemy의 데미지를 받았을 때 호출될 함수 연결
            enemy.OnDamageTaken += UpdateHpBar;
        }
    }
    private void UpdateHpBar(float damageAmount)
    {
        // 데미지를 받았을 때 호출되는 함수
        // 여기서 체력바를 갱신하는 작업을 수행
        // 예를 들어, 체력바의 값을 감소시키거나 다른 효과를 줄 수 있음
        // ...

        // 예시: 체력바를 데미지만큼 감소시키기
        for (int i = 0; i < m_objectList.Count; i++)
        {
            if (m_objectList[i] != null)
            {
                float curHealth = m_objectList[i].GetComponent<Enemy>().curHealth;
                float maxHealth = m_objectList[i].GetComponent<Enemy>().maxHealth;

                // 각 체력바의 값을 갱신
                m_hpBarList[i].GetComponent<Slider>().value = curHealth / maxHealth;
            }
        }
    }
}