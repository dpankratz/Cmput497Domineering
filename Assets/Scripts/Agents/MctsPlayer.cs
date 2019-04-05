using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MCTS;

public class MctsPlayer : Agent
{

    [SerializeField] private int _numSimulations = 56;

    private int _wins = 0;
    private int _games = 0;


    public override AgentType Type
    {
        get { return AgentType.Mcts; }
    }

    public override void OnGameOverEvent(bool isWinner)
        {
            if (isWinner)
                _wins++;

            _games++;

            Debug.LogFormat("{0} has winrate {1} / {2}!", name, _wins, _games);
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
