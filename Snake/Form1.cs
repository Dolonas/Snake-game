using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private int rI, rJ;
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[120];
        private GroupBox PanelMng;
        private Label labelScore;
        private MyButton buttonPause;
        private int _width = 990;
        private int _height = 800;
        private int _sizeOfSides = 40;
        private int dirX = 1;
        private int dirY = 0;
        private int score = 0;
        private MelodyPlayer mPlayer = new MelodyPlayer();
        private SoundPlayer eatItselfSound = new SoundPlayer("eat itself.wav");
        private SoundPlayer collideSound = new SoundPlayer("collide.wav");
        private SoundPlayer eatingFruitSound = new SoundPlayer("eating fruit.wav");

        public Form1()
        {
            InitializeComponent();
            this.Width = _width;
            this.Height = _height;
            PanelMng = new GroupBox();
            PanelMng.Text = "Управление";
            PanelMng.ForeColor = Color.White;
            PanelMng.Size = new Size(90, _height-45);
            PanelMng.Location = new Point(_width - 120, 2);
            this.Controls.Add(PanelMng);
            labelScore = new Label();
            labelScore.Font = new Font(labelScore.Font.FontFamily, 12, labelScore.Font.Style);
            labelScore.ForeColor = Color.White;
            labelScore.Text = "Очки: 0";
            labelScore.Size = new Size(80, 40);
            labelScore.Location = new Point(5, 30);
            PanelMng.Controls.Add(labelScore);
            buttonPause = new MyButton();
            buttonPause.Location = new Point(10, 200);
            buttonPause.AutoSize = false;
            buttonPause.Size = new Size(80, 40);
            buttonPause.ForeColor = Color.White;
            buttonPause.Text = "Пауза";
            PanelMng.Controls.Add(buttonPause);
            snake[0] = new PictureBox();
            snake[0].Location = new Point(1, 1);
            snake[0].Size = new Size(_sizeOfSides-1, _sizeOfSides-1);
            snake[0].BackColor = Color.Red;
            this.Controls.Add(snake[0]);
            GenerateMap();
            FruitInit();
            GenerateFruit();
            timer.Tick += new EventHandler(_update);
            timer.Interval = 300;
            timer.Start();
            mPlayer.Play(true);
            this.KeyDown += new KeyEventHandler(OKP);
            this.buttonPause.Click += new EventHandler(MakePause);
        }

        private void GenerateMap()
        {
            for (int i = 0; i< _height/_sizeOfSides-1; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.SlateGray;
                pic.Location = new Point(0, _sizeOfSides * i);
                pic.Size = new Size(_width - 150, 1);
                this.Controls.Add(pic);
            }

            for (int j = 0; j < _width / _sizeOfSides-1; j++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.SlateGray;
                pic.Location = new Point(_sizeOfSides * j, 0);
                pic.Size = new Size(1, _height - 80);
                this.Controls.Add(pic);
            }
        }

        private void _moveSnake()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * _sizeOfSides, snake[0].Location.Y + dirY * _sizeOfSides);
            _eatItself();
        }


        private void _eatFruit()
        {
            if(snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                eatingFruitSound.Play();
                labelScore.Text = "Очки: " + ++score;
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + _sizeOfSides * dirX, snake[score - 1].Location.Y + _sizeOfSides * dirY);
                snake[score].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
                snake[score].BackColor = Color.Red;
                this.Controls.Add(snake[score]);
                GenerateFruit();
            }
        }

        private void FruitInit()
        {
            fruit = new PictureBox();
            fruit.Load("banana.png");
            //fruit.BackColor = Color.Yellow;
            fruit.Size = new Size(_sizeOfSides, _sizeOfSides);
        }

        private void GenerateFruit()
        {
            Random r = new Random();
            rI = r.Next(0, _width - _sizeOfSides * 3);
            int tempI = rI % _sizeOfSides;
            rI -= tempI;
            rJ = r.Next(0, _height - _sizeOfSides * 2);
            int tempJ = rJ % _sizeOfSides;
            rJ -= tempJ;
            rI++;
            rJ++;
            fruit.Location = new Point(rI, rJ);
            this.Controls.Add(fruit);
        }

        private void _update (Object myObject, EventArgs eventArgs)
        {
            checkBorders();
            _eatFruit();
            _moveSnake();
        }

        private void _eatItself()
        {
            
            for(int i = 1; i < score; i++)
            {
                if(snake[0].Location == snake[i].Location)
                {
                    for (int j = i; j <= score; j++)
                        this.Controls.Remove(snake[j]);
                    score = score - (score - i + 1);
                    eatItselfSound.Play();
                }
            }
        }

        private void checkBorders()
        {
            if (snake[0].Location.X < 0)
            {
                collideSound.Play();
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Очки: " + score;
                dirX = 1;
                dirY = 0;
            }

            if (snake[0].Location.X > _width - _sizeOfSides * 3)
            {
                collideSound.Play();
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Очки: " + score;
                dirX = -1;
                dirY = 0;
            }

            if (snake[0].Location.Y < 0)
            {
                collideSound.Play();
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Очки: " + score;
                dirY = 1;
                dirX = 0;
            }

            if (snake[0].Location.Y > _height - _sizeOfSides * 2.5)
            {
                collideSound.Play();
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Очки: " + score;
                dirY = -1;
                dirX = 0;
            }

        }

        private void  OKP (object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    dirX = 0;
                    dirY = -1;
                    break;
                case "Down":
                    dirX = 0;
                    dirY = 1;
                    break;
                case "P":
                    MakePause(sender, e);
                    break;
            }
        }

        private void MakePause(Object myObject, EventArgs eventArgs)
        {
            if (timer.Enabled == true)
                timer.Stop();
            else
                timer.Start();
        }

    }

    

}
