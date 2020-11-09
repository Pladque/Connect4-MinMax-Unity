using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameOutput : MonoBehaviour
{
    public static void LightColumn(int column)
    {
        foreach (GameObject hole in SetupGrid.instance.AllHoles)
        {
            if (Math.Abs(hole.GetComponent<Hole>().X) == column && hole.GetComponent<SpriteRenderer>().color == Color.white)
            {
                hole.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            else if (Math.Abs(hole.GetComponent<Hole>().X) != column && hole.GetComponent<SpriteRenderer>().color == Color.gray)
            {
                hole.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public static void ShowBoard(int[,] board)
    {
        foreach (GameObject hole in SetupGrid.instance.AllHoles)
        {
            if (board[hole.GetComponent<Hole>().X, hole.GetComponent<Hole>().Y] == 1)
            {
                hole.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (board[hole.GetComponent<Hole>().X, hole.GetComponent<Hole>().Y] == 2)
            {
                hole.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        
    }

}
