using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    private int _wins = 0;
    private int _games = 0;

    public virtual AgentType Type
    {
        get { return AgentType.Unimplemented; }
    }

    public delegate void MoveChoiceCallback(Move move);

    public abstract void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback);

    public virtual void OnGameOverEvent(bool isWinner)
    {
        if (isWinner){
          Debug.LogFormat("{0} is the winner!",name);
          _wins++;
        }
        _games++;
        // Debug.LogFormat("{0} has winrate {1} / {2}!", name, _wins, _games);
    }

}
