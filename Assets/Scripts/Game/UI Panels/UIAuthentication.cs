using System;
using System.Net.Mime;
using TMPro;
using UI;
using UnityEngine;
using EventData;
using Unity.VisualScripting;

public class UIAuthentication : BaseUI
{
    [Header("Events")] [SerializeField] private GameEvent onLoginClick;
    [SerializeField] private GameEvent onResetPasswordClick;
    [SerializeField] private GameEvent onRegisterClick;
    [SerializeField] private GameEvent autoLogin;

    [Header("Login")] [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField loginUsername;
    [SerializeField] private TMP_InputField loginPassword;

    [Header("Register")] [SerializeField] private GameObject registerPanel;
    [SerializeField] private TMP_InputField registerUsername;
    [SerializeField] private TMP_InputField registerEmail;
    [SerializeField] private TMP_InputField registerPassword;

    [Header("Reset")] [SerializeField] private GameObject resetPasswordPanel;
    [SerializeField] private TMP_InputField accountUsername;


    private void Start()
    {
        if (PlayerPrefs.GetInt("PlayfabRememberMe", 0) == 1)
        {
            Invoke(nameof(AutoLogin), .5f);
        }
    }

    private void AutoLogin()
    {
        var loginInfo = new AutoLoginInfo
        {
            AutoLoginFailCallback = AutoLoginFail,
            AutoLoginSuccessCallback = AutoLoginSuccess
        };
        autoLogin.Invoke(this, loginInfo);
    }

    private static void AutoLoginFail()
    {
        Debug.Log("Login Failed");
        // UI Manager Raise Warning
    }

    private void AutoLoginSuccess()
    {
        Debug.Log("Login Success");
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
        UIManager.Instance.ShowUI(UIIndex.UINavigationTab);
    }


    public void OnLoginClick()
    {
        var loginInfo = new LoginInfo
        {
            Username = loginUsername.text,
            Password = loginPassword.text,
            LoginFailCallback = OnLoginFail,
            LoginSuccessCallback = OnLoginSuccess
        };
        onLoginClick.Invoke(this, loginInfo);
    }


    public void OnRegisterClick()
    {
        var registerInfo = new RegisterInfo
        {
            Username = registerUsername.text,
            Password = registerPassword.text,
            Email = registerEmail.text,
            RegisterSuccessCallback = OnRegisterSuccess,
            RegisterFailCallback = OnRegisterFail
        };
        onRegisterClick.Invoke(this, registerInfo);
    }

    public void OnResetPasswordClick()
    {
    }

    private void OnLoginFail()
    {
        Debug.Log("Login Failed");
        // UI Manager Raise Warning
    }

    private void OnLoginSuccess()
    {
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
        UIManager.Instance.ShowUI(UIIndex.UINavigationTab);
    }

    private static void OnRegisterFail()
    {
        Debug.Log("Register Failed");
    }

    private void OnRegisterSuccess()
    {
        Debug.Log("Register Success");
        RegisterToLogin();
    }

    public void RegisterToLogin()
    {
        if (registerPanel.activeSelf)
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
            loginPanel.transform.SetAsLastSibling();
        }
    }

    public void LoginToRegister()
    {
        if (loginPanel.activeSelf)
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            registerPanel.transform.SetAsLastSibling();
        }
    }

    public void ResetToLogin()
    {
        if (registerPanel.activeSelf)
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
            loginPanel.transform.SetAsLastSibling();
        }
    }

    public void LoginToReset()
    {
        if (loginPanel.activeSelf)
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            registerPanel.transform.SetAsLastSibling();
        }
    }
}