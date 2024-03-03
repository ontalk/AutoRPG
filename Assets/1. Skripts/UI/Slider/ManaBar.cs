using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaBar : ExpBar
{
    [SerializeField]
    private Slider manaBar;
    private float maxMana;
    private float curMana;
    private int curlevel;
    // Start is called before the first frame update
    void Start()
    {
        maxMana = GameManager.Instance.maxMana;
        curMana = GameManager.Instance.curMana;
        manaBar.value = maxMana;
    }

    // Update is called once per frame
    void Update()
    {

        HealCondition();
        LevelUp();
    }

    void HealCondition()
    {
        maxMana = GameManager.Instance.maxMana;
        curMana = GameManager.Instance.curMana;
        manaBar.value = curMana / maxMana;
    }

    void LevelUp()
    {

        if (curExp > MaxExp)
        {
            maxMana = GameManager.Instance.maxMana;
            manaBar.value = maxMana;
        }
    }
}