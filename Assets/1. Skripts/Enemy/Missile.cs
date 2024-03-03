using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Missile : EnemyWeapon
{
    /*public Transform Target;
    NavMeshAgent nav;*/
    // Update is called once per frame

    private void Awake()
    {
        //nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        //nav.SetDestination(Target.position);
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
