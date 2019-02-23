using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MctsNode {

    
    internal Move Move { get; private set; }
    internal MctsNode Parent { get; private set; }
    internal List<MctsNode> Children { get; private set; }

    internal bool IsRoot
    {
        get { return Parent == null; }
    }

    internal MctsNode(Move move, MctsNode parent)
    {
        Move = move;
        Children = new List<MctsNode>();
        Parent = parent;
    }

   


}
