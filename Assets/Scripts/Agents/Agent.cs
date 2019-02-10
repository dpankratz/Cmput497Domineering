using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public delegate void MoveChoiceCallback(Move move);

    public abstract void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback);

    public abstract void OnGameOverEvent(bool isWinner);
}
