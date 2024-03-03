using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExpBar : MonoBehaviour
{
    [SerializeField]
    private Slider expBar;
    [NonSerialized]
    public float curExp;
    [NonSerialized]
    public float MaxExp;


    // Update is called once per frame

    private void Start()
    {
        curExp = GameManager.Instance.curExp;
        MaxExp = GameManager.Instance.maxExp;
        expBar.value = curExp / MaxExp;
    }

    private void Update()
    {
        curExp = GameManager.Instance.curExp;
        expBar.value += curExp;
        Handle();
    }

    void Handle()
    {
        curExp = GameManager.Instance.curExp;
        MaxExp = GameManager.Instance.maxExp;
        expBar.value = curExp / MaxExp;

    }
}