using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthReceiver : MonoBehaviour
{

    private const string LoginSuccess = "ls";
    private const string WrongAuth = "e";
    private const string ExistUsername = "eu";
    
    private Thread _receiver;

    private delegate void UnityAction();

    private readonly Queue<UnityAction> _queue = new Queue<UnityAction>();

    public Text hintText;

    public Button submitButton;

    void Start()
    {
        _receiver = new Thread(() =>
        {
            while (true)
            {
                var response = Client.Receive();
                var exit = false;
                UnityAction action = null;
                switch (response)
                {
                    case LoginSuccess:
                        action = WhenLoginSuccess;
                        exit = true;
                        break;
                    case WrongAuth:
                        action = WhenWrongAuth;
                        break;
                    case ExistUsername:
                        action = WhenExistUsername;
                        break;
                }
                lock (_queue)
                {
                    if (action != null) 
                    {
                        _queue.Enqueue(action);
                    }
                }
                if (exit)
                {
                    return;
                }
            }
        });
        _receiver.Start();
        InvokeRepeating(nameof(Execute), 5, 2);
    }

    private void Execute()
    {
        if (!Client.ServerEnable)
        {
            hintText.color = Color.red;
            hintText.text = "服务器未开机";
        }
        lock (_queue)
        {
            if (_queue.Count > 0)
            {
                _queue.Dequeue().Invoke();
            }
        }
    }

    private void WhenLoginSuccess()
    {
        hintText.color = Color.green;
        hintText.text = "登录成功";
        SceneManager.LoadScene("Scenes/Lobby");
    }

    private void WhenWrongAuth()
    {
        hintText.color = Color.red;
        hintText.text = "错误的用户名或密码";
        submitButton.enabled = true;
    }

    private void WhenExistUsername()
    {
        hintText.color = Color.red;
        hintText.text = "用户名已存在";
        submitButton.enabled = true;
    }
    
}
