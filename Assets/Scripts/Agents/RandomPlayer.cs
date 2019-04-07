using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayer : Agent
{

    public override AgentType Type
    {
        get { return AgentType.Random; }
    }

    public override void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback)
    {
         var allMoves = board.GetAllValidMoves();
         var keys = allMoves.Keys;
         moveChoiceCallback(allMoves[keys[Random.Range(0, keys.Count)]]);
    }

}
