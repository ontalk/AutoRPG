using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpawn : MonoBehaviour
{
    public GameObject petPrefab; // ���� �������� �Ҵ����ּ���.
    private GameObject petInstance; // ���� ������ ���� �ν��Ͻ��� �����մϴ�.

    private void Start()
    {
        // ������ ���۵� �� ���� �����մϴ�.
        SpawnPet();
    }

    private void Update()
    {
        // ���⿡ ĳ������ Ȱ��ȭ/��Ȱ��ȭ ���¸� �����ϴ� �ڵ带 �߰����ּ���.
        bool isCharacterActive = GetCharacterActivationState(); // ĳ���� Ȱ��ȭ ���¸� Ȯ���ϴ� �Լ��� ȣ���մϴ�.

        if (isCharacterActive)
        {
            if (petInstance == null)
            {
                SpawnPet(); // ĳ���Ͱ� Ȱ��ȭ�ǰ�, ���� �������� �ʾҴٸ� ���� �����մϴ�.
            }
        }
        else
        {
            if (petInstance != null)
            {
                DestroyPet(); // ĳ���Ͱ� ��Ȱ��ȭ�ǰ�, ���� ������ ���¶�� ���� �ı��մϴ�.
            }
        }
    }

    private void SpawnPet()
    {
        if (petPrefab != null)
        {
            petInstance = Instantiate(petPrefab, transform.position, Quaternion.identity);
            // ���� �÷��̾� ��ó�� �����մϴ�. ��ġ �� ȸ���� ��Ȳ�� ���� ������ �ʿ��� �� �ֽ��ϴ�.
        }
        else
        {
            Debug.LogError("PetPrefab�� �Ҵ���� �ʾҽ��ϴ�. PetPrefab�� �Ҵ����ּ���.");
        }
    }

    private void DestroyPet()
    {
        if (petInstance != null)
        {
            Destroy(petInstance);
            petInstance = null;
        }
    }

    private bool GetCharacterActivationState()
    {
        // ���⿡ ĳ������ Ȱ��ȭ ���¸� ��ȯ�ϴ� �ڵ带 �������ּ���.
        // ���� ���, ĳ������ �±װ� "Player"���� ���θ� Ȯ���� �� �ֽ��ϴ�.
        return gameObject.activeSelf;
    }
}
