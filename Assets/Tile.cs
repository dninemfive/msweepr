﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool IsMine { get; private set; } = false;
    private bool revealed = false;
    public IEnumerable<Tile> Neighbors
    {
        get
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    Tile item = Board[X + i, Y + j];
                    if (item != null) yield return item;
                }
            }
        }
    }
    public int NeighborMineCount => Neighbors.Where(x => x.IsMine).Count();
    public Board Board => Game.Board;
    public int X { get; private set; }
    public int Y { get; private set; }
    public FlagStatus Flag;
    public Text Text;
    public enum FlagStatus
    {
        None = 0,
        Flagged = 1,
        Question = 2
    }
    public void Init(int x, int y, bool isMine)
    {
        X = x;
        Y = y;
        IsMine = isMine;
    }
    // https://answers.unity.com/questions/1350065/onmousedown-for-right-mouse.html
    private void OnMouseOver()
    {
        if (revealed) return;
        if(Input.GetMouseButtonDown(0))
        {
            Reveal();
        }
        if(Input.GetMouseButtonDown(1))
        {
            RotateFlag();
        }
    }
    public void Reveal()
    {
        if (revealed) return;
        Text.text = "";
        revealed = true;
        transform.Rotate(new Vector3(0, 0, 180));
        // rotate the text to keep it right-side-up
        Text.transform.Rotate(new Vector3(0, 0, 180));
        if (IsMine)
        {
            Text.text = "*";
            Game.End();
            return;
        }
        if(NeighborMineCount > 0)
        {
            Text.text = "" + NeighborMineCount;
            Text.color = TextColor(NeighborMineCount);
            return;
        }
        foreach (Tile t in Neighbors)
        {
            t.Reveal();
        }
    }
    //                                      1  2     3     4     5     6     7      8
    //static readonly float[] colorValues = { 0, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 0.95f, 1f };
    public static Color TextColor(int numAdjacents)
    {
        // 50 is a magic number which makes sure the color cycles so adjacent numbers are a bit more distinguishable
        float h = Mathf.Clamp01((numAdjacents * 110f));
        float s = numAdjacents % 2 == 0 ? 1f : 0.5f;
        //float v = colorValues[numAdjacents - 1];
        return Color.HSVToRGB(h, 1, 1);
    }
    public void RotateFlag()
    {
        Flag = (FlagStatus)(((int)Flag + 1) % 3);
        switch (Flag)
        {
            case FlagStatus.None:
                Text.text = "";
                return;
            case FlagStatus.Flagged:
                Text.text = "!";
                return;
            case FlagStatus.Question:
                Text.text = "?";
                return;
        }
    }
}
