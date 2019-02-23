using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MctsPlayer : Agent {

    private readonly MctsNode _root = new MctsNode(null,null);
    private MctsNode _currentNode;

    public override void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback)
    {
        var lastMove = board.ElapsedMoves[board.ElapsedMoves.Count - 1];
        if (lastMove == null)
        {
            //Game just started
            Init();
        }
    }

    public override void OnGameOverEvent(bool isWinner)
    {
        Init();
    }

    private void Init()
    {
        _currentNode = _root;
    }


}
 