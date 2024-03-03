using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class MainCamera : MonoBehaviour
{
    public Transform target; // ī�޶��� Ÿ��
    public float rotationSpeed = 3.0f; // ī�޶� ȸ�� �ӵ�
    public float distance = 5.0f; // ī�޶�� Ÿ�� ������ �Ÿ�
    public float scrollSpeed = 2.0f; // 마우스 휠 스크롤 속도
    private float minDistance = 7;
    private float maxDistance = 20; 
    private float currentRotationX = 0.0f;
    private float currentRotationY = 0.0f;

    public GameObject menuSet;

    private bool isCursorLocked = true;

    public GameObject[] targetObjects;

    static public MainCamera instance;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindAndSetPlayerTarget(); // �ʱ⿡ �÷��̾ �����ϵ��� ����
        
    }
    
    private void Update()
    {
        if (target != null)
        {
            // 마우스 휠 스크롤 입력을 감지하여 distance 값 조절
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scrollInput * scrollSpeed, minDistance, maxDistance);

            // 나머지 코드는 그대로 유지
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentRotationX -= mouseY;
            currentRotationX = Mathf.Clamp(currentRotationX, -80, 80);
            currentRotationY += mouseX;

            Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
            Vector3 offset = new Vector3(0, 0, -distance);
            Vector3 targetPosition = target.position + rotation * offset;

            float minHeight = 1.0f;
            if (targetPosition.y < minHeight)
            {
                targetPosition.y = minHeight;
            }

            transform.position = targetPosition;
            transform.LookAt(target.position);
        }

        MouseCursor();
    }

    // ĳ���͸� ���� ������� �����ϴ� �޼���
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void MouseCursor()
    {
        // 배열에 있는 모든 게임 오브젝트를 순회하며 처리
        foreach (GameObject targetObject in targetObjects)
        {
            // 게임 오브젝트가 켜져있고, 그 태그가 OnUI인지 확인
            if (targetObject.activeInHierarchy && targetObject.CompareTag("OnUI"))
            {
                // 마우스 커서 보이게
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // 중단하고 바로 반환할 수 있음 (이 코드는 한 번만 실행되어야 함)
                return;
            }
        }

        // 여기까지 왔다면 어떤 OnUI 오브젝트도 켜져있지 않음
        // 그래서 마우스 커서 안보이게
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // �÷��̾ ã�Ƽ� ���� ������� �����ϴ� �޼���
    public void FindAndSetPlayerTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            SetTarget(player.transform);
        }
        //else
        //{
        //    Debug.LogWarning("Player not found in the scene.");
        //}
    }

    // ī�޶� ���� �������� �̵� ������ ��ȯ�ϴ� �޼���
    public Vector3 GetMoveDirection(float horizontalInput, float verticalInput)
    {
        Vector3 cameraForward = transform.forward;
        cameraForward.y = 0; // ���� ���� �̵� ����
        cameraForward.Normalize();

        return cameraForward * verticalInput + transform.right * horizontalInput;
    }
}
