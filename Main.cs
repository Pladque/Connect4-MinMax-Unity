using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

/*
 * TO ADD
 * Add possibilty to play AI againts herself
 * better check (finding floor for 3 in diagnals)
 * make sure if I covered all posiiblities
 * add 'if not' if there is 2+1 in row, to amout2 bc I dont want to count it twice
 * add 'if not' to not searching for 2 in (for example) column if I have already found 3 in column
 * make code less complex = cleaner
 * */

public class Main : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public Text text3_timer;
    public Text text4_depth;
    public Text text4_round_timer;
    public static int StartTurn = 1;
    public int Depth = 5;
    public bool AIvsAI = false;

    private int curr_colum;
    private int turn;
    private int movesNumber = 0;        //for increasing depth 

    private bool run = true;
    Stopwatch GameTimer = System.Diagnostics.Stopwatch.StartNew();


    void Start()
    {
        turn = StartTurn;
        GaneLogic.instance.init_board(SetupGrid.instance.width, SetupGrid.instance.height);
        SetupGrid.instance.DrawBoard();
        GameOutput.LightColumn((int)SetupGrid.instance.width / 2);
        curr_colum = (int)SetupGrid.instance.width / 2;

        if (turn == 1)
        {
            text1.text = "Player Turn";
        }
        else
        {
            text1.text = "AI Turn";
        }
        text2.text = "Moves: 0";
        text4_depth.text = "Starting depth*: " + Depth.ToString();
    }


    void Update()
    {
        if (run)
        {
            if (turn == 2)      // AI turn
            {
                Tuple<int, int> moveMadeByAI;

                if (movesNumber == 0)
                    moveMadeByAI = Tuple.Create(0, SetupGrid.instance.width / 2);
                else
                {
                    var MoveTimer = System.Diagnostics.Stopwatch.StartNew();
                    moveMadeByAI = AIScrpt.instance.AIMove(Depth, movesNumber, 2, 1);
                    MoveTimer.Stop();
                    var elapsed = MoveTimer.ElapsedMilliseconds / 1000.0;
                    text3_timer.text = "That move took:" + elapsed.ToString();
                }


                if (moveMadeByAI.Item1 != -1)
                {
                    GaneLogic.instance.SetBoardValue(moveMadeByAI.Item1, moveMadeByAI.Item2);
                    updateTurn();
                    GameOutput.ShowBoard(GaneLogic.instance.Board);
                    ShowWinner();

                }
            }
            else    //Player turn (or secound AI turn)
            {
                if (!AIvsAI)
                {
                    gameControlInputHandler();  //UpdateTurn inside input
                    GameOutput.ShowBoard(GaneLogic.instance.Board);
                    ShowWinner();
                }
                else
                {
                    Tuple<int, int> moveMadeByAI;

                    if (movesNumber == 0)
                        moveMadeByAI = Tuple.Create(0, SetupGrid.instance.width / 2);
                    else
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        moveMadeByAI = AIScrpt.instance.AIMove(Depth, movesNumber, 1, 2);
                        watch.Stop();
                        var elapsed = watch.ElapsedMilliseconds / 1000.0;
                        text3_timer.text = "That move took:" + elapsed.ToString();
                    }


                    if (moveMadeByAI.Item1 != -1)
                    {
                        GaneLogic.instance.SetBoardValue(moveMadeByAI.Item1, moveMadeByAI.Item2);
                        updateTurn();
                        GameOutput.ShowBoard(GaneLogic.instance.Board);
                        ShowWinner();
                    }
                }
            }
        }
        else
        {
            GameTimer.Stop();
            var elapsed = GameTimer.ElapsedMilliseconds / 1000.0;
            text4_round_timer.text = "Round time:" + elapsed.ToString();
        }

        /*
        /// DEBUG ///EvalRowsColumnDiagnals(int[,] board, bool debug_print = false, int rewardFor3InRow = 2)
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Debug.Log("wartosc 0 0");
            //Debug.Log(GaneLogic.instance.Board[0, 0]);
            AIScrpt.instance.EvalRowsColumnDiagnals(GaneLogic.instance.Board, true);
        }
        */

    }

    private void gameControlInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CircleAdder.AddCircle(curr_colum))
            {
                updateTurn();
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curr_colum--;
            GameOutput.LightColumn(curr_colum);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            curr_colum++;
            GameOutput.LightColumn(curr_colum);
        }
    }

    private void updateTurn()
    {
        movesNumber++;
        GaneLogic.instance.UpdateTurn();
        if (turn == 1 && !AIvsAI)
        {
            turn = 2;
            text1.text = "AI Turn";
        }
        else if (turn == 2 && !AIvsAI)
        {
            turn = 1;
            text1.text = "Player Turn";
        }
        else if(turn == 1 && AIvsAI)
        {
            turn = 2;
            text1.text = "Yellow AI turn";
        }
        else if(turn == 2 && AIvsAI)
        {
            turn = 1;
            text1.text = "Red AI turn";
        }

        text2.text = "Moves: " + movesNumber.ToString();
    }

    private void ShowWinner()
    {
        int winner = GaneLogic.instance.GetWinner(GaneLogic.instance.Board);
        if (winner == 2 && !AIvsAI)
        {
            text1.text = " AI WON!";
            text2.text = " Press R to Restart";
            run = false;
        }
        else if (winner == 1 && !AIvsAI)
        {
            text1.text = " PLAYER WON!";
            text2.text = " Press R to Restart";
            run = false;

        }
        else if (winner == 1 && AIvsAI)
        {
            text1.text = " RED AI WON!";
            text2.text = " Press R to Restart";
            run = false;

        }
        else if (winner == 2 && AIvsAI)
        {
            text1.text = " YELLOW AI WON!";
            text2.text = " Press R to Restart";
            run = false;
        }else if (winner == -1)
        {
            text1.text = "DRAW";
            text2.text = " Press R to Restart";
            run = false;
        }
    }



    /*
             
    */
}
