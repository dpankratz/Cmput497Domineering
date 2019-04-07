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
        Move bestMove = new Move();
        foreach (var move in allMoves.Values)
        {
            var simulatedBoard = new Board(board);
            simulatedBoard.PlayMove(move);
            var numOpponentMoves = simulatedBoard.GetAllValidMoves().Count;            
            if (numOpponentMoves > min) continue;            
            min = numOpponentMoves;            
            bestMove = move;
        }
        moveChoiceCallback(bestMove);
    }
}
