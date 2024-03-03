using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{

    public InputField email;
    public InputField password;

    public Text outputText;

    // Start is called before the first frame update
    void Start()
    {
        FireBaseAuthManager.Instance.LoginState += OnChangedState;
        FireBaseAuthManager.Instance.Init();
    }
    private void OnChangedState(bool sign)
    {
        outputText.text = sign ? "로그인 : " : "로그아웃 : ";
        outputText.text += FireBaseAuthManager.Instance.UserId;
    }

    public void Create()
    {
        string e = email.text;
        string p = password.text;

        FireBaseAuthManager.Instance.Create(e, p);
    }

    public void LogIn()
    {
        FireBaseAuthManager.Instance.Login(email.text, password.text);
    }

    public void LogOut()
    {
        FireBaseAuthManager.Instance.LogOut();
    }
}
