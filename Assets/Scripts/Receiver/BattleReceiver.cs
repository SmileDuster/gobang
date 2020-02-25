using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleReceiver : MonoBehaviour
{

    private const string Win = "w";
    private const string SurrenderWin = "sw";
    private const string Lose = "l";
    private const string Draw = "d";

    private Thread _receiver;

    public Text hintText;

    public Button backButton;

    public BoardScript board;

    private int _lastX;

    private int _lastY;
    
    private delegate void UnityAction();
    
    private readonly Queue<UnityAction> _queue = new Queue<UnityAction>();

    void Start()
    {
        backButton.enabled = false;
        _receiver = new Thread(() =>
        {
            while (true)
            {
                var response = Client.Receive();
                var exit = false;
                UnityAction action = null;
                switch (response)
                {
                    case Win:
                        action = WhenWin;
                        exit = true;
                        break;
                    case SurrenderWin:
                        action = WhenSurrenderWin;
                        exit = true;
                        break;
                    case Lose:
                        action = WhenLose;
                        exit = true;
                        break;
                    case Draw:
                        action = WhenDraw;
                        exit = true;
                        break;
                    default:
                        if (response != null)
                        {
                            try
                            {
                                var code = int.Parse(response);
                                var x = code / 100;
                                var y = code % 100;
                                if (x <= BoardScript.BoardSize && x > 0 && y <= BoardScript.BoardSize && y > 0)
                                {
                                    Interlocked.Exchange(ref _lastX, x);
                                    Interlocked.Exchange(ref _lastY, y);
                                    action = WhenStep;
                                }
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
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
        InvokeRepeating(nameof(Execute), 2, 0.5f);
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

    private void WhenStep()
    {
        board.OpponentStep(_lastX, _lastY);
    }

    private void WhenWin()
    {
        hintText.color = Color.green;
        hintText.text = "胜利";
        LxcTools.EnableTextButton(backButton);
    }
    
    private void WhenLose()
    {
        hintText.color = Color.red;
        hintText.text = "战败";
        LxcTools.EnableTextButton(backButton);
    }
    
    private void WhenDraw()
    {
        hintText.color = Color.yellow;
        hintText.text = "平局";
        LxcTools.EnableTextButton(backButton);
    }
    
    private void WhenSurrenderWin()
    {
        hintText.color = Color.green;
        hintText.text = "对方投降了";
        LxcTools.EnableTextButton(backButton);
    }
    
}
