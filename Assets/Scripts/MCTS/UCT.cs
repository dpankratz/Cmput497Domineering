using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MCTS
{
    public class UCT
    {
        public Move getMove(Board rootBoard, int itermax)
        {
            Node rootNode = new Node(Move.InvalidMove, null, rootBoard.DeepCopy());
            for (int i = 0; i < itermax; i++)
            {
                Node node = rootNode;
                Board board = rootBoard.DeepCopy();

                while (node.UntriedMoves.Count == 0 & node.Children.Count != 0)
                {
                    node = node.UCTSelectChild();
                    board.PlayMove(node.PrevMove);
                }

                if (node.UntriedMoves.Count != 0)
                {
                    int MoveKey = node.UntriedMoves.Keys[Random.Range(0, node.UntriedMoves.Count)];
                    board.PlayMove(node.UntriedMoves[MoveKey]);
                    node = node.AddChild(node.UntriedMoves[MoveKey], board);
                }

                while (board.GetAllValidMoves().Count != 0)
                    board.PlayMove(board.GetValidMove());

                while (node != null)
                {
                    if (board.IsGameOver) node.Update(board.GetWinner());
                    node = node.Parent;
                }
            }
            rootNode.Children.Sort();
            return rootNode.Children[rootNode.Children.Count - 1].PrevMove;
        }
    }
}
