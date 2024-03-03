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

        // ��ư�� OnClick �̺�Ʈ �߰�
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
        // ��ư�� ����ϴ� �Լ�
        isChasing = !isChasing;

        if (isChasing)
        {
            // �±װ� "Enemy"�� ������Ʈ�� ã�Ƽ� Enemy�� ����
            GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyObject != null)
            {
                playerAgent.SetDestination(enemyObject.transform.position);
                StartCoroutine(FollowEnemyCoroutine());
            }
        }
        else
        {
            // �÷��̾� �̵� ����
            playerAgent.SetDestination(transform.position);
        }
    }

    private IEnumerator FollowEnemyCoroutine()
    {
        while (isChasing)
        {
            // �±װ� "Enemy"�� ������Ʈ�� Enemy�� ����ؼ� ����
            GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyObject != null)
            {
                playerAgent.SetDestination(enemyObject.transform.position);
            }

            yield return null;
        }

        // �̵��� �Ϸ�Ǹ� ����
        playerAgent.SetDestination(transform.position);
    }
}