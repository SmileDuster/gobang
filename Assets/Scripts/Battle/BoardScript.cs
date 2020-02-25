using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour
{

    public Text hintText;
    
    public Sprite black;

    public Sprite white;
    
    public static bool Offensive;

    public static bool MyTurn;

    public const int BoardSize = 15;

    private const int BoardLength = 504;

    private const int PieceLength = 36;

    public GameObject pieceTemplate;

    private readonly PieceScript[,] _pieces = new PieceScript[BoardSize+1, BoardSize+1];

    private void Start()
    {
        hintText.text = Offensive ? "该你了" : "等待对手";
        for (var i = 1; i <= BoardSize; i++)
        {
            for (var j = 1; j <= BoardSize; j++)
            {
                var piece = Instantiate(pieceTemplate, transform, true);
                piece.transform.localScale = Vector3.one;
                var pieceTransform = piece.transform.localPosition;
                pieceTransform.x = -BoardLength / 2 + (i - 1) * PieceLength;
                pieceTransform.y = -BoardLength / 2 + (j - 1) * PieceLength;
                pieceTransform.z = 1;
                piece.transform.localPosition = pieceTransform;
                var pieceButton = piece.GetComponent<Button>();
                var pieceScript = piece.AddComponent<PieceScript>();
                pieceScript.x = i;
                pieceScript.y = j;
                pieceScript.hintText = hintText;
                pieceScript.black = black;
                pieceScript.white = white;
                pieceButton.onClick.AddListener(pieceScript.Step);
                _pieces[i,j] = pieceScript;
                MyTurn = Offensive;
            }
        }
    }

    public void OpponentStep(int x, int y) 
    {
        _pieces[x, y].OpponentStep();
    }

}
