using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SnakeLab
{
	public partial class Form1 : Form
	{
		private Point PastLocationHead;
		private int rI, rJ;
		private PictureBox fruit;
		private PictureBox vegetable;
		private PictureBox lemon;
		private PictureBox[] snake = new PictureBox[400];
		private Label labelScore;
		private int dirX, dirY;
		private int _width;
		private int _height;
		private int _sizeofSides = 40;
		private int score;
		Bitmap bitmap = new Bitmap(Image.FromFile(@"C:\Images\Vegetable.jpg"));
		Bitmap bitmap1 = new Bitmap(Image.FromFile(@"C:\Images\lemon.png"));
		private int fruitspawn = 9000;
		public Form1()
		{
			InitializeComponent();
			labelScore = new Label();
			panel2.Controls.Add(labelScore);
			StartGame();
		}
		private SaveRecord record;
		private bool recordIsOpened = SaveRecord.recordIsOpened;
		int rG;
		private void StartGame()
		{
			score = 0;
			_width = panel1.Width;
			_height = panel1.Height;
			SoundPlayer player2 = new SoundPlayer(@"C:\Sounds\RIPears.wav");
			dirX = 1;
			dirY = 0;
			labelScore.Text = "Score: 0";
			labelScore.Location = new Point(10, 10);
			snake[0] = new PictureBox();
			snake[0].Location = new Point(201, 201);
			PastLocationHead = new Point(161, 201);
			snake[0].Size = new Size(_sizeofSides, _sizeofSides);
			snake[0].BackColor = Color.Black;
			panel1.Controls.Add(snake[0]);
			lemon = new PictureBox();
			lemon.Image = bitmap1;
			lemon.Size = new Size(_sizeofSides, _sizeofSides);
			vegetable = new PictureBox();
			vegetable.BackColor = Color.Red;
			vegetable.Size = new Size(_sizeofSides, _sizeofSides);
			fruit = new PictureBox();
			fruit.Image = bitmap;
			fruit.Size = new Size(_sizeofSides, _sizeofSides);
			_generateMap();
			generateFood();
			timer.Tick += new EventHandler(_update);
			timer.Interval = 200;
			timer.Start();
			this.KeyDown += new KeyEventHandler(OKP);
			NewGame.Enabled = false;
			button2.Enabled = false;
			fruitspawn = 9000;
		}
		private void generateFood()
		{
			Random p = new Random();
			rG = p.Next(0, 100);
				if (panel1.Controls.Contains(lemon))
				{
					panel1.Controls.Remove(lemon);
				lemon.Location = new Point(-10, -10);
				}
				if (panel1.Controls.Contains(fruit))
				{
					panel1.Controls.Remove(fruit);
					fruit.Location = new Point(-10, -10);
				}
				if (panel1.Controls.Contains(vegetable))
				{
					panel1.Controls.Remove(vegetable);
					vegetable.Location = new Point(-10, -10);
				}
			if (rG <= 20)
			{
				generateFruit(lemon);
			}
			else if (rG > 20 && rG < 60)
			{
				generateFruit(fruit);
			}
			else if (rG >= 60)
			{
				generateFruit(vegetable);
			}
		}
		
		private void _eatItself()
		{
			for (int i = 1; i < score; i++)
			{
				if (snake[0].Location == snake[i].Location)
				{
					SoundPlayer player1 = new SoundPlayer(@"C:\Sounds\Penetration4.wav");
					player1.Play();
					labelScore.Text = "Score: "+score;
					gameOver();
					score = 0;
				}
			}

		}
		private void _checkBorders()
		{
			SoundPlayer player = new SoundPlayer(@"C:\Sounds\Spank.wav");
			if ((snake[0].Location.X + dirX * _sizeofSides) >= _width - (_width % _sizeofSides))
			{
				player.Play();
				gameOver();
				NewGame.Enabled = true;
			}
			else if ((snake[0].Location.X + dirX * _sizeofSides) < 0)
			{
				player.Play();
				gameOver();
				NewGame.Enabled = true;
			}
			else if ((snake[0].Location.Y + dirY * _sizeofSides) >= _height - (_height) % _sizeofSides)
			{
				player.Play();
				gameOver();
				NewGame.Enabled = true;
			}
			else if ((snake[0].Location.Y + dirY * _sizeofSides) < 0)
			{
				player.Play();
				gameOver();
				NewGame.Enabled = true;
			}
		}
		private void gameOver()
		{
			timer.Enabled = false;
			timer.Tick -= _update;
			dirX = 0;
			dirY = 0;
			NewGame.Enabled = true;
			button2.Enabled = true;
			if (!recordIsOpened)
			{
				record = new SaveRecord(score);
				record.ShowDialog();
			}

		}
		private void generateFruit(PictureBox fruit1)
		{
			fruit1.Location = new Point(-10, -10);
			Random r = new Random();
			rI = r.Next(0, _width - _sizeofSides);
			int tempI = rI % _sizeofSides;
			rI -= tempI;
			rJ = r.Next(0, _height - _sizeofSides);
			int tempJ = rJ % _sizeofSides;
			rJ -= tempJ;
			rI++;
			rJ++;
			Point newLoc = new Point(rI, rJ);
			for (int i = 0; i <= score; i++)
			{
				if (snake[i].Location == newLoc)
				{
					generateFood();
					break;
				}
				else
				{
					fruit1.Location = newLoc;
					panel1.Controls.Add(fruit1);
				}
			}
		}
		public void eatLemon()
		{
			SoundPlayer player2 = new SoundPlayer(@"C:\Sounds\RIPears.wav");
			if (snake[0].Location==lemon.Location)
			{
				player2.Play();
				if (score == 0 && snake[0].Location == lemon.Location)
				{
					gameOver();
				}

				else if (snake[0].Location == lemon.Location)
				{
					player2.Play();
					panel1.Controls.Remove(snake[score]);
					labelScore.Text = "Score: " + --score;
					panel1.Controls.Remove(lemon);
					fruitspawn = 9000;
					lemon.Location = new Point(-10, -10);
					generateFood();
					
				}
			}
		}
		public void eatVegetable()
		{
			if (snake[0].Location==vegetable.Location)
			{
				SoundPlayer player1 = new SoundPlayer(@"C:\Sounds\WOO.wav");
				player1.Play();
				labelScore.Text = "Score: " + ++score;
				snake[score] = new PictureBox();
				snake[score].Location = new Point(snake[score - 1].Location.X - 40 * dirX, snake[score - 1].Location.Y - 40 * dirY);
				snake[score].Size = new Size(_sizeofSides, _sizeofSides);
				snake[score].BackColor = vegetable.BackColor;
				panel1.Controls.Add(snake[score]);
				panel1.Controls.Remove(vegetable);
				fruitspawn = 9000;
				timer.Interval = 80;
				vegetable.Location = new Point(-10, -10);
				generateFood();
			}
		}
		private void _eatfruit()
		{
			if (snake[0].Location==fruit.Location)
			{
				SoundPlayer player1 = new SoundPlayer(@"C:\Sounds\Mmmmh.wav");
				player1.Play();
				labelScore.Text = "Score: " + ++score;
				snake[score] = new PictureBox();
				snake[score].Location = new Point(snake[score - 1].Location.X - 40 * dirX, snake[score - 1].Location.Y - 40 * dirY);
				snake[score].Size = new Size(_sizeofSides,_sizeofSides);
				snake[score].Image = fruit.Image;
				panel1.Controls.Add(snake[score]);
				panel1.Controls.Remove(fruit);
				fruitspawn = 9000;
				timer.Interval = 200;
				fruit.Location = new Point(-10, -10);
				generateFood();
			}
		}
		private void _generateMap()
		{
			_width = panel1.Size.Width;
			_height = panel1.Size.Height;
			for (int i = 0; i <= _height / _sizeofSides; i++)
			{
				PictureBox pic = new PictureBox();
				pic.BackColor = Color.Black;
				pic.Location = new Point(0, _sizeofSides * i);
				pic.Size = new Size(_width - (_width % _sizeofSides), 1);
				panel1.Controls.Add(pic);
			}
			for (int i = 0; i <= _width / _sizeofSides; i++)
			{
				PictureBox pic = new PictureBox();
				pic.BackColor = Color.Black;
				pic.Location = new Point(_sizeofSides * i, 0);
				pic.Size = new Size(1, _height - (_height % _sizeofSides));
				panel1.Controls.Add(pic);
			}
		}
		
		private void _moveSnake()
		{
			_checkBorders();
			if (timer.Enabled)
			{
				for (int i = score; i >= 1; i--)
				{
					snake[i].Location = snake[i - 1].Location;
				}
				PastLocationHead = snake[0].Location;
				snake[0].Location = new Point(snake[0].Location.X + dirX * (_sizeofSides), snake[0].Location.Y + dirY * (_sizeofSides));

				_eatItself();
			}
		}
		private void _update(Object myObject,EventArgs eventsArgs)
		{

			eatLemon();
			eatVegetable();
			_eatfruit();
			_moveSnake();
			fruitspawn -= 100;
			if (fruitspawn==0)
			{
				fruit.Location = new Point(-10, -10);
				vegetable.Location = new Point(-10, -10);
				lemon.Location = new Point(-10, -10);
				generateFood();
				fruitspawn = 9000;
			}
		}

		private void OKP(object sender,KeyEventArgs e)
		{
			switch (e.KeyCode.ToString())
			{
				case "Right":
					if (PastLocationHead.X==snake[0].Location.X)
					{
						dirX = 1;
						dirY = 0;
					}
					break;
				case "Left":
					if (PastLocationHead.X==snake[0].Location.X)
					{
						dirX = -1;
						dirY = 0;
					}
					break;
				case "Up":
					if (PastLocationHead.Y==snake[0].Location.Y)
					{

						dirY = -1;
						dirX = 0;
					}
					break;
				case "Down":
					if (PastLocationHead.Y == snake[0].Location.Y)
					{
						dirY = 1;
						dirX = 0;
					}
					
					break;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form form = new Form();
			ListBox list = new ListBox();
			list.Dock = DockStyle.Fill;
			form.Controls.Add(list);
			for (int i = 0; i < Properties.Settings.Default.Records.Count; i++)
			{
				list.Items.Add(Properties.Settings.Default.Records[i]);
			}
			form.ShowDialog();
		}
		private void restartGame()
		{
			fruit.Location = new Point(-10, -10);
			lemon.Location = new Point(-10, -10);
			vegetable.Location = new Point(-10, -10);
			panel1.Controls.Clear();
			StartGame();
			
		}
			private void button1_Click(object sender, EventArgs e)
		{
			restartGame();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}
	}
}
