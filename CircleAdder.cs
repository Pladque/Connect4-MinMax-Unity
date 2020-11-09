using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAdder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool AddCircle(int column)
    {
        int[] colBoard = new int[SetupGrid.instance.height];

        colBoard = GaneLogic.instance.GetColumn(column);
        for (int i = colBoard.Length-1; i>0; i--)
        {
            if (colBoard[i] == 0 && colBoard[i-1] != 0)
            {
                GaneLogic.instance.SetBoardValue(i, column);
                return true;
            }
        }

        if (colBoard[colBoard.Length - 1] ==0)
        {
            GaneLogic.instance.SetBoardValue(0, column);
            return true;
        }

        return false;
    }
}
