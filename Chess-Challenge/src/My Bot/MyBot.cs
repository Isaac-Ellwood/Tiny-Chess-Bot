using ChessChallenge.API;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        Random rng = new();

        int moveToPlay = -1;
        int highMoveValue = 0;
        // Piece values: null, pawn, knight, bishop, rook, queen, king
        int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };

        Move[] moves = board.GetLegalMoves(false);

        for (int i = 0; i < moves.Length; i++)
        {
            int moveValue = 0;

            // Captures
            if (moves[i].IsCapture)
            {
                Piece capturedPiece = board.GetPiece(moves[i].TargetSquare);
                int capturedPieceValue = pieceValues[(int)capturedPiece.PieceType];

                moveValue += capturedPieceValue;
            }

            // Checks and Checkmates
            board.MakeMove(moves[i]);

            if (board.IsInCheckmate())
            {
                moveValue += 100000000;
                board.UndoMove(moves[i]);
            }
            else if (board.IsInCheck())
            {
                moveValue += 100;
                board.UndoMove(moves[i]);
            }
            else
            {
                board.UndoMove(moves[i]);
            }

            // Takesies Backsies
            board.MakeMove(moves[i]);
            Move[] enemyTaksiesMoves = board.GetLegalMoves(true);

            int highEnemiesTaksies = 0;

            for (int enemyIndex = 0; enemyIndex < enemyTaksiesMoves.Length; enemyIndex++)
            {
                Piece capturedPiece = board.GetPiece(enemyTaksiesMoves[enemyIndex].TargetSquare);
                int currentEmemiesTaksies = pieceValues[(int)capturedPiece.PieceType] * 3/2;
                if (currentEmemiesTaksies > highEnemiesTaksies)
                {
                    highEnemiesTaksies = currentEmemiesTaksies;
                }
            }
            board.UndoMove(moves[i]);
            moveValue -= highEnemiesTaksies;

            if (moveValue > highMoveValue)
            {
                moveToPlay = i;
                highMoveValue = moveValue;
            }
        }
        if (moveToPlay == -1)
        {
            Console.WriteLine("Random");
            moveToPlay = rng.Next(moves.Length);
        }
        else
        {
            Console.WriteLine($"{highMoveValue}");
        }
        return moves[moveToPlay];
    }
}