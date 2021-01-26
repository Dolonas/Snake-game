using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Snake
{
    public partial class Form1 : Form
    {
        private int xAxisOfFruit, yAxisOfFruit;
        private GameItem fruit;  // заготовка фрукта, который будет поедать Змей
        private GameItem[] snake = new GameItem[120];    //массив элементов Змейки
        private Panel PanelGameField;  // элемент в которое будет помещено игровое поле
        private Panel PanelMng;  // элемент формы, на котором будут располагаться органы управления
        private Label labelScore;   //  текстовый элемент с подсчётом очков
        private MyButton buttonPause;
        private MyButton buttonExit;
        private int formWidth = 990;
        private int formHeight = 800;
        private int sizeOfSquare = 40;
        private int xSizeOfField;
        private int ySizeOfField;
        private int xAmount = 20;
        private int yAmount = 20;
        private string workDirectory;

        private int dirX = 1;
        private int dirY = 0;
        private int score = 0;
        private WindowsMediaPlayer WMP;
        private string melodyFile;
        private SoundPlayer eatItselfSound;
        private SoundPlayer collideSound;
        private SoundPlayer eatingFruitSound;

        public Form1()
        {
            InitializeComponent();
            this.Width = formWidth;
            this.Height = formHeight;
            xSizeOfField = formWidth - 160;
            ySizeOfField = formHeight - 45;
            string currDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = currDirectory + @"\..\..\Resources\";
            workDirectory = Path.GetFullPath(tempDirectory);
            melodyFile = workDirectory + "Ocean_Man.wav";
            eatItselfSound = new SoundPlayer(workDirectory + "eat itself.wav");
            collideSound = new SoundPlayer(workDirectory + "collide.wav");
            eatingFruitSound = new SoundPlayer(workDirectory + "eating fruit.wav");

            FormBuilding();
            SnakeAdd();
            GenerateMap();
            FruitInit();
            GenerateFruit();

            //mPlayer.Play(true);
            MelodyStart();

            
            this.KeyDown += new KeyEventHandler(OKP);
            this.buttonPause.Click += new EventHandler(MakePause);
            this.buttonExit.Click += new EventHandler(MakeExit);
        }

        private void FormBuilding()
        {
            PanelGameField = new Panel();
            PanelGameField.Text = "Игра";
            PanelGameField.ForeColor = Color.White;
            PanelGameField.BackColor = Color.FromArgb(30, 40, 40);
            PanelGameField.Size = new Size(xSizeOfField, ySizeOfField);
            PanelGameField.Location = new Point(2, 2);
            this.Controls.Add(PanelGameField);

            PanelMng = new Panel();
            PanelMng.Text = "Управление";
            PanelMng.ForeColor = Color.White;
            PanelMng.BackColor = Color.FromArgb(40,40,40);
            PanelMng.Size = new Size(90, formHeight - 45);
            PanelMng.Location = new Point(formWidth - 120, 2);
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

            buttonExit = new MyButton();
            buttonExit.Location = new Point(10, 250);
            buttonExit.AutoSize = false;
            buttonExit.Size = new Size(80, 40);
            buttonExit.ForeColor = Color.White;
            buttonExit.Text = "Выход";
            PanelMng.Controls.Add(buttonExit);

            timer.Tick += new EventHandler(Update);
            timer.Interval = 300;
            timer.Start();
        }

        private void SnakeAdd()
        {
            snake[0] = new GameItem();
            snake[0].XCoor = 0;
            snake[0].YCoor = 0;
            snake[0].Size = new Size(sizeOfSquare - 1, sizeOfSquare - 1);
            snake[0].BackColor = Color.Red;
            PanelGameField.Controls.Add(snake[0]);
        }

        private void GenerateMap()
        {
            for (int x = 0; x < xAmount; x++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.SlateGray;
                pic.Location = new Point(x * sizeOfSquare + GameItem.XPanelShift + GameItem.XFieldShift, GameItem.YPanelShift + GameItem.YFieldShift);
                pic.Size = new Size(1, formHeight - 80);
                PanelGameField.Controls.Add(pic);
            }

            for (int y = 0; y< yAmount; y++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.SlateGray;
                pic.Location = new Point(GameItem.XPanelShift + GameItem.XFieldShift, y * sizeOfSquare + GameItem.YPanelShift + GameItem.YFieldShift);
                pic.Size = new Size(formWidth - 150, 1);
                PanelGameField.Controls.Add(pic);
            }

            
        }

        private void MoveSnake()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].LocationField = snake[i - 1].LocationField;
            }
            snake[0].LocationField = new Point(snake[0].LocationField.X + dirX, snake[0].LocationField.Y + dirY);
            EatItself();
        }


        private void EatFruit()
        {
            if(snake[0].LocationField == fruit.LocationField)
            {
                eatingFruitSound.Play();
                labelScore.Text = "Очки: " + ++score;
                snake[score] = new GameItem();
                snake[score].LocationField = new Point(snake[score - 1].LocationField.X + dirX, snake[score - 1].LocationField.Y + dirY);
                snake[score].BackColor = Color.Red;
                this.Controls.Add(snake[score]);
                GenerateFruit();
            }
        }

        private void FruitInit()
        {
            fruit = new GameItem();
            fruit.Load(workDirectory + "banana.png");
            //fruit.BackColor = Color.Yellow;
            //fruit.Size = new Size(sizeOfSquare, sizeOfSquare);
        }

        private void GenerateFruit()
        {
            Random r = new Random();
            xAxisOfFruit = r.Next(0, xAmount);
            yAxisOfFruit = r.Next(0, yAmount);
            
            fruit.LocationField = new Point(xAxisOfFruit, yAxisOfFruit);
            PanelGameField.Controls.Add(fruit);
        }

        private void Update (Object myObject, EventArgs eventArgs)
        {
            CheckBorders();
            EatFruit();
            MoveSnake();
        }

        private void EatItself()
        {
            
            for(int i = 1; i < score; i++)
            {
                if(snake[0].LocationField == snake[i].LocationField)
                {
                    for (int j = i; j <= score; j++)
                        this.Controls.Remove(snake[j]);
                    score = score - (score - i + 1);
                    eatItselfSound.Play();
                }
            }
        }

        private void CheckBorders()
        {
            if (snake[0].LocationField.X < 0)
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

            if (snake[0].LocationField.X > xAmount)
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

            if (snake[0].LocationField.Y < 0)
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

            if (snake[0].LocationField.Y > yAmount)
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
                case "Escape":
                    MakeExit(sender, e);
                    break;
            }
        }

        private void MakePause(Object myObject, EventArgs eventArgs)
        {
            if (timer.Enabled == true)
                timer.Stop();
            else
                timer.Start();
            MakeMelodyPause();

        }

        private void MakeExit(Object myObject, EventArgs eventArgs)
        {
            MakePause(myObject, eventArgs);
            
            var result = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                MakePause(myObject, eventArgs);
            }
        }

        private void MelodyStart()
        {
            WMP = new WindowsMediaPlayer();
            WMP.settings.volume = 20;
            WMP.URL = melodyFile;
            WMP.controls.play();
        }

        private void MakeMelodyPause()
        {
            if (WMP.playState == WMPPlayState.wmppsPlaying)
            {
                WMP.controls.pause();
            }
            else if (WMP.playState == WMPPlayState.wmppsPaused)
            {
                WMP.controls.play();
            }

        }


    }

    

}
