using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MCTS;

public class MctsPlayer : Agent
{
    [SerializeField] private int _numSimulations = 56;

    public override AgentType Type
    {
        get { return AgentType.Mcts; }
    }

    public override void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback)
    {
        // var allMoves = board.GetAllValidMoves();
        // var keys = allMoves.Keys;
        // moveChoiceCallback(allMoves[keys[Random.Range(0, keys.Count)]]);
        Move move = new MCTS.UCT().getMove(board, _numSimulations);
        moveChoiceCallback(move);
    }
}
