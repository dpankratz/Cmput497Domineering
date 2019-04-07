using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{

    public virtual AgentType Type
    {
        get { return AgentType.Unimplemented; }
    }

    public delegate void MoveChoiceCallback(Move move);

    public abstract void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback);

    public virtual void OnGameOverEvent(bool isWinner)
    {
        if (isWinner)
          Debug.LogFormat("{0} is the winner!",name);
    }

}
