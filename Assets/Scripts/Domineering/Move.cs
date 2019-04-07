using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public static Move InvalidMove = new Move()
    {
        Location = new Vector2Int(-2, -2)
    };

    //Note that the move location is the upper left location of the tile.
    //i.e. if the tile is vertical is the location of the top half of the tile.
    public Vector2Int Location;
    public Orientation Orientation;

    public override string ToString()
    {
        return string.Format("{0}, {1}, ({2})", Location, Location + new Vector2Int((int)Orientation, 1 - (int)Orientation), Orientation);
    }

    public static bool operator ==(Move c1, Move c2)
    {
        return c1.Location.x == c2.Location.x && c1.Location.y == c2.Location.y && c1.Orientation == c2.Orientation;
    }

    public static bool operator !=(Move c1, Move c2)
    {
        return !(c1 == c2);
    }

    public int Value(Board board)
    {
      var simulatedBoard = new Board(board);
      simulatedBoard.PlayMove(this);
      var numMoves = simulatedBoard.GetAllValidOpponentMoves().Count;
      var numOpponentMoves = simulatedBoard.GetAllValidMoves().Count;
      return numMoves - numOpponentMoves;
    }
}


public enum Orientation
{
    Vertical = 0,
    Horizontal = 1
}
