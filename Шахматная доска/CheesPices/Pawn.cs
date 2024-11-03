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
    public class Pawn: ChessPiece
	{
		public const string icon = "♟";
        public override List<Cell> AvibalMoves(MainForm form)
        {
            List<Point> points = new List<Point>();
            List<Cell> avibalCells = new List<Cell>();


            foreach (ChessPiece piece in form.Chesses)
                points.Add(new Point(piece.X, piece.Y));

            if (this.PieceColor == Color.White)
            {
                if (Y < BoardSize-1 && form.Chesses.Find(chss => chss.X == this.X && chss.Y == (this.Y - 1)) == null)
                    avibalCells.Add(new Cell(X, Y - 1, Color.Green));
                if(Y == 6)
                    avibalCells.Add(new Cell(X, Y - 2, Color.Green));

                if (form.Chesses.Find(chss => chss.PieceColor == Color.Black && chss.X == this.X - 1 && chss.Y == (this.Y - 1)) != null)
                    avibalCells.Add(new Cell(X - 1, Y - 1, Color.Green));
                if (form.Chesses.Find(chss => chss.PieceColor == Color.Black && chss.X == this.X + 1 && chss.Y == (this.Y - 1)) != null)
                    avibalCells.Add(new Cell(X + 1, Y - 1, Color.Green));
            }
            if (this.PieceColor == Color.Black)
            {
                if (Y > 0 && form.Chesses.Find(chss => chss.X == this.X && chss.Y == (this.Y + 1)) == null)
                    avibalCells.Add(new Cell(X, Y + 1, Color.Green));
                if(Y == 1)
                    avibalCells.Add(new Cell(X, Y + 2, Color.Green));

                if (form.Chesses.Find(chss => chss.PieceColor == Color.White && chss.X == this.X - 1 && chss.Y == (this.Y + 1)) != null)
                    avibalCells.Add(new Cell(X - 1, Y + 1, Color.Green));
                if (form.Chesses.Find(chss => chss.PieceColor == Color.White && chss.X == this.X + 1 && chss.Y == (this.Y + 1)) != null)
                    avibalCells.Add(new Cell(X + 1, Y + 1, Color.Green));
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
