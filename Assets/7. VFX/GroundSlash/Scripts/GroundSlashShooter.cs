using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GroundSlashShooter : MonoBehaviour
{
    public Transform player;
    public GameObject projectile;
    public Transform firePoint;
    public float fireRate = 4;

    private Vector3 destination;
    private float timeToFire;
    private GroundSlash groundSlashScript;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1")&& Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
    }
    void ShootProjectile()
    {
        // 플레이어를 기준으로 Ray를 생성
        Ray ray = new Ray(player.position, player.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            destination = hit.point;
            InstantiateProjectile();
        }
    }
    void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile,firePoint.position, Quaternion.identity)as GameObject;
        groundSlashScript = projectileObj.GetComponent<GroundSlash>();
        RotateToDestination(projectileObj, destination, true);
        projectileObj.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;
    }
    void RotateToDestination(GameObject obj,Vector3 destination, bool onlyY)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        if(onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;

        }
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
