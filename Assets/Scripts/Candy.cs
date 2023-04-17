using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int row;
    public int column;
    public CandyColor color;

    public void Initialize(int newRow, int newColumn, CandyColor newColor)
    {
        row = newRow;
        column = newColumn;
        color = newColor;
    }

    public void Fall(int newRow)
    {
        //TODO: implementare la caduta della caramella verso il basso
    }
}

public enum CandyColor
{
    Blue,
    Red,
    Green,
    Purple,
    Yellow,
    Orange
}
