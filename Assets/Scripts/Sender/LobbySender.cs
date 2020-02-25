using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySender : MonoBehaviour
{

    public Text welcomeText;

    public InputField numberText;

    public Button matchButton;

    public Button roomButton;

    public Button quitButton;

    public Button logoutButton;

    public Text hintText;

    private bool _matching;

    private void Start()
    {
        matchButton.onClick.AddListener(Match);
        roomButton.onClick.AddListener(Room);
        quitButton.onClick.AddListener(Quit);
        logoutButton.onClick.AddListener(Logout);
        welcomeText.text = "欢迎 " + Client.Name;
    }

    private void Match()
    {
        if (_matching)
        {
            LxcTools.EnableTextButton(roomButton);
            LxcTools.EnableTextButton(quitButton);
            LxcTools.EnableTextButton(logoutButton);
            hintText.text = "取消了匹配";
        }
        else
        {
            LxcTools.DisableTextButton(roomButton, 0.3f, false);
            LxcTools.DisableTextButton(logoutButton, 0.3f, false);
            LxcTools.DisableTextButton(quitButton, 0.3f, false);
            hintText.text = "开始随机匹配";
        }
        _matching = !_matching;
        Client.Match();
    }

    private void Room()
    {
        if (_matching)
        {
            LxcTools.EnableTextButton(matchButton);
            LxcTools.EnableTextButton(quitButton);
            LxcTools.EnableTextButton(logoutButton);
            numberText.enabled = true;
            hintText.text = "取消了匹配";
        }
        else
        {
            if (numberText.text.Equals(""))
            {
                hintText.text = "房间号为空";
                return;
            }
            LxcTools.DisableTextButton(matchButton, 0.3f, false);
            LxcTools.DisableTextButton(logoutButton, 0.3f, false);
            LxcTools.DisableTextButton(quitButton, 0.3f, false);
            numberText.enabled = false;
            hintText.text = "等待玩家加入 " + numberText.text;
        }
        _matching = !_matching;
        Client.Room(numberText.text);
    }
    
    private void Quit()
    {
        Application.Quit();
    }

    private void Logout()
    {
        Client.Logout();
    }
    
}
