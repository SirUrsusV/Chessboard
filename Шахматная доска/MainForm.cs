using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Шахматная_доска.CheesPices;
using static Шахматная_доска.MainForm;

namespace Шахматная_доска
{
	public partial class MainForm : Form
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

		private HelloForm HelloForm;

		private Color ColorMove;

		public MainForm(GameMod gameMod, HelloForm helloForm)
		{
			HelloForm = helloForm;
			GmMod = gameMod;
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
			this.Disposed += MainForm_Disposed;

			if(GmMod == GameMod.Classic)
			{
                Size = new Size(BoardSize * CellSize + 350, BoardSize * CellSize + 39);
				ColorMove = Color.White;
                this.Text = HeadText + $": ход " + ((ColorMove == Color.White) ? "белых" : "чёрных");
            }

		}
		private void MainForm_Disposed(object sender, EventArgs e)
		{
			HelloForm.Dispose();
		}
		private void InitializeChessPiece(Graphics graphics)
		{
			if(GmMod == GameMod.Testing)
			{
				ChessPiece whhitePawn = new Pawn();
				whhitePawn.PieceColor = Color.White;
				whhitePawn.X = 7;
				whhitePawn.Y = 6;
				DrawChessPieceInCell(whhitePawn, graphics);
				Chesses.Add(whhitePawn);

				ChessPiece blackPawn = new Pawn();
				blackPawn.PieceColor = Color.Black;
				blackPawn.X = 3;
				blackPawn.Y = 1;
				DrawChessPieceInCell(blackPawn, graphics);
				Chesses.Add(blackPawn);

				ChessPiece whhiteBishop = new Bishop();
				whhiteBishop.PieceColor = Color.Black;
				whhiteBishop.X = 6;
				whhiteBishop.Y = 1;
				DrawChessPieceInCell(whhiteBishop, graphics);
				Chesses.Add(whhiteBishop);

				ChessPiece whhiteRook = new Rook();
				whhiteRook.PieceColor = Color.Black;
				whhiteRook.X = 6;
				whhiteRook.Y = 0;
				DrawChessPieceInCell(whhiteRook, graphics);
				Chesses.Add(whhiteRook);

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
				BlackKnight2.PieceColor = Color.Black;
				BlackKnight2.X = 4;
				BlackKnight2.Y = 5;
				DrawChessPieceInCell(BlackKnight2, graphics);
				Chesses.Add(BlackKnight2);

				ChessPiece WhiteKing = new King();
				WhiteKing.PieceColor = Color.White;
				WhiteKing.X = 5;
				WhiteKing.Y = 6;
				DrawChessPieceInCell(WhiteKing, graphics);
				Chesses.Add(WhiteKing);
			}
			if(GmMod == GameMod.Classic)
			{
				List<Color> colors = new List<Color>
				{
					Color.Black,
					Color.White,
				};
				foreach (var color in colors)
				{
					int y = (color == Color.Black) ? 0 : 7;
					for (int i = 0; i < 8; i++)
					{

						Pawn pawn = new Pawn();
						pawn.PieceColor = color;
						pawn.X = i;
						pawn.Y = (color == Color.Black) ? 1 : 6;
						DrawChessPieceInCell(pawn, graphics);
						Chesses.Add(pawn);
					}

					Rook leftRook = new Rook();
					leftRook.PieceColor = color;
					leftRook.X = 0;
					leftRook.Y = y;
					DrawChessPieceInCell(leftRook, graphics);
					Chesses.Add(leftRook);

					Bishop leftBishop = new Bishop();
					leftBishop.PieceColor = color;
					leftBishop.X = 2;
					leftBishop.Y = y;
					DrawChessPieceInCell(leftBishop, graphics);
					Chesses.Add(leftBishop);

					Knight leftKnight = new Knight();
					leftKnight.PieceColor = color;
					leftKnight.X = 1;
					leftKnight.Y = y;
					DrawChessPieceInCell(leftKnight, graphics);
					Chesses.Add(leftKnight);

					Rook rightRook = new Rook();
					rightRook.PieceColor = color;
					rightRook.X = 7;
					rightRook.Y = y;
					DrawChessPieceInCell(rightRook, graphics);
					Chesses.Add(rightRook);

					Knight rightKnight = new Knight();
					rightKnight.PieceColor = color;
					rightKnight.X = 6;
					rightKnight.Y = y;
					DrawChessPieceInCell(rightKnight, graphics);
					Chesses.Add(rightKnight);

					Bishop rightBishop = new Bishop();
					rightBishop.PieceColor = color;
					rightBishop.X = 5;
					rightBishop.Y = y;
					DrawChessPieceInCell(rightBishop, graphics);
					Chesses.Add(rightBishop);

					Queen queen = new Queen();
					queen.PieceColor = color;
					//if (color == Color.Black)
					//{
						queen.X = 4;
						queen.Y = y;
						DrawChessPieceInCell(queen, graphics);
						Chesses.Add(queen);

						King king = new King();
						king.PieceColor = color;
						king.X = 3;
						king.Y = y;
						DrawChessPieceInCell(king, graphics);
						Chesses.Add(king);
					//}
					//if(color == Color.White)
					//{
					//	queen.X = 3;
					//	queen.Y = y;
					//	DrawChessPieceInCell(queen, graphics);
					//	Chesses.Add(queen);

					//	King king = new King();
					//	king.PieceColor = color;
					//	king.X = 4;
					//	king.Y = y;
					//	DrawChessPieceInCell(king, graphics);
					//	Chesses.Add(king);
					//}

				}

			}

			isStartd = false;
		}
		private void InitializeContextMenu()
		{
			ContextMenuStrip contextMenu = new ContextMenuStrip();
			contextMenu.Opened += ContextMenu_Opened;

			ToolStripMenuItem moveItem = new ToolStripMenuItem("Двигаться");
			moveItem.Click += MoveItem_Click;
			contextMenu.Items.Add(moveItem);

			ToolStripMenuItem transformPawn = new ToolStripMenuItem();
			transformPawn.Text = "Переделать в";

			ToolStripMenuItem transformToQueen = new ToolStripMenuItem("Ферзь");
			transformToQueen.Click += TransformToQueen_Click;
			ToolStripMenuItem transformToBishop = new ToolStripMenuItem("Слон");
			transformToBishop.Click += TransformToBishop_Click;
			ToolStripMenuItem transformToRook = new ToolStripMenuItem("Ладья");
			transformToRook.Click += TransformToRook_Click;
			ToolStripMenuItem transformToKnight = new ToolStripMenuItem("Конь");
			transformToKnight.Click += TransformToKnight_Click;

			transformPawn.DropDownItems.Add(transformToQueen);
			transformPawn.DropDownItems.Add(transformToBishop);
			transformPawn.DropDownItems.Add(transformToRook);
			transformPawn.DropDownItems.Add(transformToKnight);

			transformPawn.Enabled = false;
			contextMenu.Items.Add(transformPawn);

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
		private void TransformToKnight_Click(object sender, EventArgs e)
		{
			if (selectedChessPiece is Pawn pawn)
			{
				int index = Chesses.FindIndex(chss => selectedChessPiece.X == chss.X && selectedChessPiece.Y == chss.Y);
				int x = Chesses[index].X;
				int y = Chesses[index].Y;
				Color color = Chesses[index].PieceColor;
				Chesses[index] = null;
				Chesses[index] = new Knight { X = x, Y = y, PieceColor = color };
				Invalidate();
			}
		}
		private void TransformToRook_Click(object sender, EventArgs e)
		{
			if (selectedChessPiece is Pawn pawn)
			{
				int index = Chesses.FindIndex(chss => selectedChessPiece.X == chss.X && selectedChessPiece.Y == chss.Y);
				int x = Chesses[index].X;
				int y = Chesses[index].Y;
				Color color = Chesses[index].PieceColor;
				Chesses[index] = null;
				Chesses[index] = new Rook { X = x, Y = y, PieceColor = color };
				Invalidate();
			}
		}
		private void TransformToBishop_Click(object sender, EventArgs e)
		{
			if (selectedChessPiece is Pawn pawn)
			{
				int index = Chesses.FindIndex(chss => selectedChessPiece.X == chss.X && selectedChessPiece.Y == chss.Y);
				int x = Chesses[index].X;
				int y = Chesses[index].Y;
				Color color = Chesses[index].PieceColor;
				Chesses[index] = null;
				Chesses[index] = new Bishop { X = x, Y = y, PieceColor = color };
				Invalidate();
			}
		}
		private void TransformToQueen_Click(object sender, EventArgs e)
		{
			if(selectedChessPiece is Pawn pawn)
			{
				int index = Chesses.FindIndex(chss => selectedChessPiece.X == chss.X && selectedChessPiece.Y == chss.Y);
				int x = Chesses[index].X;
				int y = Chesses[index].Y;
				Color color = Chesses[index].PieceColor;
				Chesses[index] = null;
				Chesses[index] = new Queen { X = x, Y = y, PieceColor = color };
				Invalidate();
			}
		}
		private void ContextMenu_Opened(object sender, EventArgs e)
		{
			Point point = this.PointToClient(Cursor.Position);
			ChessPiece temp = Chesses.Find(cll => cll.X == point.X / CellSize && cll.Y == point.Y / CellSize);
			if(temp == null || ColorMove != temp.PieceColor)
                ContextMenuStrip.Items[0].Enabled = false;
			else 
			{
				ContextMenuStrip.Items[0].Enabled = true;
				selectedChessPiece = temp;
				if(selectedChessPiece is Pawn && (selectedChessPiece.Y == 7 || selectedChessPiece.Y == 0))
					this.ContextMenuStrip.Items[1].Enabled = true;
				else
					this.ContextMenuStrip.Items[1].Enabled = false;
			}
		}
		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{

			int x = e.X / CellSize;
			int y = e.Y / CellSize;
			if (selectedChessPiece != null && moved && e.Button == MouseButtons.Left && AvibalMove.Contains(new Cell { X = x, Y = y }))
			{
				ChessPiece chess = Chesses.Find(c => c.X == x && c.Y == y);
				if (chess != null)
					Chesses.Remove(chess);
				AvibalMove.Clear();
				selectedChessPiece.Move(x, y, this);
				moved = false;

				if(ColorMove == Color.White)
					ColorMove = Color.Black;
                else if (ColorMove == Color.Black)
                    ColorMove = Color.White;

                this.Text = HeadText + $": ход " + ((ColorMove == Color.White) ? "белых": "чёрных");

				if (Chesses.Find(chss => chss is King && chss.AvibalMoves(this).Count == 0) != null)
					Win((ColorMove == Color.White) ? Color.Black : Color.White);
            }

		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if(e.KeyData == Keys.Escape)
			{
				moved = false;
				AvibalMove.Clear();
				this.Text = HeadText;
				Invalidate();
			}
			base.OnKeyUp(e);
		}
		private void MoveItem_Click(object sender, EventArgs e)
		{
			Point point = this.PointToClient(Cursor.Position);
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


		private void Win(Color wining)
		{
			MessageBox.Show("Победил игрок за " + ((ColorMove == Color.Black) ? "белых" : "чёрных"));
		}
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
}
