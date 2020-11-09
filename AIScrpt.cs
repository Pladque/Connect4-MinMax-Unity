using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class AIScrpt : MonoBehaviour
{
    private int[,] board;
    public static AIScrpt instance;
    private int width;
    private int height;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        width = SetupGrid.instance.width;
        height = SetupGrid.instance.height;
        board = new int[width, height];
    }

    private int CheckNextMove(int turn, int[,] board, int depth, int alpha, int beta)
    {
        if (depth < 0)
        {
            return EvalRowsColumnDiagnals(board);
        }
        else
        {
            int winner = GaneLogic.instance.GetWinner(board);

            if (winner == 2) return 100000 + depth;     // + depth bc I want to win asap
            if (winner == 1) return -100000 - depth;    //bd I want to loose later
            if (winner == -1) return 0;                 //draw

        }

        int[] possibleMoves = allPossibleMoves(board);

        int[,] myBoard = new int [7,6];
        Array.Copy(board, board.GetLowerBound(0), myBoard, myBoard.GetLowerBound(0), board.Length);

        int bestScore;
        int i;
        int middle = possibleMoves.Length / 2;

        if (turn == 2)      //AI turn
        {
            bestScore = -Int32.MaxValue;
            for (int ind = 0; ind <= middle; ind++)
            {
                if (ind == 0)
                {
                    i = middle;
                    if (possibleMoves[i] != -1)
                    {
                        myBoard[i, possibleMoves[i]] = 2;

                        bestScore = Math.Max(CheckNextMove(1, myBoard, depth - 1, alpha, beta), bestScore);
                        alpha = Math.Max(alpha, bestScore);

                        if (beta <= alpha)
                            break;

                        myBoard[i, possibleMoves[i]] = 0;   //reseting board
                    }
                   
                }
                else
                {
                    i = middle - ind;
                    if (possibleMoves[i] != -1)
                    {
                        myBoard[i, possibleMoves[i]] = 2;

                        bestScore = Math.Max(CheckNextMove(1, myBoard, depth - 1, alpha, beta), bestScore);
                        alpha = Math.Max(alpha, bestScore);

                        if (beta <= alpha)
                            break;

                        myBoard[i, possibleMoves[i]] = 0;   //reseting board
                    }

                    i = middle + ind;
                    if (possibleMoves[i] != -1)
                    {
                        myBoard[i, possibleMoves[i]] = 2;

                        bestScore = Math.Max(CheckNextMove(1, myBoard, depth - 1, alpha, beta), bestScore);
                        alpha = Math.Max(alpha, bestScore);

                        if (beta <= alpha)
                            break;

                        myBoard[i, possibleMoves[i]] = 0;   //reseting board
                    }
                }
               
            }
        }
        else        //max score in else mean "minScore"
        {
            bestScore = Int32.MaxValue;
            for (int ind = 0; ind <= middle; ind++)
            {
                if (ind == 0)
                {
                    i = middle;
                    if (possibleMoves[i] != -1)
                    {
                        myBoard[i, possibleMoves[i]] = 1;

                        bestScore = Math.Min(CheckNextMove(2, myBoard, depth - 1, alpha, beta), bestScore);
                        beta = Math.Min(beta, bestScore);

                        if (beta <= alpha)
                            break;

                        myBoard[i, possibleMoves[i]] = 0;
                    }

                }
                else
                {
                    i = middle - ind;
                    if (possibleMoves[i] != -1)
                    {
                        myBoard[i, possibleMoves[i]] = 1;

                        bestScore = Math.Min(CheckNextMove(2, myBoard, depth - 1, alpha, beta), bestScore);
                        beta = Math.Min(beta, bestScore);

                        if (beta <= alpha)
                            break;

                        myBoard[i, possibleMoves[i]] = 0;
                    }

                    i = middle + ind;
                    if (possibleMoves[i] != -1)
                    {
                        myBoard[i, possibleMoves[i]] = 1;

                        bestScore = Math.Min(CheckNextMove(2, myBoard, depth - 1, alpha, beta), bestScore);
                        beta = Math.Min(beta, bestScore);

                        if (beta <= alpha)
                            break;

                        myBoard[i, possibleMoves[i]] = 0;
                    }
                }
            }
        }
        return bestScore;
    }

    private int[] allPossibleMoves(int[,] board)
    {
        int[] moves = new int[width];


        for (int j = 0; j < width; j++)
        {
            
            if (board[j, height - 1] == 0)
            {
                moves[j] = height - 1;
                for (int i = 0; i < height-1; i++)
                {
                    if (board[j, i] == 0)
                    {
                        moves[j] = i;
                        break;
                    }
                }
            }else moves[j] = -1;
        }

        return moves;
    }

    public Tuple<int, int> AIMove(int depth, int moveCounter = 0, int turn = 2, int enemy = 1)
    {
        if (moveCounter > 25)
        {
            depth += 8;
        }
        else if (moveCounter > 20)
        {
            depth += 4;
        }
        else if (moveCounter > 15)
        {
            depth+=2;
        }

        // copying board from gameLogic, bc I want Copy, not same board
        Array.Copy(GaneLogic.instance.Board, GaneLogic.instance.Board.GetLowerBound(0),
                                                    board, board.GetLowerBound(0), board.Length);

        int[] possibleMoves = allPossibleMoves(board);
        float tempScore;
        int bestScoreIndex = -1;
        bool lost_game = false;
        int winner;
        float bestScore;
        int i;
        int middle = possibleMoves.Length / 2;

        if (turn == 2)
            bestScore = -Int64.MaxValue;
        else if (turn == 1)
            bestScore = Int64.MaxValue;
        else bestScore = 0;

        for (int ind = 0; ind <= middle; ind++)
        {
            if (ind == 0)
            {
                i = middle;
                if (possibleMoves[i] != -1)
                {
                    board[i, possibleMoves[i]] = turn;
                    winner = GaneLogic.instance.GetWinner(board);
                    if (winner == turn)
                    {
                        bestScoreIndex = i;
                        break;
                    }
                    else if (winner == enemy)
                    {
                        lost_game = true;
                    }

                    if (!lost_game)
                    {
                        tempScore = CheckNextMove(enemy, board, depth - 1, Int32.MinValue, Int32.MaxValue);
                        if (tempScore > bestScore && turn == 2)
                        {
                            bestScore = tempScore;
                            bestScoreIndex = i;
                        }
                        else if (tempScore < bestScore && turn == 1)
                        {
                            bestScore = tempScore;
                            bestScoreIndex = i;
                        }
                    }
                    /// RESET BOARD ///
                    lost_game = false;
                    board[i, possibleMoves[i]] = 0;
                }
            }
            else
            {
                i = middle + ind;
                if (possibleMoves[i] != -1)
                {
                    board[i, possibleMoves[i]] = turn;
                    winner = GaneLogic.instance.GetWinner(board);
                    if (winner == turn)
                    {
                        bestScoreIndex = i;
                        break;
                    }
                    else if (winner == enemy)
                    {
                        lost_game = true;
                    }

                    if (!lost_game)
                    {
                        tempScore = CheckNextMove(enemy, board, depth - 1, Int32.MinValue, Int32.MaxValue);
                        if (tempScore > bestScore && turn == 2)
                        {
                            bestScore = tempScore;
                            bestScoreIndex = i;
                        }
                        else if (tempScore < bestScore && turn == 1)
                        {
                            bestScore = tempScore;
                            bestScoreIndex = i;
                        }
                    }
                    /// RESET BOARD ///
                    lost_game = false;
                    board[i, possibleMoves[i]] = 0;
                }
                i = middle - ind;
                if (possibleMoves[i] != -1)
                {
                    board[i, possibleMoves[i]] = turn;
                    winner = GaneLogic.instance.GetWinner(board);
                    if (winner == turn)
                    {
                        bestScoreIndex = i;
                        break;
                    }
                    else if (winner == enemy)
                    {
                        lost_game = true;
                    }

                    if (!lost_game)
                    {
                        tempScore = CheckNextMove(enemy, board, depth - 1, Int32.MinValue, Int32.MaxValue);
                        if (tempScore > bestScore && turn == 2)
                        {
                            bestScore = tempScore;
                            bestScoreIndex = i;
                        }
                        else if (tempScore < bestScore && turn == 1)
                        {
                            bestScore = tempScore;
                            bestScoreIndex = i;
                        }
                    }
                    /// RESET BOARD ///
                    lost_game = false;
                    board[i, possibleMoves[i]] = 0;
                }
            }
        }

        if (bestScoreIndex != -1)   //if it is -1 thats mean that are 0 moves for AI
        {
            int[] tempBestMove = {
            possibleMoves[bestScoreIndex],
            bestScoreIndex
            };
            return Tuple.Create(tempBestMove[0], tempBestMove[1]);
        }
        else
            return Tuple.Create(-1, -1);

    }


    public int EvalRowsColumnDiagnals(int[,] board, bool debug_print = false, int rewardFor3InRow = 5)
    {
        int sum = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (board[i, j] == 2)
                {
                    if (debug_print) Debug.Log(2);
                    sum += GetEvalValue(board, i, j, rewardFor3InRow, 2, debug_print);
                }
                else if (board[i, j] == 1)
                {
                    if (debug_print) Debug.Log(1);
                    sum -= GetEvalValue(board, i, j, rewardFor3InRow, 1, debug_print);
                }
            }
        }

        return sum;
    }

    /// DO NOT LOOK AT THAT CODE ///
    private int GetEvalValue(int[,] board, int i, int j, int rewardFor3InRow, int turn, bool debug_print)
    {
        int sum = 0;

        if (i > 0 && i < width - 3)
        {
            /// Checking rows ///
            if (board[i, j] == board[i + 1, j] && board[i + 1, j] == board[i + 2, j]) //  for X, X, X, 0 or 0, X, X, X
            {
                if (j > 0 && board[i + 3, j] == 0 && board[i + 3, j - 1] != 0)          //checking if there is floor
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("ROW 1_1");
                }
                else if (board[i - 1, j] == 0 && j > 0 && board[i - 1, j - 1] != 0)
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("ROW 1_321");
                }
                else if (j == 0 && (board[i + 3, j] == 0 || board[i - 1, j] == 0))      //checking on lowest level
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("ROW 1_321");
                }
                sum += 1; //reward for making opportunities
            }
            else if (board[i + 3, j] == turn)
            {
                if (board[i, j] == board[i + 1, j] && board[i + 2, j] == 0)      // for X, X, 0, X
                {
                    if (j > 0 && board[i + 2, j - 1] != 0)          //checking if there is floor
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("ROW 1_441");
                    }
                    else if (j == 0)        //checking on lowest level
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("ROW 1_165");
                    }
                    sum += 1; //reward for making opportunities
                }
                else if (board[i, j] == board[i + 2, j] && board[i + 1, j] == 0)         //for X, 0, X, X
                {
                    if (j > 0 && board[i + 1, j - 1] != 0)          //checking if there is floor
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("ROW 1_177");
                    }
                    else if (j == 0)        //checking on lowest level
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("ROW 1_67891");
                    }
                    sum += 1; //reward for making opportunities
                }
            }

            ///  Checking diagnals from bottom left to top right ///
            if (j > 0 && j < height - 3)
            {
                if (board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2]) //for X, X , X, 0 or 0, X, X, X
                {
                    if (board[i + 3, j + 3] == 0 && board[i + 3, j + 2] != 0)       //floor
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("DIAGNAL1_11");
                    }
                    else if (j > 1 && board[i - 1, j - 1] == 0 && board[i - 1, j - 2] != 0)  //floor
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("DIAGNAL1_12");
                    }
                    else if (j == 1 && board[i - 1, j - 1] == 0)        //flor on lovest lvl
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("DIAGNAL1_17");
                    }
                    sum += 1; //reward for making opportunities

                }
                else if (board[i + 3, j + 3] == turn)
                {
                    if (board[i, j] == board[i + 1, j + 1] && board[i + 2, j + 2] == 0)         //for X, X , 0, X
                    {
                        if (board[i + 2, j + 1] != 0)                             //floor
                        {
                            sum += rewardFor3InRow;
                            if (debug_print)
                                Debug.Log("DIAGNAL1_13");

                        }
                        sum += 1; //reward for making opportunities
                    }
                    else if (board[i, j] == board[i + 2, j + 2] && board[i + 1, j + 1] == 0)    // for X, 0 ,X, X
                    {
                        if (board[i + 1, j] != 0)                             //floor
                        {
                            sum += rewardFor3InRow;
                            if (debug_print)
                                Debug.Log("DIAGNAL1_14");
                        }
                        sum += 1; //reward for making opportunities
                    }
                }
            }
        }
        else if (i == 0 && j == 0)
        {
            //for X, X ,X, 0 or 0, X, X, X
            if (board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2] && board[i + 3, j + 3] == 0)
            {
                if (board[i + 3, j + 3] != 0)
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                    {
                        Debug.Log("DIAGNAL1_21");
                    }
                    sum += 1; //reward for making opportunities
                }
            }

        }
        /// bot left top right agan
        if (j == 0 && i < width - 3)
        {
            if (board[i + 3, j + 3] == turn)
            {
                if (board[i, j] == board[i + 1, j + 1] && 0 == board[i + 2, j + 2])          //for X, X , 0, X
                {
                    if (board[i + 2, j + 1] != 0)  //floor
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("DIAGNAL1_22");
                    }
                    sum += 1; //reward for making opportunities
                }
                else if (0 == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2])    //for X, 0 , X, X
                {
                    if (board[i + 1, j] != 0)
                    {
                        sum += rewardFor3InRow;
                        if (debug_print)
                            Debug.Log("DIAGNAL1_23");
                    }
                    sum += 1; //reward for making opportunities
                }
            }
        }
        /// checking row for i == 0
        if (i == 0 && j > 0)
            if (board[i, j] == board[i + 1, j] && board[i + 1, j] == board[i + 2, j] && board[i + 3, j] == 0) //  for X, X, X, 0 or 0, X, X, X
            {
                if (board[i + 3, j - 1] != 0)
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("ROW XXX");
                }
                sum += 1; //reward for making opportunities
            }

        /// Checking columns ///
        if (j < height - 3 && board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2] && board[i, j + 3] == 0)
        {
            sum += rewardFor3InRow;
            if (debug_print)
            {
                Debug.Log("COLUMN");
            }
        }


        // Checking diagnals from bot right to top left
        if (j < height - 3 && j > 0 && i > 2 && i < width - 1)
        {

            if (board[i, j] == board[i - 1, j + 1] && board[i, j] == board[i - 2, j + 2])
            {
                if (board[i - 3, j + 3] == 0 && board[i - 3, j + 3] != 0)       //floor
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("DIAGNAL2_113");
                }
                else if (j > 1 && board[i + 1, j - 1] == 0 && board[i + 1, j - 2] != 0)     //floor
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("DIAGNAL2_114");
                }
                sum += 1; //reward for making opportunities
            }
            else if (board[i, j] == board[i - 1, j + 1] && board[i - 2, j + 2] == 0 && board[i - 3, j + 3] == turn)
            {
                if (board[i - 2, j + 1] != 0)   //floor
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("DIAGNAL2_12");
                }
                sum += 1; //reward for making opportunities
            }
            else if (board[i, j] == board[i - 2, j + 2] && board[i - 1, j + 1] == 0 && (board[i - 3, j + 3] == turn || board[i + 1, j - 1] == turn))
            {
                if (board[i - 1, j] != 0)
                {
                    sum += rewardFor3InRow;
                    if (debug_print)
                        Debug.Log("DIAGNAL2_13");
                }
                sum += 1; //reward for making opportunities
            }

        }
        else if (j == 0 && i == width - 3 && board[i, j] == board[i - 1, j + 1] && board[i, j] == board[i - 2, j + 2] && board[i - 3, j + 3] == 0)
        {
            if (board[i - 3, j + 2] != 0)       //floor
            {
                sum += rewardFor3InRow;
                if (debug_print)
                    Debug.Log("DIAGNAL2_21");
            }
            sum += 1; //reward for making opportunities
        }
        else if (j == 0 && i < width - 3 && i > 2 && board[i, j] == board[i - 1, j + 1] && board[i, j] == board[i - 2, j + 2] && board[i - 3, j + 3] == 0)
        {
            sum += rewardFor3InRow;
            if (debug_print)
                Debug.Log("DIAGNAL2_21");
        }

        return sum;

    }
}
