using ChessChallenge.API;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

// This is the NegaMax Bot

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        // Piece values: null, pawn, knight, bishop, rook, queen, king
        int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000, -0, -100, -300, -300, -500, -900, -10000 };

        List<string> chessSquares = new List<string>
            {
                "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1",
                "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
                "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
                "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
                "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
                "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
                "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
                "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8"
            };
        int who2move;
        if (board.IsWhiteToMove)
        {
            who2move = 1;
        }
        else
        {
            who2move = -1;
        }

        Move[] moves = board.GetLegalMoves(false);
        int score = 0;

        // DEPTH
        int depth = 4;
        int max = -999999999;
        Move bestMove = moves[moves.Length - 1];

        for (int i = 0; i < moves.Length; i++)
        {
            // Make move, Recursion, Undo move
            board.MakeMove(moves[i]);
            score = -negaMax(depth - 1);
            if (score >= max)
            {
                max = score;
                bestMove = moves[i];
            }
            board.UndoMove(moves[i]);
        }

        return bestMove;

        int negaMax(int depth)
        {
            // end of the tree
            if (depth == 0) return evaluate();

            int max = -999999999;
            Move[] moves = board.GetLegalMoves(false);
            for (int i = 0; i < moves.Length; i++)
            {
                // Make move, Recursion, Undo move
                board.MakeMove(moves[i]);
                score = -negaMax(depth - 1);
                if (score >= max) max = score;
                board.UndoMove(moves[i]);
            }
            return max;
        }

        int alphaBeta(int alpha, int beta, int depthleft)
        {
            if (depthleft == 0) return quiesce(alpha, beta);
            for (all moves)
            {
                score = -alphaBeta(-beta, -alpha, depthleft - 1);
                if (score >= beta)
                    return beta;   //  fail hard beta-cutoff
                if (score > alpha)
                    alpha = score; // alpha acts like max in MiniMax
            }
            return alpha;
        }

        int Quiesce(int alpha, int beta)
        {
            int stand_pat = Evaluate();
            if (stand_pat >= beta)
                return beta;
            if (alpha < stand_pat)
                alpha = stand_pat;

            until(every_capture_has_been_examined)  {
                MakeCapture();
                score = -Quiesce(-beta, -alpha);
                TakeBackMove();

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
            return alpha;
        }

        int evaluate()
        {
            int score = 0;

            // Printing the list of chess squares
            foreach (string square in chessSquares)
            {
                Square realSquare = new Square(square);
                Piece piece = board.GetPiece(realSquare);
                if (piece.IsWhite)
                {
                    score += pieceValues[(int)piece.PieceType] * 1;
                }
                else
                {
                    score += pieceValues[(int)piece.PieceType] * -1;
                }
            }

            return score * who2move;
        }
    }
}