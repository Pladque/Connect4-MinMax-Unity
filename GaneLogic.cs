using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaneLogic : MonoBehaviour
{
    public int[,] Board { get; private set; }
    public static GaneLogic instance;
    [HideInInspector]
    public int turn;

    private void Awake()
    {
        instance = this;
        turn = Main.StartTurn;
    }


    public void init_board(int width, int height)
    {
        Board = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Board[i, j] = 0;
            }
        }
    }

    public int[] GetColumn(int column)
    {
        int[] colBoard = new int[SetupGrid.instance.height];

        for (int j = 0; j < SetupGrid.instance.height; j++)
        {
            colBoard[j] = Board[column, j];
        }

        return colBoard;
    }

    public void SetBoardValue(int row, int column)
    {
        Board[column, row] = turn;
    }

    public void UpdateTurn()
    {
        if (turn == 1) turn = 2;
        else turn = 1;
    }

    public int GetWinner(int[,] board)
    {
        int width = SetupGrid.instance.width;
        int height = SetupGrid.instance.height;
        bool AnyZeroes = false;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (board[i, j] != 0)
                {
                    if (j < height - 3)
                    {
                        // Checking columns
                        if (board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2] &&
                            board[i, j] == board[i, j + 3])
                        {
                            return board[i, j];
                        }

                        // Checking diagnals from bot. right to to left
                        if (i > 2 && board[i, j] == board[i - 1, j + 1] && board[i, j] == board[i - 2, j + 2] &&
                        board[i, j] == board[i - 3, j + 3])
                        {
                            return board[i, j];
                        }

                    }

                    if (i < width - 3)
                    {
                        // Checking diagnals from bot legt to top right
                        if (j < height - 3 && board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2] &&
                            board[i, j] == board[i + 3, j + 3])
                        {
                            return board[i, j];
                        }

                        // Checking rows
                        if (board[i, j] == board[i + 1, j] && board[i + 1, j] == board[i + 2, j] &&
                        board[i + 2, j] == board[i + 3, j])
                        {
                            return board[i, j];
                        }
                    }
                }
                else AnyZeroes = true;
            }
        }

        if (!AnyZeroes) return -1;      //means draw


        return 0;
    }



}
