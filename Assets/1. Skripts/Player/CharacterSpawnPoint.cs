using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CharacterSpawnPoint : MonoBehaviour
{
    public GameObject characterPrefab; // 플레이어 프리팹을 인스펙터에서 설정
    public MainCamera cameraFollow; // 카메라를 따라다니는 스크립트 (선택 사항)

    private GameObject spawnedCharacter; // 생성된 플레이어

    // 캐릭터가 죽을 때 발생하는 이벤트
    void Start()
    {
        // 게임 시작 시 플레이어 생성
        
        if(!GameManager.Instance.isGameOver)
        {
            SpawnCharacter(true);
        }
    }

    public void SpawnCharacter(bool revive)
    {
        // 생성된 플레이어가 이미 있다면 리턴합니다.
        if (spawnedCharacter != null)
        {
            return;
        }

        if (revive && GameManager.Instance.curHeart > 0) // revive가 true이고 현재 목숨이 0보다 큰 경우에만 캐릭터를 생성합니다.
        {
            // 플레이어를 spawnPoint 위치에 생성
            spawnedCharacter = Instantiate(characterPrefab, transform.position, transform.rotation);

            // 게임 매니저(GameManager)에서 플레이어의 초기 상태를 설정
            GameManager.Instance.curHealth = GameManager.Instance.maxHealth;

            // 카메라를 따라다니는 스크립트에 플레이어를 설정 (선택 사항)
            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(spawnedCharacter.transform);
            }
        }
        else
        {
                // 생성된 플레이어가 있다면 비활성화
            if (spawnedCharacter != null)
            {
                spawnedCharacter.SetActive(false);
            }
        }
     }

        public void TeleportCharacter(GameObject player)
        {
        // 플레이어를 spawnPoint 위치로 텔레포트
        player.transform.position = transform.position;

        // 플레이어 활성화
        player.SetActive(true);

        // 게임 매니저(GameManager)에서 플레이어의 초기 상태를 설정
        GameManager.Instance.curHealth = GameManager.Instance.maxHealth;

        // 카메라를 따라다니는 스크립트에 플레이어를 설정 (선택 사항)
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(player.transform);
        }
    }
}
