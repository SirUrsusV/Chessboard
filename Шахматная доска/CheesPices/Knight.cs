using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Шахматная_доска.MainForm;

namespace Шахматная_доска
{
    public class Knight : ChessPiece
    {
        public const string icon = "♞";
        public Knight() { }
        public override List<Cell> AvibalMoves(MainForm form)
        {
            List<Point> points = new List<Point>();
            List<Cell> avibalCells = new List<Cell>();
            int[,] Moves = new int[,]
            {
                { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
            };

            foreach (ChessPiece piece in form.Chesses)
                points.Add(new Point(piece.X, piece.Y));

            for (int i = 0; i < Moves.GetLength(0); i++)
            {
                int newX = X + Moves[i, 0];
                int newY = Y + Moves[i, 1];

                if (newX >= 0 && newX < BoardSize && newY >= 0 && newY < BoardSize)
                {
                    if (points.Contains(new Point(newX, newY)))
                    {
                        if (form.Chesses.Find(css => css.X == newX && css.Y == newY).PieceColor != this.PieceColor)
                            avibalCells.Add(new Cell(newX, newY, Color.Green));
                    }
                    else
                        avibalCells.Add(new Cell(newX, newY, Color.Green));
                }
            }
            return avibalCells;
        }
        public override void DrawPiece(Graphics graphics)
        {
            graphics.DrawString(icon, new Font("Arial Black", CellSize / 2),
                PieceColor == Color.Black ? BlackTeamBrush : WhiteTeamBrush, X * CellSize, Y * CellSize);
        }
    }
}
