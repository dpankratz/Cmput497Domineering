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

    internal Agent AgentOne
    {
        get { return _agents[0]; }
    }

    internal Agent AgentTwo
    {
        get { return _agents[1]; }
    }

	void Start ()
	{	 
        if(_agents.Length != 2)
            Debug.LogError("GameManager does not have 2 agents!");
        ResetGame();
	}
	
	
	void Update ()
	{
	    if (Time.time - _lastMoveTime >= _minimumTimeBetweenMoves && _nextMove != Move.InvalidMove && !_currentBoard.IsGameOver)
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
        _agents[0].gameObject.SetActive(false);
        _agents[1].gameObject.SetActive(false);
        _agents = agents;
        _agents[0].gameObject.SetActive(true);
        _agents[1].gameObject.SetActive(true);
        ResetGame();
    }

    private void ResetGame()
    {
        _currentBoard = new Board(Board.DefaultDimensions, true);
        _boardVisualization.VisualizeBoard(_currentBoard);
        _lastMoveTime = Time.time + _minimumTimeBetweenMoves * 2;
        _nextMove = Move.InvalidMove;
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
        _nextMove = Move.InvalidMove;
        if (_currentBoard.IsGameOver)
        {
            int winnerIndex = _currentBoard.GetWinner();

            var winnerOrientation = (winnerIndex == 0) ? Orientation.Vertical : Orientation.Horizontal;

            _boardVisualization.FlashDominos(winnerOrientation);

            _agents[    winnerIndex].OnGameOverEvent(true);
            _agents[1 - winnerIndex].OnGameOverEvent(false);
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
