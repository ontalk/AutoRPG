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
    public float maxDistanceFromPlayer = 5f; // 최대 플레이어와의 거리
    public float stopDistance = 2f; // 플레이어 앞에서 멈출 거리
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
        // 초기화 코드 추가
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            FindAndSetTarget();
        }
        //캐릭터가 비활성화되면 펫도 파괴
        if (!GetCharacterActivationState())
        {
            Destroy(gameObject);
        }
    }

    public void Tracking()
    {
        if (nav.enabled && Target != null)
        {
            // 플레이어와의 거리를 계산
            float distanceToPlayer = Vector3.Distance(transform.position, Target.position);

            if (distanceToPlayer <= maxDistanceFromPlayer)
            {
                // 일정 범위 안에 있을 때만 추적
                anim.SetBool("isRun", true);
                nav.SetDestination(Target.position);

                if (distanceToPlayer <= stopDistance)
                {
                    // 일정 거리 이내에 있으면 멈추기
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
                // 플레이어가 20미터를 벗어나면 플레이어 앞으로 순간 이동
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
            isTracking = true; // 타겟을 찾았을 때 추적을 활성화
        }
    }

    void FreezeVelocity()
    {
        if (isTracking)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            // 포지션이 변화가 없다면 isRun을 false로 설정
            if (transform.position == lastPosition)
            {
                anim.SetBool("isRun", false);
            }

            lastPosition = transform.position;
        }
    }
    private bool GetCharacterActivationState()
    {
        // 여기에 캐릭터의 활성화 상태를 반환하는 코드를 구현해주세요.
        // 예를 들어, 캐릭터의 태그가 "Player"인지 여부를 확인할 수 있습니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        return playerObject != null && playerObject.activeSelf;
    }
}
