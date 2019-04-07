using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GreedyPlayer : Agent
{
    public override AgentType Type
    {
        get { return AgentType.Greedy; }
    }

    public override void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback)
    {
        var allMoves = board.GetAllValidMoves();

        var min = int.MaxValue;
        var max = 0;
        Move bestMove = new Move();
        foreach (var move in allMoves.Values)
        {
            var simulatedBoard = new Board(board);
            simulatedBoard.PlayMove(move);
            var numOpponentMoves = simulatedBoard.GetAllValidMoves().Count;
            var numOurMoves = simulatedBoard.GetAllValidOpponentMoves().Count;
            if (numOpponentMoves > min) continue;
            if (numOpponentMoves == min && (Random.value > 0.8f || numOurMoves <= max)) continue;
            min = numOpponentMoves;
            max = numOurMoves;
            bestMove = move;
        }
        moveChoiceCallback(bestMove);
    }
}
