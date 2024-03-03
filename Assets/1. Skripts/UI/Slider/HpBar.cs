using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : ExpBar
{
    [SerializeField]
    private Slider hpBar;
    private float maxHp;
    private float curHp;
    private int curlevel;
    // Start is called before the first frame update
    void Start()
    {

        maxHp = GameManager.Instance.maxHealth;
        curHp = GameManager.Instance.curHealth;
        hpBar.value = maxHp;

    }

    // Update is called once per frame
    void Update()
    {

        HealCondition();
        LevelUp();
    }

    void HealCondition()
    {
        maxHp = GameManager.Instance.maxHealth;
        curHp = GameManager.Instance.curHealth;
        hpBar.value = curHp/maxHp;
    }
    
    void LevelUp()
    {

        if (curExp >MaxExp)
        {
            maxHp = GameManager.Instance.maxHealth;
            hpBar.value = maxHp;
        }
    }
}