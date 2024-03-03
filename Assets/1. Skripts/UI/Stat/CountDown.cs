using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDown : MonoBehaviour
{
    [SerializeField] Text countDownText;

    private float _setTime;
    public float setTime
    {
        get { return _setTime; }
        set
        {
            _setTime = value;
            // Update UI text when setTime is set
            countDownText.text = Mathf.Round(_setTime).ToString();
        }
    }

    // Awake is called when the script instance is being loaded
    void OnEnable()
    {
        // Ensure the countdown is reset when the object is enabled
        setTime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (setTime >= 0)
        {
            setTime -= Time.deltaTime;
        }
        else if (setTime < 0)
        {
            Time.timeScale = 0f;
        }
    }
}