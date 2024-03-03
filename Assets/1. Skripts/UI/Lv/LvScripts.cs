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
        // Level 텍스트 컴포넌트를 찾아서 가져옵니다.
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
            // GameManager.Instance.level 값을 가져와서 Level 텍스트를 업데이트
            int currentLevel = GameManager.Instance.level;
            levelText.text = currentLevel.ToString();
        }
    }
}
