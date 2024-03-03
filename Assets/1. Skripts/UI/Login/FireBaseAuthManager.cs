using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Unity.VisualScripting;
using System;
public class FireBaseAuthManager
{
    private static FireBaseAuthManager instance = null;

    public static FireBaseAuthManager Instance //싱글톤
    {
        get
        {
            if(instance == null)
            {
                instance = new FireBaseAuthManager();
            }
            return instance;
        }
    }
    private FirebaseAuth auth;//로그인 / 회원가입 등에 사용
    private FirebaseUser user;//인증이 완료된 유저 정보

    public string UserId => user.UserId;
    // Start is called before the first frame update
    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        if(auth.CurrentUser != null)
        {
            LogOut();
        }
        auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if (!signed && user !=null)
            {
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }
            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("로그인");
                LoginState?.Invoke(true);
            }
        }
    }
    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 실패");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
                return;
                
            }

            FirebaseUser newUser;
            AuthResult result = task.Result;
            newUser = result.User;
            
            Debug.LogError("회원가입 완료");
        });
    }
    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 실패");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                return;

            }

            FirebaseUser newUser;
            AuthResult result = task.Result;
            newUser = result.User;

            Debug.LogError("로그인 완료");
        });
    }
    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}
