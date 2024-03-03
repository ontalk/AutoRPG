using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : Enemy
    {
        public GameObject missile;
        public GameObject enemyObject;
        public Transform missilePortA;
        public Transform missilePortB;
        public Transform[] missilePorts; // 여러 미사일 포트를 배열로 정의
        public BoxCollider meleeArea;
        Vector3 tauntVec;

        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            boxcollider = GetComponent<BoxCollider>();
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
            isLook = true;
            //Target = GameObject.FindWithTag("Player").transform;
            nav.isStopped = true;
        FindAndSetTarget();
        StartCoroutine(BossPatern());
        }
        void Update()
        {
            if (Target == null)
            {
                FindAndSetTarget();
            }
            if (isDead)
            {
                StopAllCoroutines();
                return;//아래에 있는것들을 실행을 멈춤 그냥 반환한다는 뜻.
            }
            if (isLook && Target != null)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v);
                transform.LookAt(Target.position + lookVec);
            }
            else if(isDead)
                 nav.SetDestination(tauntVec);
            }

    IEnumerator BossPatern()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            int RandomPatern = Random.Range(0, 6);
            switch (RandomPatern)
            {
                //Taunt Patern
                case 0:
                case 1:
                    StartCoroutine(MissileShot());
                    break;
                //바위
                case 2:
                case 3:
                    StartCoroutine(Rock());
                    break;
                //타운트
                case 4:
                    StartCoroutine(Taunt());
                    break;
                //휴식
                case 5:
                    StartCoroutine(SpawnEnemy());
                    break;
                case 6:
                    break;

            }
            yield return new WaitForSeconds(2f);

        }
    }

    IEnumerator MissileShot()
    {
        float missileSpeed = 20.0f;

        for (int i = 0; i < missilePorts.Length; i++)
        {
            yield return new WaitForSeconds(0.2f);

            GameObject instantMissile = Instantiate(missile, missilePorts[i].position, missilePorts[i].rotation);
            Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
            missileRigid.velocity = missilePorts[i].forward * missileSpeed;
        }

        yield return new WaitForSeconds(2f);
    }

    IEnumerator SpawnEnemy() //미사일 포트에서 몬스터 생성.
    {
        
        GameObject instantMissileA = Instantiate(enemyObject, missilePortA.position, missilePortA.rotation);
        Enemy bossMissileA = instantMissileA.GetComponent<Enemy>();
        bossMissileA.Target = Target;

        GameObject instantMissileB = Instantiate(enemyObject, missilePortB.position, missilePortB.rotation);
        Enemy bossMissileB = instantMissileA.GetComponent<Enemy>();
        bossMissileA.Target = Target;

        yield return new WaitForSeconds(2f);
    }

    IEnumerator Rock()
    {
        isLook = false;
        anim.SetTrigger("doRock");
        Instantiate(bullet, transform.position, transform.rotation);

        yield return new WaitForSeconds(3f);
        isLook = true;
    }

    IEnumerator Taunt()
    {
        tauntVec = Target.position + lookVec;
        isLook = false;
        nav.isStopped = false;//네비게이션 풀기
        boxcollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        boxcollider.enabled = true;
        nav.isStopped = true;//네비게이션 다시 설정하기
    }
}

