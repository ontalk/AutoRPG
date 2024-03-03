using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float health;
    public float attackPower;

    public EnemyData(float health, float attackPower)
    {
        this.health = health;
        this.attackPower = attackPower;
    }
}
