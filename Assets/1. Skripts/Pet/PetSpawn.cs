using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpawn : MonoBehaviour
{
    public GameObject petPrefab; // 펫의 프리팹을 할당해주세요.
    private GameObject petInstance; // 현재 생성된 펫의 인스턴스를 저장합니다.

    private void Start()
    {
        // 게임이 시작될 때 펫을 생성합니다.
        SpawnPet();
    }

    private void Update()
    {
        // 여기에 캐릭터의 활성화/비활성화 상태를 감지하는 코드를 추가해주세요.
        bool isCharacterActive = GetCharacterActivationState(); // 캐릭터 활성화 상태를 확인하는 함수를 호출합니다.

        if (isCharacterActive)
        {
            if (petInstance == null)
            {
                SpawnPet(); // 캐릭터가 활성화되고, 펫이 생성되지 않았다면 펫을 생성합니다.
            }
        }
        else
        {
            if (petInstance != null)
            {
                DestroyPet(); // 캐릭터가 비활성화되고, 펫이 생성된 상태라면 펫을 파괴합니다.
            }
        }
    }

    private void SpawnPet()
    {
        if (petPrefab != null)
        {
            petInstance = Instantiate(petPrefab, transform.position, Quaternion.identity);
            // 펫을 플레이어 근처에 생성합니다. 위치 및 회전은 상황에 따라 조절이 필요할 수 있습니다.
        }
        else
        {
            Debug.LogError("PetPrefab이 할당되지 않았습니다. PetPrefab을 할당해주세요.");
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
        // 여기에 캐릭터의 활성화 상태를 반환하는 코드를 구현해주세요.
        // 예를 들어, 캐릭터의 태그가 "Player"인지 여부를 확인할 수 있습니다.
        return gameObject.activeSelf;
    }
}
