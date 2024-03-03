using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LvScripts : MonoBehaviour
{

    public Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        // Level �ؽ�Ʈ ������Ʈ�� ã�Ƽ� �����ɴϴ�.
        levelText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevelText();
    }

    public void UpdateLevelText()
    {
        if (levelText != null)
        {
            // GameManager.Instance.level ���� �����ͼ� Level �ؽ�Ʈ�� ������Ʈ
            int currentLevel = GameManager.Instance.level;
            levelText.text = currentLevel.ToString();
        }
    }
}
