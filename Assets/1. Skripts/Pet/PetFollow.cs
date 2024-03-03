using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PetFollow : MonoBehaviour
{
    public Rigidbody rigid;
    public BoxCollider boxcollider;
    public NavMeshAgent nav;
    public Animator anim;
    public Transform Target;
    public bool isTracking;
    private Vector3 lastPosition;
    public float maxDistanceFromPlayer = 5f; // �ִ� �÷��̾���� �Ÿ�
    public float stopDistance = 2f; // �÷��̾� �տ��� ���� �Ÿ�
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxcollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        FindAndSetTarget();
    }

    private void Start()
    {
        // �ʱ�ȭ �ڵ� �߰�
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            FindAndSetTarget();
        }
        //ĳ���Ͱ� ��Ȱ��ȭ�Ǹ� �굵 �ı�
        if (!GetCharacterActivationState())
        {
            Destroy(gameObject);
        }
    }

    public void Tracking()
    {
        if (nav.enabled && Target != null)
        {
            // �÷��̾���� �Ÿ��� ���
            float distanceToPlayer = Vector3.Distance(transform.position, Target.position);

            if (distanceToPlayer <= maxDistanceFromPlayer)
            {
                // ���� ���� �ȿ� ���� ���� ����
                anim.SetBool("isRun", true);
                nav.SetDestination(Target.position);

                if (distanceToPlayer <= stopDistance)
                {
                    // ���� �Ÿ� �̳��� ������ ���߱�
                    nav.isStopped = true;
                    anim.SetBool("isRun", false);
                }
                else
                {
                    nav.isStopped = false;
                }
            }
            else
            {
                // �÷��̾ 20���͸� ����� �÷��̾� ������ ���� �̵�
                Vector3 moveDirection = Target.position - transform.position;
                Vector3 newPosition = Target.position - moveDirection.normalized * stopDistance;
                nav.Warp(newPosition);
                anim.SetBool("isRun", true);
            }
        }
        else if (Target == null)
        {
            anim.SetBool("isRun", false);
        }

    }

    private void FixedUpdate()
    {
        Tracking();
        FreezeVelocity();
    }

    public void FindAndSetTarget()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Target = playerObject.transform;
            isTracking = true; // Ÿ���� ã���� �� ������ Ȱ��ȭ
        }
    }

    void FreezeVelocity()
    {
        if (isTracking)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            // �������� ��ȭ�� ���ٸ� isRun�� false�� ����
            if (transform.position == lastPosition)
            {
                anim.SetBool("isRun", false);
            }

            lastPosition = transform.position;
        }
    }
    private bool GetCharacterActivationState()
    {
        // ���⿡ ĳ������ Ȱ��ȭ ���¸� ��ȯ�ϴ� �ڵ带 �������ּ���.
        // ���� ���, ĳ������ �±װ� "Player"���� ���θ� Ȯ���� �� �ֽ��ϴ�.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        return playerObject != null && playerObject.activeSelf;
    }
}
