using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardVisualization : MonoBehaviour
{
    private readonly int FlashHash = Animator.StringToHash("Flash");
    
    [SerializeField] private Transform _piecesRightRoot;
    [SerializeField] private Transform _piecesLeftRoot;
    [SerializeField] private GameObject _piecePrefab;
    [SerializeField] private Transform _gridOrigin;
    //Keep this as a child of board to enable resizing
    [SerializeField] private Transform _gridOffsets;
    [SerializeField] private Transform _gridRotationOffset;

    [SerializeField] private Color[] _playerColors;
    

    private float _tileWidth; //short side 
    private readonly Dictionary<Move,GameObject> _selectedMoves = new Dictionary<Move,GameObject>();
    

    public void VisualizeBoard(Board board)
    {
        foreach(Transform piece in _piecesRightRoot)
            Destroy(piece.gameObject);
        foreach (Transform piece in _piecesLeftRoot)
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
        var piece = Instantiate(_piecePrefab, (move.Orientation == Orientation.Vertical ? _piecesLeftRoot : _piecesRightRoot));
        piece.transform.position = GetWorldPositionFromMove(move);
        if (move.Orientation == Orientation.Vertical)        
            piece.transform.Rotate(0, 90, 0);                
        piece.GetComponent<MeshRenderer>().material.color = _playerColors[(int) move.Orientation];
        return piece;
    }

    public Vector2Int GetLocationFromWorldPosition(Vector3 position, Orientation orientation)
    {
        var x = position.x - _gridOrigin.position.x + (orientation == Orientation.Horizontal ? -0.5f : 0);                   
        var y = position.z - _gridOrigin.position.z + (orientation == Orientation.Vertical ? -1 : -0.5f);
        return new Vector2Int(Mathf.CeilToInt(x), Mathf.CeilToInt(y));
    }

    public void SelectMove(Move move)
    {
        var newPiece = GenerateNewPiece(move);
        var material = newPiece.GetComponent<MeshRenderer>().material;
        material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a /2f);
        newPiece.transform.localScale *= 0.9f;
        _selectedMoves[move] = newPiece;
    }
    
    public void ClearSelectedMove(Move move)
    {
        if (!_selectedMoves.ContainsKey(move))
            return;
        Destroy(_selectedMoves[move]);
        _selectedMoves.Remove(move);        
    }

    public void FlashDominos(Orientation winner)
    {

        foreach(Transform domino in (winner == Orientation.Vertical ? _piecesLeftRoot : _piecesRightRoot))
            domino.GetComponent<Animator>().SetTrigger(FlashHash);        
        
    }

    private void Start()
    {
        _tileWidth = _gridOffsets.position.z - _gridOrigin.position.z;     
    }

}
