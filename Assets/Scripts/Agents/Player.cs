using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent
{    
    [SerializeField] private Camera _camera;
    [SerializeField] private BoardVisualization _visualization;
    [SerializeField] private BoxCollider _boardBoxCollider;
    [SerializeField] private Board _board;
    private Vector2Int _visualizedLocation = new Vector2Int(-1,-1);
    private SortedList<int, Move> _validMoves = null;
    private Move _visualizedMove = null;

    private bool _isSelectingMove = false;

    private MoveChoiceCallback _moveChoiceCallback = null;

    public override AgentType Type
    {
        get { return AgentType.Human; }
    }

    void OnEnable()
    {        
        _visualizedMove = null;
        _isSelectingMove = false;
        _validMoves = null;
        _moveChoiceCallback = null;
    }

    void Update ()
	{
    

	    if (_visualizedMove != null)
	    {
	        if (InputManager.GetMouseButtonDown(0))
	        {                
	            _moveChoiceCallback(_visualizedMove);
	            _isSelectingMove = false;
                _visualization.ClearSelectedMove(_visualizedMove);
	            _visualizedMove = null;
	        }
	    }

	    //Only visualize if allowed to move.
        if (!_isSelectingMove)
	        return;

        var ray = _camera.ScreenPointToRay(InputManager.mousePosition);
	    RaycastHit hit;
	    if (_boardBoxCollider.Raycast(ray, out hit,100f))
	    {

	        var hitLocation = _visualization.GetLocationFromWorldPosition(hit.point,_board.NextMoveOrientation);

            
	        if (hitLocation == _visualizedLocation)
	            return;
	       
            if(_visualizedMove != null)
                _visualization.ClearSelectedMove(_visualizedMove);
            _visualizedLocation = hitLocation;  

	        var candidateMove = new Move(_visualizedLocation, _board.NextMoveOrientation);

	        _visualizedMove = candidateMove;

	        foreach (var kvp in _validMoves)
	        {
	            var move = kvp.Value;
	            if (move.Equals(candidateMove))
	            {
	                _visualization.SelectMove(candidateMove);
	                return;
	            }
	        }

	        _visualizedMove = null;
	    }   
	}

    public override void OnMyMoveEvent(Board board, MoveChoiceCallback moveChoiceCallback)
    {
        _isSelectingMove = true;
        _validMoves = board.GetAllValidMoves();
        _board = board;
        _moveChoiceCallback = moveChoiceCallback;
    }


    public override void OnGameOverEvent(bool isWinner)
    {
        if(isWinner)
            Debug.LogFormat("{0} is the winner!",name);

        _visualizedMove = null;
        _isSelectingMove = false;
        _validMoves = null;
        _moveChoiceCallback = null;
    }


}
