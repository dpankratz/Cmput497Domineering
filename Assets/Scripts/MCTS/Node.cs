using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This class holds the node logic.
public class Node : IComparable<Node>
{
    public Move PrevMove { get; private set; }
    public Node Parent { get; private set; }
    public bool IsLeftPlayerMove { get; private set; }
    public List<Node> Children = new List<Node>();
    public int Wins { get; private set; }
    public int Visits { get; private set; }
    public SortedList<int, Move> UntriedMoves = null;

    public Node(Move move, Node parent, Board state)
    {
        PrevMove = move;
        Parent = parent;
        IsLeftPlayerMove = state.IsLeftPlayerMove;
        UntriedMoves = state.GetAllValidMoves();
    }

    public Node UCTSelectChild()
    {
        Node best = Children[0];
        var UTCK = 2.0f;
        foreach (Node node in Children) if
        (node.Wins / node.Visits + UTCK * Math.Sqrt(2 * Math.Log(Visits) / node.Visits) >
         best.Wins / best.Visits + UTCK * Math.Sqrt(2 * Math.Log(Visits) / best.Visits)) best = node;
        return best;
    }

    public Node AddChild(Move move, Board state)
    {
        Node child = new Node(move, this, state);
        UntriedMoves.Remove(state.GetSortableIndexFromMove(move));
        Children.Add(child);
        return child;
    }

    public void Update(int result)
    {
        Visits += 1;
        Wins += result;
    }

    public int CompareTo(Node other)
    {
        return Visits - other.Visits;
    }
}
