using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSender : MonoBehaviour
{

    public Button surrenderButton;

    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(Back);
        surrenderButton.onClick.AddListener(Surrender);
        LxcTools.DisableTextButton(backButton, 0, false);
    }

    private void Surrender()
    {
        surrenderButton.enabled = false;
        Client.Surrender();
        LxcTools.EnableTextButton(backButton);
        LxcTools.DisableTextButton(surrenderButton, 0.3f, false);
    }

    private void Back()
    { 
        SceneManager.LoadScene("Scenes/Lobby");
    }
    
}
