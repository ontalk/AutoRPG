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
    private Boss enemy; // Enemy 대신 Boss로 변경

    private float CurHp;
    private float MaxHp;

    private void Awake()
    {
        // Awake에서 실행되므로 프리팹이나 오브젝트가 활성화되기 전에 실행됩니다.
        FindAndSetBoss();
    }

    private void Start()
    {
        // Start에서 실행되어 Awake에서 실행한 후 추가적인 초기화가 필요한 경우에 사용될 수 있습니다.
    }

    private void FindAndSetBoss()
    {
        Boss[] bosses = FindObjectsOfType<Boss>();
        if (bosses.Length > 0)
        {
            // 여러 개의 Boss 중 하나를 선택하거나, 상황에 맞게 로직을 수정하세요.
            enemy = bosses[0];
            CurHp = enemy.curHealth;
            MaxHp = enemy.maxHealth;
        }
    }

    private void Update()
    {
        if (enemy == null)
        {
            // 보스가 아직 설정되지 않았다면 찾아서 설정
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
