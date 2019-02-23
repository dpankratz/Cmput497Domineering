using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardVisualization _boardVisualization;
    [SerializeField] private Agent[] _agents;
    [SerializeField] private float _minimumTimeBetweenMoves;
    [SerializeField] private float _minimumTimeBetweenGames;

    private Board _currentBoard;
    private float _lastMoveTime;
    private float _lastGameTime;
    private Move _nextMove;
    private int _agentIndex;

	void Start ()
	{	 
        if(_agents.Length != 2)
            Debug.LogError("GameManager does not have 2 agents!");
        ResetGame();
	}
	
	
	void Update ()
	{
	    if (Time.time - _lastMoveTime >= _minimumTimeBetweenMoves && _nextMove != null && !_currentBoard.IsGameOver)
	    {
            CommitMove();
	    }

	    if (Time.time - _lastGameTime >= _minimumTimeBetweenGames && _currentBoard.IsGameOver)
	    {
            ResetGame();
	    }
    }

    public void ResetGameWithNewAgents(Agent[] agents)
    {
       
        _agents = agents;
        ResetGame();
    }

    private void ResetGame()
    {
        _currentBoard = new Board(Board.DefaultDimensions, true);
        _boardVisualization.VisualizeBoard(_currentBoard);
        _lastMoveTime = Time.time + _minimumTimeBetweenMoves * 2;
        _nextMove = null;
        _agentIndex = 0;
        GetNextMoveFromAgent();

    }

    private void GetNextMoveFromAgent()
    {
        _agents[_agentIndex++ % _agents.Length].OnMyMoveEvent(_currentBoard, ReceiveMoveFromAgent);
    }

    private void CommitMove()
    {
        _currentBoard.PlayMove(_nextMove);
        _boardVisualization.VisualizeBoard(_currentBoard);
        _nextMove = null;
        if (_currentBoard.IsGameOver)
        {
            //If left to play then right is winner and vice versa
            var winnerIndex = _currentBoard.IsLeftPlayerMove ? 1 : 0;

            var winnerOrientation = (winnerIndex == 0) ? Orientation.Vertical : Orientation.Horizontal;

            _boardVisualization.FlashDominos(winnerOrientation);

            for (var i = 0; i < _agents.Length; i++)
            {
                _agents[i].OnGameOverEvent(i == winnerIndex);                
            }
            _lastGameTime = Time.time;
            return;
        }
        GetNextMoveFromAgent();
        
        _lastMoveTime = Time.time;
        
    }


    private void ReceiveMoveFromAgent(Move move)
    {
        _nextMove = move;
    }
}
