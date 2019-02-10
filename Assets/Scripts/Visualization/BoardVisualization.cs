using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardVisualization : MonoBehaviour
{    
    [SerializeField] private Transform _piecesRoot;
    [SerializeField] private GameObject _piecePrefab;
    [SerializeField] private Transform _gridOrigin;
    //Keep this as a child of board to enable resizing
    [SerializeField] private Transform _gridOffsets;
    [SerializeField] private Transform _gridRotationOffset;

    [SerializeField] private bool _isUsingPlayerColors = true;
    [SerializeField] private Color[] _playerColors;

    private float _tileWidth; //short side 


    public void VisualizeBoard(Board board)
    {
        foreach(Transform piece in _piecesRoot)
            Destroy(piece.gameObject);

        if (!board.IsStoringMoves)
        {
            Debug.LogError("Couldn't visual the board. It is not tracking moves");
            return;
        }

        foreach (var move in board.ElapsedMoves)        
            GenerateNewPiece(move);            
        
    }

    private Vector3 GetWorldPositionFromMove(Move move)
    {
        var location = move.Location;

        var rotationOffset = move.Orientation == Orientation.Vertical
            ? (_gridRotationOffset.position - _gridOrigin.position)
            : Vector3.zero;


        //NOTE: The board is horizontal in dimension x and vertical in z.
        //Y dimension here represents distance between the bottom of the tile and the top of the board                
        return _gridOrigin.position + new Vector3(location.x * _tileWidth, 0, location.y 
        * _tileWidth) + rotationOffset;
    }

    private GameObject GenerateNewPiece(Move move)
    {
        var piece = Instantiate(_piecePrefab, _piecesRoot);
        piece.transform.position = GetWorldPositionFromMove(move);
        if (move.Orientation == Orientation.Vertical)        
            piece.transform.Rotate(0, 90, 0);                
        piece.GetComponent<MeshRenderer>().material.color = _playerColors[(int) move.Orientation];
        return piece;
    }

    private void Start()
    {
        _tileWidth = _gridOffsets.position.z - _gridOrigin.position.z;     
    }

}
