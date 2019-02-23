using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

//This class holds the node logic.
public class Node : IComparable<Node>
{
  public Move PrevMove { get; private set; }
  public Node Parent { get; private set; }
  public Board State { get; private set; }
  public List<Node> Children = new List<Node>();
  public int Wins { get; private set; }
  public int Visits { get; private set; }
  public List<Move> UntriedMoves = new List<Move>();

  public class NodeComparer : IComparer<Node>
  {
    public NodeComparer (int argument)
    {
      Argument = argument;
    }
    public int Argument { get ; private set; }
    public int Compare(Node x, Node y)
    {
      double UCB1x = x.Wins/x.Visits + Math.Sqrt(2*Math.Log(Argument)/x.Visits);
      double UCB1y = y.Wins/y.Visits + Math.Sqrt(2*Math.Log(Argument)/y.Visits);
      return (UCB1x > UCB1y) ? 1 : -1;
    }
  }

  public Node(Move move, Node parent, Board state)
  {
    PrevMove = move;
    Parent = parent;
    State = state;
  }

  public Node UCTSelectChild()
  {
    return Children.OrderBy(x => x, new NodeComparer(Visits)).ToList()[Children.Count - 1];
  }

  public Node AddChild(Move move, Board state)
  {
    Node child = new Node(move, this, state);
    UntriedMoves.Remove(move);
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
    return (Visits > other.Visits) ? 1 : -1;
  }
}
