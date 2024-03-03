using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BossHpbar : MonoBehaviour
{
    [SerializeField]
    private Slider hpbar;
    [SerializeField]
    private Boss enemy; // Enemy ��� Boss�� ����

    private float CurHp;
    private float MaxHp;

    private void Awake()
    {
        // Awake���� ����ǹǷ� �������̳� ������Ʈ�� Ȱ��ȭ�Ǳ� ���� ����˴ϴ�.
        FindAndSetBoss();
    }

    private void Start()
    {
        // Start���� ����Ǿ� Awake���� ������ �� �߰����� �ʱ�ȭ�� �ʿ��� ��쿡 ���� �� �ֽ��ϴ�.
    }

    private void FindAndSetBoss()
    {
        Boss[] bosses = FindObjectsOfType<Boss>();
        if (bosses.Length > 0)
        {
            // ���� ���� Boss �� �ϳ��� �����ϰų�, ��Ȳ�� �°� ������ �����ϼ���.
            enemy = bosses[0];
            CurHp = enemy.curHealth;
            MaxHp = enemy.maxHealth;
        }
    }

    private void Update()
    {
        if (enemy == null)
        {
            // ������ ���� �������� �ʾҴٸ� ã�Ƽ� ����
            FindAndSetBoss();
        }
        else
        {
            MaxHp = enemy.maxHealth;
            CurHp = enemy.curHealth;
            hpbar.value = CurHp / MaxHp;
        }
    }


}
