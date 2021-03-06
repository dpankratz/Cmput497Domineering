using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This class holds the domineering game logic.
public class Board
{
    private const int InvalidBoardPosition = -1;

    public static readonly Vector2Int DefaultDimensions = new Vector2Int(8,8);

    public Vector2Int Dimensions { get; private set; }

    //This stores the current board state. Notice that a given position in domineering can be in exactly 2 states thus lending itself to a bitarray.
    public BitArray BoardState { get; private set; }

    public bool IsStoringMoves { get; private set; }

    public bool IsLeftPlayerMove { get; private set; } //i.e. vertical

    public bool IsGameOver
    {
        get
        {
            if (_cachedVerticalMoveList == null || _cachedHorizontalMoveList == null)
                GetAllValidMoves();
            var validMovesForNext = (IsLeftPlayerMove) ? _cachedVerticalMoveList.Count : _cachedHorizontalMoveList.Count;
            return validMovesForNext == 0;
        }
    }

    public Orientation NextMoveOrientation
    {
        get { return (IsLeftPlayerMove) ? Orientation.Vertical : Orientation.Horizontal; }
    }

    public readonly List<Move> ElapsedMoves = new List<Move>();

    private SortedList<int, Move> _cachedVerticalMoveList = null;
    private SortedList<int, Move> _cachedHorizontalMoveList = null;

    public Board(Vector2Int dimensions,bool shouldStoreElapsedMoves)
    {
        Dimensions = dimensions;
        IsStoringMoves = shouldStoreElapsedMoves;
        BoardState = new BitArray(Dimensions.x * Dimensions.y);
        IsLeftPlayerMove = true;
        Reset();
    }

    public Board(Board board)
    {
        Dimensions = board.Dimensions;
        IsStoringMoves = false;
        BoardState = (BitArray) board.BoardState.Clone();
        IsLeftPlayerMove = board.IsLeftPlayerMove;
        Reset();
    }

    public void Reset()
    {

        ElapsedMoves.Clear();
        
        _cachedVerticalMoveList = _cachedHorizontalMoveList = null;
    }

    public override string ToString()
    {
        var boardString = "";
        for (var y = 0; y < Dimensions.y; y++)
        {
            for (var x = 0; x < Dimensions.x; x++)
            {
                boardString += BoardState[y * Dimensions.x + x] ? "1" : "0";
            }
            boardString += "\n";
        }
        return boardString;
    }

    public SortedList<int,Move> GetAllValidMoves()
    {
        if (_cachedVerticalMoveList != null || _cachedHorizontalMoveList != null)
            return (NextMoveOrientation == Orientation.Vertical) ? _cachedVerticalMoveList : _cachedHorizontalMoveList;

        var validVerticalMoves = new SortedList<int, Move>();
        var validHorizontalMoves = new SortedList<int, Move>();

        //Only iterate to dimension size - 1 in either direction since a move uses the top left of a 1x2 domino
        for (var y = 0; y < Dimensions.y; y++)
        {
            for (var x = 0; x < Dimensions.x; x++)
            {
                if (y < Dimensions.y - 1)
                {
                    var candidateMoveVertical = new Move(){
                        Location = new Vector2Int(x, y),
                        Orientation = Orientation.Vertical
                    };

                    if (!IsMoveOverlapping(candidateMoveVertical))
                        validVerticalMoves.Add(GetSortableIndexFromMove(candidateMoveVertical), candidateMoveVertical);
                }

                if (x < Dimensions.x - 1)
                {
                    var candidateMoveHorizontal = new Move(){
                        Location = new Vector2Int(x, y),
                        Orientation = Orientation.Horizontal
                    };

                    if (!IsMoveOverlapping(candidateMoveHorizontal))
                        validHorizontalMoves.Add(GetSortableIndexFromMove(candidateMoveHorizontal), candidateMoveHorizontal);
                }
            }
        }
        _cachedVerticalMoveList = validVerticalMoves;
        _cachedHorizontalMoveList = validHorizontalMoves;

        return (NextMoveOrientation == Orientation.Vertical) ? _cachedVerticalMoveList : _cachedHorizontalMoveList;
    }

