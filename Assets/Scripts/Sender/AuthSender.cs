using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthSender : MonoBehaviour
{
    private int _mode = 1;

    public Button submitButton;

    public Button loginButton;

    public Button registerButton;

    public Button guestButton;

    private Text _submitText;

    public InputField usernameField;

    public InputField passwordField;

    public Text hintText;

    public Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(Exit);
        _submitText = submitButton.GetComponentInChildren<Text>();
        submitButton.onClick.AddListener(Submit);
        loginButton.onClick.AddListener(() =>
        {
            SetMode(1);
        });
        registerButton.onClick.AddListener(() =>
        {
            SetMode(2);
        });
        guestButton.onClick.AddListener(() =>
        {
            SetMode(3);
        });
    }

    private void SetMode(int mode)
    {
        _mode = mode;
        switch (_mode)
        {
            case 1:
                _submitText.text = "登录";
                usernameField.text = "";
                passwordField.text = "";
                usernameField.enabled = true;
                passwordField.enabled = true;
                LxcTools.DisableTextButton(registerButton, 0.3f, true);
                LxcTools.DisableTextButton(guestButton, 0.3f, true);
                LxcTools.EnableTextButton(loginButton);
                break;
            case 2:
                _submitText.text = "注册并登录";
                usernameField.text = "";
                passwordField.text = "";
                usernameField.enabled = true;
                passwordField.enabled = true;
                LxcTools.DisableTextButton(loginButton, 0.3f, true);
                LxcTools.DisableTextButton(guestButton, 0.3f, true);
                LxcTools.EnableTextButton(registerButton);
                break;
            case 3:
                _submitText.text = "游客身份登录";
                usernameField.enabled = false;
                passwordField.enabled = false;
                LxcTools.DisableTextButton(loginButton, 0.3f, true);
                LxcTools.DisableTextButton(registerButton, 0.3f, true);
                LxcTools.EnableTextButton(guestButton);
                break;
        }

        hintText.text = null;
    }

    private void Submit()
    {
        submitButton.enabled = false;
        switch (_mode)
        {
            case 1:
                Login();
                break;
            case 2:
                Register();
                break;
            case 3:
                Guest();
                break;
        }
    }

    private void Login()
    {
        var username = usernameField.text;
        if (username.Equals(""))
        {
            hintText.color = Color.red;
            hintText.text = "用户名为空";
            return;
        }
        var password = passwordField.text;
        if (password.Equals(""))
        {
            hintText.color = Color.red;
            hintText.text = "密码为空";
            return;
        }
        hintText.color = Color.white;
        hintText.text = "正在登录中";
        Client.Name = username;
        Client.Login(username, password);
    }

    private void Register()
    {
        var username = usernameField.text;
        if (username.Equals(""))
        {
            hintText.color = Color.red;
            hintText.text = "用户名为空";
            return;
        }
        var password = passwordField.text;
        if (password.Equals(""))
        {
            hintText.color = Color.red;
            hintText.text = "密码为空";
            return;
        }
        hintText.color = Color.white;
        hintText.text = "正在登录中";
        Client.Name = username;
        Client.Register(username, password);
    }

    private void Guest()
    {
        hintText.color = Color.white;
        hintText.text = "正在登录中";
        Client.Name = "游客";
        Client.Guest();
    }

    private void Exit()
    {
        Application.Quit();
    }
    
}
