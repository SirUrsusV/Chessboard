using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Шахматная_доска.MainForm;

namespace Шахматная_доска
{
    public class ChessPiece
    {
        public Color PieceColor { get; set; }
        public int X;
        public int Y;
        public void Move(int x, int y, MainForm form1)
        {
            X = x;
            Y = y;
            form1.Invalidate();
        }
        public virtual void DrawPiece(Graphics graphics)
        { }
        public virtual List<Cell> AvibalMoves(MainForm form)
        { return null; }
    }
}
