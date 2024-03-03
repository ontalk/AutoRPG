using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    public BoxCollider Area;

    public bool isMelee;
    public bool isRock;
    public bool isMissile;

    private void Update()
    {
        if (isMissile)
        {
            Destroy(gameObject, 5f);
        }
    }
}