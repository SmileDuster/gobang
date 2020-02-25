using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyReciver : MonoBehaviour
{

    private const string MatchedFirst = "mf";
    private const string MatchedSecond = "ms";
    private const string Logout = "lo";

    private Thread _receiver;

    public Text hintText;
    
    private delegate void UnityAction();
    
    private readonly Queue<UnityAction> _queue = new Queue<UnityAction>();

    private void Start()
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
                    case MatchedFirst:
                        action = WhenMatchedFirst;
                        exit = true;
                        break;
                    case MatchedSecond:
                        action = WhenMatchedSecond;
                        exit = true;
                        break;
                    case Logout:
                        action = WhenLogout;
                        exit = true;
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
        InvokeRepeating(nameof(Execute), 3, 1);
    }
    
    private void Execute()
    {
        lock (_queue)
        {
            if (_queue.Count > 0)
            {
                _queue.Dequeue().Invoke();
            }
        }
    }

    private void WhenMatchedFirst()
    {
        hintText.color = Color.green;
        hintText.text = "匹配成功";
        BoardScript.Offensive = true;
        SceneManager.LoadScene("Scenes/Battle");
    }

    private void WhenMatchedSecond()
    {
        hintText.color = Color.green;
        BoardScript.Offensive = false;
        hintText.text = "匹配成功";
        SceneManager.LoadScene("Scenes/Battle");
    }

    private void WhenLogout()
    {
        SceneManager.LoadScene("Scenes/Auth");
    }
    
}