    public SortedList<int, Move> GetAllValidOpponentMoves()
    {
        GetAllValidMoves();
        return (NextMoveOrientation == Orientation.Vertical) ? _cachedHorizontalMoveList : _cachedVerticalMoveList ;
    }

    public Move GetValidMove()
    {
        //This is okay because we're using a cache
        var moves = GetAllValidMoves();
        return moves[moves.Keys[Random.Range(0,moves.Count)]];
    }

    public bool IsMoveValid(Move move)
    {
        if (IsGameOver)
            return false;

        if (move.Orientation != NextMoveOrientation)
            return false;

        return !IsMoveOverlapping(move);
    }

    private bool IsMoveOverlapping(Move move)
    {
        if (!IsLocationEmpty(move.Location))
            return true;

        return (move.Orientation == Orientation.Vertical)
            ? !IsLocationEmpty(move.Location + Vector2Int.up)
            : !IsLocationEmpty(move.Location + Vector2Int.right);
    }

    public bool IsLocationEmpty(Vector2Int location)
    {
        return !BoardState[GetBoardIndexFromLocation(location)];
    }

    public void PlayMove(Move move)
    {
        if (!IsMoveValid(move))
        {
            Debug.LogErrorFormat("Board received invalid move! {0}",move);
            return;
        }

        if(IsStoringMoves)
            ElapsedMoves.Add(move);

        var otherLocation = move.Location + (move.Orientation == Orientation.Vertical
                                ? Vector2Int.up
                                : Vector2Int.right);

        var topLeftIndex = GetBoardIndexFromLocation(move.Location);
        var otherIndex = GetBoardIndexFromLocation(otherLocation);

        BoardState[topLeftIndex] = true;
        BoardState[otherIndex] = true;
        RemoveMovesFromCache(move.Location);
        RemoveMovesFromCache(otherLocation);

        IsLeftPlayerMove = !IsLeftPlayerMove;
    }

    private void RemoveMovesFromCache(Vector2Int location)
    {
        //Update cached moves
        if (_cachedVerticalMoveList == null || _cachedHorizontalMoveList == null)
            return;

        //Remove the moves in the exact spot the domino was placed
        var verticalKey = GetSortableIndexFromMove(new Move(){Location = location, Orientation = Orientation.Vertical});
        var horizontalKey = GetSortableIndexFromMove(new Move() { Location = location, Orientation = Orientation.Horizontal });
        _cachedVerticalMoveList.Remove(verticalKey);
        _cachedHorizontalMoveList.Remove(horizontalKey);

        //Remove the moves that instersect with the domino
        var downKey = GetSortableIndexFromMove(new Move(){Location = location + Vector2Int.down, Orientation = Orientation.Vertical});
        var leftKey = GetSortableIndexFromMove(new Move(){Location = location + Vector2Int.left, Orientation = Orientation.Horizontal});
        _cachedVerticalMoveList.Remove(downKey);
        _cachedHorizontalMoveList.Remove(leftKey);
    }

    public bool IsLocationWithinBoard(Vector2Int location)
    {
        return location.x >= 0 && location.x <= Dimensions.x && location.y >= 0 && location.y <= Dimensions.y;
    }

    private int GetBoardIndexFromLocation(Vector2Int location)
    {
        return Dimensions.y * location.y + location.x;
    }

    public int GetSortableIndexFromMove(Move move)
    {
        var location = move.Location;
        if (!IsLocationWithinBoard(location))
            return InvalidBoardPosition;
        return (move.Orientation == Orientation.Horizontal ? Dimensions.x * Dimensions.y : 0) + Dimensions.y * location.y + location.x;
    }

    public Board DeepCopy()
    {
      Board copy = new Board(Dimensions, false);
      copy.BoardState = (BitArray) BoardState.Clone();
      copy.IsLeftPlayerMove = IsLeftPlayerMove;
      return copy;
    }

    public int GetWinner()
    {
      //If left to play then right is winner and vice versa
      return IsLeftPlayerMove ? 1 : 0;
    }

    public int GetWinner(bool IsLeft)
    {
      return IsLeftPlayerMove == IsLeft ? 1 : 0;
    }
}
