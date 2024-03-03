using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private Slider hpbarPrefab; // ü�¹� ������ �߰�
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
            //ü�¹� ���� ����Ʈ
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
            // Enemy ������ �޾Ƽ� HP Bar ���� �� ����
            m_objectList.Add(enemy.transform);

            //ü�¹� ���� 
            GameObject hpBar = Instantiate(hpbarPrefab.gameObject, enemy.transform.position, Quaternion.identity);
            hpBar.transform.SetParent(transform);
            m_hpBarList.Add(hpBar);

            float initialHpRatio = enemy.curHealth / enemy.maxHealth;
            hpBar.GetComponent<Slider>().value = initialHpRatio;

            // �̺�Ʈ ���: Enemy�� �������� �޾��� �� ȣ��� �Լ� ����
            enemy.OnDamageTaken += UpdateHpBar;
        }
    }
    private void UpdateHpBar(float damageAmount)
    {
        // �������� �޾��� �� ȣ��Ǵ� �Լ�
        // ���⼭ ü�¹ٸ� �����ϴ� �۾��� ����
        // ���� ���, ü�¹��� ���� ���ҽ�Ű�ų� �ٸ� ȿ���� �� �� ����
        // ...

        // ����: ü�¹ٸ� ��������ŭ ���ҽ�Ű��
        for (int i = 0; i < m_objectList.Count; i++)
        {
            if (m_objectList[i] != null)
            {
                float curHealth = m_objectList[i].GetComponent<Enemy>().curHealth;
                float maxHealth = m_objectList[i].GetComponent<Enemy>().maxHealth;

                // �� ü�¹��� ���� ����
                m_hpBarList[i].GetComponent<Slider>().value = curHealth / maxHealth;
            }
        }
    }
}