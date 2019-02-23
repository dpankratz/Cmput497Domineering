using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MctsSearchManager : MonoBehaviour {
    public MctsSearchManager Instance { get; private set; }

    private MctsNode _root;
    private MctsNode _currentNode;

    private void Awake()
    {
        Instance = this;
        _root = new MctsNode(null, null);
        _currentNode = _root;
    }

    public void MoveToNode(Move move)
    {
        foreach (var node in _currentNode.Children)
        {
            if (node.Move.Equals(move))
            {
                _currentNode = node;
                return;
            }            
        }
        _currentNode = new MctsNode(move,_currentNode);
    }




}
