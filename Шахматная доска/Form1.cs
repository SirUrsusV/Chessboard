using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Шахматная_доска.Form1;

namespace Шахматная_доска
{
	public partial class Form1 : Form
	{
		public GameMod GmMod;
		public const short CellSize = 80;
		public const short BoardSize = 8; //Размер указан в клетках
		public List<Cell> Board { get; set; }
		public List<ChessPiece> Chesses { get; set; }
		public List<Cell> AvibalMove { get; set; }

		private ChessPiece selectedChessPiece;

		private bool isStartd;
		private bool moved;
		private bool oppenCtMenu;

		private const string HeadText = "Шахматная доска";
		private const string HelpHeadText = "Для отмены нажмите клавишу Esc";

		public static Brush BlackTeamBrush = Brushes.Black;
		public static Brush WhiteTeamBrush = Brushes.White;

		public Form1()
		{
			GmMod = GameMod.Classic;
			isStartd = true;

			InitializeComponent();
			InitializeContextMenu();
			selectedChessPiece = null;

			Chesses = new List<ChessPiece>();
			Board = new List<Cell>();
			this.Size = new Size(BoardSize * CellSize + 15, BoardSize * CellSize + 39);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.DoubleBuffered = true;

			this.MouseDown += Form1_MouseDown;

		}
		private void InitializeChessPiece(Graphics graphics)
		{
			ChessPiece whhiteQueen = new Queen();
			whhiteQueen.PieceColor = Color.White;
			whhiteQueen.X = 0;
			whhiteQueen.Y = 0;
			DrawChessPieceInCell(whhiteQueen, graphics);
			Chesses.Add(whhiteQueen);

			ChessPiece BlackQueen1 = new Queen();
			BlackQueen1.PieceColor = Color.Black;
			BlackQueen1.X = 1;
			BlackQueen1.Y = 1;
			DrawChessPieceInCell(BlackQueen1, graphics);
			Chesses.Add(BlackQueen1);

			ChessPiece BlackQueen2 = new Queen();
			BlackQueen2.PieceColor = Color.Black;
			BlackQueen2.X = 2;
			BlackQueen2.Y = 2;
			DrawChessPieceInCell(BlackQueen2, graphics);
			Chesses.Add(BlackQueen2);

			ChessPiece BlackKnight1 = new Knight();
			BlackKnight1.PieceColor = Color.Black;
			BlackKnight1.X = 5;
			BlackKnight1.Y = 5;
			DrawChessPieceInCell(BlackKnight1, graphics);
			Chesses.Add(BlackKnight1);

			ChessPiece BlackKnight2 = new Knight();
			BlackKnight2.PieceColor = Color.White;
			BlackKnight2.X = 4;
			BlackKnight2.Y = 5;
			DrawChessPieceInCell(BlackKnight2, graphics);
			Chesses.Add(BlackKnight2);

			isStartd = false;
		}
		private void InitializeContextMenu()
		{
			ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Opened += ContextMenu_Opened;

			ToolStripMenuItem moveItem = new ToolStripMenuItem("Двигаться");

			moveItem.Click += MoveItem_Click;
			

			moveItem.Enabled = true;

			contextMenu.Items.Add(moveItem);

			if(GmMod == GameMod.Testing)
			{
				ToolStripMenuItem deleteItem = new ToolStripMenuItem("Удалить");
				ToolStripMenuItem createItem = new ToolStripMenuItem("Создать");

				deleteItem.Click += DeleteItem_Click;
				createItem.Click += CreateItem_Click;

				deleteItem.Enabled = false;
				createItem.Enabled = false;

				contextMenu.Items.Add(new ToolStripSeparator());
				contextMenu.Items.Add(deleteItem);
				contextMenu.Items.Add(new ToolStripSeparator());
				contextMenu.Items.Add(createItem);
			}
			this.ContextMenuStrip = contextMenu;
		}
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                moved = false;
                AvibalMove.Clear();
                this.Text = HeadText;
                Invalidate();
            }
            base.OnKeyUp(e);
        }

        private void ContextMenu_Opened(object sender, EventArgs e)
        {
            Point point = this.PointToClient(Cursor.Position);
			ChessPiece temp = Chesses.Find(cll => cll.X == point.X / CellSize && cll.Y == point.Y / CellSize);
			if(temp == null)
				ContextMenuStrip.Items[0].Enabled = false;
			else
                ContextMenuStrip.Items[0].Enabled = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			int x = e.X / CellSize;
			int y = e.Y / CellSize;
            if (moved && e.Button == MouseButtons.Left && AvibalMove.Contains(new Cell { X = x, Y = y }))
            {
                ChessPiece chess = Chesses.Find(c => c.X == x && c.Y == y);
                if (chess != null)
                    Chesses.Remove(chess);
                AvibalMove.Clear();
                selectedChessPiece.Move(x, y, this);
                moved = false;
                this.Text = HeadText;
            }
        }
		private void MoveItem_Click(object sender, EventArgs e)
		{
			Point point = this.PointToClient(Cursor.Position);
            ChessPiece temp = Chesses.Find(cll => cll.X == point.X / CellSize && cll.Y == point.Y / CellSize);
			selectedChessPiece = temp;
            moved = true;
			AvibalMove = selectedChessPiece.AvibalMoves(this);
			this.Text = HelpHeadText;

			Invalidate();
		}

		private void DeleteItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Удалить выбран");
		}

		private void CreateItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Создать выбран");
		}
		private void Form1_Paint(object sender, PaintEventArgs e)
		{
            this.SuspendLayout();
            DrawChessBoard(e.Graphics);
			if (moved)
				DrawAvibalMoves(e.Graphics);
			if (isStartd)
				InitializeChessPiece(e.Graphics);
			else
				foreach (ChessPiece chessPiece in Chesses)
					chessPiece.DrawPiece(e.Graphics);
			ResumeLayout(true);

        }
		private void DrawAvibalMoves(Graphics graphics)
		{
			List<Point> points = new List<Point>();
			foreach (var piece in Chesses)
				points.Add(new Point(piece.X, piece.Y));


			foreach (Cell cell in AvibalMove)
			{
				if(points.Contains(new Point(cell.X, cell.Y)))
				{
					if (Chesses.Find(chss => cell.X == chss.X && cell.Y == chss.Y).PieceColor != selectedChessPiece.PieceColor)
						graphics.FillRectangle((cell.X + cell.Y) % 2 != 0 ? Brushes.Red : Brushes.DarkRed, cell.CellRectangle);
				}
					
				else
					graphics.FillRectangle((cell.X + cell.Y) % 2 != 0 ? Brushes.DarkGreen : Brushes.Green, cell.CellRectangle);
			}
		}
		private void DrawChessBoard(Graphics graphics)
		{
			Board.Clear();
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					Rectangle rectangle = new Rectangle(i * CellSize, j * CellSize, CellSize, CellSize);
					graphics.FillRectangle((i + j) % 2 != 0 ? Brushes.DarkGray : Brushes.Gray, rectangle);
					Cell cell = new Cell();
					cell.CellRectangle = rectangle;
					cell.X = i;
					cell.Y = j;
					Board.Add(cell);
				}
			}
		}
		private void DrawChessPieceInCell(ChessPiece chessPiece, Graphics graphics)
		{
			chessPiece.DrawPiece(graphics);
		}
		//private void DrawPossibleMoves(Graphics graphics)
		//{
		//	for (int i = 1; i < BoardSize+1; i++)
		//	{
		//		for (int j = 1; j < BoardSize+1; j++)
		//		{
		//			int x = 0 + (-1 * 5);
		//			int y = -4;

		//			//(i + x) == j

		//			if (i == BoardSize - (j + y) || (i + x) == j)
		//			{
		//				Rectangle rectangle = new Rectangle((i-1) * CellSize, (j-1) * CellSize, CellSize, CellSize);
		//				graphics.FillRectangle(Brushes.DarkGreen, rectangle);
		//				Cell cell = new Cell();
		//				cell.CellRectangle = rectangle;
		//				cell.X = i-1;
		//				cell.Y = j-1;
		//				cell.Piece = null;
		//				Board.Add(cell);
		//			}
		//		}
		//	}
		//}
		public class Cell
		{
			public Rectangle CellRectangle { get; set; }
			public int X;
			public int Y;
			public Cell(int x, int y, Color color)
			{
				CellRectangle = new Rectangle(x*CellSize, y*CellSize, CellSize, CellSize);
				X = x;
				Y = y;
			}
			public Cell()
			{}
			public override bool Equals(object obj)
			{
				if(obj is Cell cell)
				{
					return X == cell.X && Y == cell.Y;
				}
				return false;
			}
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}
	}
	public enum GameMod
	{
		Testing,
		Classic
	}
	abstract public class ChessPiece
	{
		public Color PieceColor { get; set; }
		public int X;
		public int Y;
		public abstract void DrawPiece(Graphics graphics);
		public abstract void Move(int x, int y, Form1 form1);
		public abstract List<Cell> AvibalMoves(Form1 form);
	}
	public class Queen : ChessPiece
	{
		public const string icon = "♛";
		public override List<Cell> AvibalMoves(Form1 form)
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
						if(form.Chesses.Find(chss => chss.X == x && chss.Y == y).PieceColor != this.PieceColor)
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
		public override void Move(int x, int y, Form1 form1)
		{
			X = x;
			Y = y;
			form1.Invalidate();
		}
	}
	public class Knight : ChessPiece
	{
		public const string icon = "♞";
		public Knight() { }
		public override List<Cell> AvibalMoves(Form1 form)
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

				if (X >= 0 && X < BoardSize && Y >= 0 && Y < BoardSize)
				{
					if (points.Contains(new Point(newX, newY)))
					{
						if(form.Chesses.Find(css => css.X == newX && css.Y == newY).PieceColor != this.PieceColor)
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
		public override void Move(int x, int y, Form1 form1)
		{
			X = x;
			Y = y;
			form1.Invalidate();
		}
	}
}
