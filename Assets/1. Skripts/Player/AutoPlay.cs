using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AutoPlay : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private Button toggleButton;
    private bool isChasing = false;

    private void Start()
    {
        
        playerAgent = GetComponent<NavMeshAgent>();
        toggleButton = GetComponent<Button>();

        // 버튼에 OnClick 이벤트 추가
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleChase);
        }
        if (playerAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the 'Auto' game object.");
            return;
        }
    }

    private void ToggleChase()
    {
        // 버튼을 토글하는 함수
        isChasing = !isChasing;

        if (isChasing)
        {
            // 태그가 "Enemy"인 오브젝트를 찾아서 Enemy로 설정
            GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyObject != null)
            {
                playerAgent.SetDestination(enemyObject.transform.position);
                StartCoroutine(FollowEnemyCoroutine());
            }
        }
        else
        {
            // 플레이어 이동 중지
            playerAgent.SetDestination(transform.position);
        }
    }

    private IEnumerator FollowEnemyCoroutine()
    {
        while (isChasing)
        {
            // 태그가 "Enemy"인 오브젝트를 Enemy로 계속해서 쫓음
            GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyObject != null)
            {
                playerAgent.SetDestination(enemyObject.transform.position);
            }

            yield return null;
        }

        // 이동이 완료되면 정지
        playerAgent.SetDestination(transform.position);
    }
}