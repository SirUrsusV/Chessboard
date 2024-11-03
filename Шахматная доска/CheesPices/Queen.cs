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
    public class Queen : ChessPiece
    {
        public const string icon = "♛";
        public override List<Cell> AvibalMoves(MainForm form)
        {
            List<Point> points = new List<Point>();
            List<Cell> avibalCells = new List<Cell>();
            int[,] directions = new int[,]
            {
                { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, // Вертикальные и горизонтальные направления
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } // Диагональные направления
			};

            foreach (ChessPiece piece in form.Chesses)
                points.Add(new Point(piece.X, piece.Y));

            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int dx = directions[d, 0];
                int dy = directions[d, 1];
                int x = X + dx;
                int y = Y + dy;

                while (x >= 0 && x < BoardSize && y >= 0 && y < BoardSize)
                {
                    if (points.Contains(new Point(x, y)))
                    {
                        if (form.Chesses.Find(chss => chss.X == x && chss.Y == y).PieceColor != this.PieceColor)
                            avibalCells.Add(new Cell(x, y, Color.Green));
                        break;
                    }
                    else
                        avibalCells.Add(new Cell(x, y, Color.Green));
                    x += dx;
                    y += dy;
                }
            }
            /*
			 for (int i = 0; i < BoardSize; i++)
			{
				if (i != X)
					avibalCells.Add(new Cell(i, Y, Color.Green));
				if (i != Y)
					avibalCells.Add(new Cell(X, i, Color.Green));
			
			}
			for (int i = 0; i < BoardSize; i++)
			{
				if (X + i <= 7 && Y + i <= 7)
					avibalCells.Add(new Cell(X + i, Y + i, Color.Green));
				if (X - i >= 0 && Y - i >= 0)
					avibalCells.Add(new Cell(X - i, Y - i, Color.Green));
				if (X + i <= 7 && Y - i >= 0)
					avibalCells.Add(new Cell(X + i, Y - i, Color.Green));
				if (X - i >= 0 && Y + i <= 7)
					avibalCells.Add(new Cell(X - i, Y + i, Color.Green));
			}
			*/


            return avibalCells;
        }
        public override void DrawPiece(Graphics graphics)
        {
            graphics.DrawString(icon, new Font("Arial Black", CellSize / 2),
                PieceColor == Color.Black ? BlackTeamBrush : WhiteTeamBrush, X * CellSize, Y * CellSize);
        }
    }
}
