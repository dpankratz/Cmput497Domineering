using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GreedyPlayer : Agent
{

    private int _wins = 0;
    private int _games = 0;

    public override void OnGameOverEvent(bool isWinner)
    {
        if (isWinner)
            _wins++;

        _games++;

        Debug.LogFormat("{0} has winrate {1} / {2}!", name, _wins, _games);
    }

    public override void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback)
    {
        var allMoves = board.GetAllValidMoves();

        var min = int.MaxValue;
        var max = 0;
        Move bestMove = null;
        foreach (var move in allMoves.Values)
        {
            var simulatedBoard = new Board(board);
            simulatedBoard.PlayMove(move);
            var numOpponenetMoves = simulatedBoard.GetAllValidMoves().Count;
            var numOurMoves = simulatedBoard.GetAllValidOpponentMoves().Count;
            if (numOpponenetMoves > min) continue;
            if (numOpponenetMoves == min && numOurMoves <= max) continue;
            min = numOpponenetMoves;
            max = numOurMoves;
            bestMove = move;


        }


        moveChoiceCallback(bestMove);
    }

}
