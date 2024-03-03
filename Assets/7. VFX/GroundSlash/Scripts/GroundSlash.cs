using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundSlash : MonoBehaviour
{
    public float speed = 10f;
    public float slowDownRate = 0.01f;
    public float detectingdistance = 0.1f;
    public float destoryDelay = 5f;
    private Rigidbody rb;
    private bool stopped;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0 , transform.position.z);
        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());

        }
        else
            Debug.Log("No RigidBody");

        Destroy(gameObject, destoryDelay);
    }
    

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x , transform.position.y +1, transform.position.z);
            if(Physics.Raycast(distance, transform.TransformDirection(-Vector3.up),out hit, detectingdistance))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x,0,transform.position.z);

            }
            Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingdistance),Color.red);
        }
    }
    IEnumerator SlowDown()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }
        stopped = true;
    }
}
