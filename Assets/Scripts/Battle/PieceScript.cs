using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PieceScript : MonoBehaviour
{

    public int x;

    public int y;

    public Sprite black;

    public Sprite white;

    public Text hintText;

    public void Step()
    {
        if (!BoardScript.MyTurn) return;
        BoardScript.MyTurn = false;
        GetComponent<Image>().sprite = BoardScript.Offensive ? black : white;
        LxcTools.EnableImage(GetComponent<Image>());
        GetComponent<Button>().enabled = false;
        hintText.text = "等待对手";
        Client.Step(x, y);
    }

    public void OpponentStep()
    {
        if (BoardScript.MyTurn) return;
        BoardScript.MyTurn = true;
        GetComponent<Image>().sprite = BoardScript.Offensive ? white : black;
        LxcTools.EnableImage(GetComponent<Image>());
        GetComponent<Button>().enabled = false;
        hintText.text = "该你了";
    }
    
}
