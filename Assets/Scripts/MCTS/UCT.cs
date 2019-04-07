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
                Board board = new Board(rootBoard);

                while (node.UntriedMoves.Count == 0 & node.Children.Count != 0)
                {
                    node = node.UCTSelectChild();
                    board.PlayMove(node.PrevMove);
                }

                if (node.UntriedMoves.Count != 0)
                {
                    var max = 0;
                    Move bestMove = new Move();
                    foreach (var move in node.UntriedMoves.Values)
                    {
                        var diff = move.Value(board);
                        if (diff < max) continue;
                        if (Random.value > .9f) continue;
                        max = diff;
                        bestMove = move;
                    }
                    board.PlayMove(bestMove);
                    node = node.AddChild(bestMove, board);
                }

                while (board.GetAllValidMoves().Count != 0)
                    board.PlayMove(board.GetValidMove());

                while (node != null)
                {
                    node.Update(board.GetWinner(node.IsLeftPlayerMove));
                    node = node.Parent;
                }
            }
            rootNode.Children.Sort();
            return rootNode.Children[rootNode.Children.Count - 1].PrevMove;
        }
    }
}
