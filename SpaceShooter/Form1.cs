using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SpaceShooter
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootMedia;
        WindowsMediaPlayer explosion;

        PictureBox[] stars;
        int backgroundSpeed;
        int playerSpeed;

        PictureBox[] munitions;
        int MunitionSpeed;

        PictureBox[] enemies;
        int enemySpeed;

        PictureBox[] enemyMunitions;
        int enemyMunitionSpeed;

        Random rnd;


        int score;
        bool pause;
        bool gameIsOver;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            gameIsOver = false;
            score = 0;

            backgroundSpeed = 4;
            playerSpeed = 4;
            enemySpeed = 4;

            MunitionSpeed = 20;
            munitions = new PictureBox[3];

            enemyMunitionSpeed = 4;
            enemyMunitions = new PictureBox[10];

            rnd = new Random();

            // Load images
            Image munition = Image.FromFile(@"..\..\Assets\munition.png");

            Image enemy1 = Image.FromFile(@"Assets\en1.png");
            Image enemy2 = Image.FromFile(@"Assets\en2.png");
            Image enemy3 = Image.FromFile(@"Assets\en2.png");
            Image boss1 = Image.FromFile(@"Assets\boss1.png");
            Image boss2 = Image.FromFile(@"Assets\boss1.png");

            enemies = new PictureBox[10];

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }
            
            enemies[0].Image = boss1;
            enemies[1].Image = enemy2;
            enemies[2].Image = enemy3;
            enemies[3].Image = enemy1; 
            enemies[4].Image = enemy2; 
            enemies[5].Image = enemy3; 
            enemies[6].Image = enemy1; 
            enemies[7].Image = enemy2; 
            enemies[8].Image = enemy3; 
            enemies[9].Image = boss2;

            for (int i = 0; i < enemyMunitions.Length; i++)
            {
                enemyMunitions[i] = new PictureBox();
                enemyMunitions[i].Size = new Size(2, 25);
                enemyMunitions[i].Visible = false;
                enemyMunitions[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                enemyMunitions[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemyMunitions[i]);
            }


            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            //Create WMP
            gameMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();

            gameMedia.URL = @"Sounds\gameSoundtrack.wav";
            shootMedia.URL = @"Sounds\laser.wav";
            explosion.URL = @"Sounds\explosion.wav";

            //Setup Sounds Settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 3;
            explosion.settings.volume = 4;
            shootMedia.settings.volume = 1;

            stars = new PictureBox[10];


            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(15, 585), rnd.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }

                this.Controls.Add(stars[i]);
            }
            gameMedia.controls.play();
            shootMedia.controls.stop();
            explosion.controls.stop();
        }

        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += backgroundSpeed;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundSpeed - 2;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Location = new Point(rnd.Next(5, 595), rnd.Next(-10, 400));

                }
            }
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }
        
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause) 
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                RightMoveTimer.Stop();
            }
            if (e.KeyCode == Keys.Left)
            {
                LeftMoveTimer.Stop();
            }
            if (e.KeyCode == Keys.Down)
            {
                DownMoveTimer.Stop();
            }
            if (e.KeyCode == Keys.Up)
            {
                UpMoveTimer.Stop();
            }
            if (e.KeyCode == Keys.Space)
            {
                if (!gameIsOver) 
                {
                    if (pause)
                    {
                        StartTimers();
                        label.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label.Location = new Point(130, 150);
                        label.Text = "Game Paused";
                        label.Visible = true;
                        gameMedia.controls.pause();
                        StopTimers();
                        pause = true;
                    }
                }
            }
        }

        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0 && munitions[i].Visible == false) 
                {
                    shootMedia.controls.play();
                }
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= MunitionSpeed;
                    Collision();
                }
                else 
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemyTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemySpeed);
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i+1) * 50, -200);
                }
            }
        }

        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++) 
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();

                    score += 1;
                    scoreLabel.Text = (score < 10) ? "Score:  0" + score.ToString() : "Score:  " + score.ToString();

                    if (score % 30 == 0)
                    {
                        enemySpeed++;
                        enemyMunitionSpeed++;
                    }

                    if (score >= 300)
                    {
                        GameOver("GOOD JOB!");
                    }

                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }
                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 9;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        
        }

        private void GameOver(string str)
        {
            label.Text = str;
            label.Visible = true;
            replayButton.Visible = true;
            quitButton.Visible = true;

            gameMedia.controls.stop();
            StopTimers();
        }

        private void StopTimers() 
        {
            MoveBgTimer.Stop();
            MoveEnemyTimer.Stop();
            MoveMunitionTimer.Stop();
            enemyMunitionTimer.Stop();
        }

        private void StartTimers()
        {
            MoveBgTimer.Start();
            MoveEnemyTimer.Start();
            MoveMunitionTimer.Start();
            enemyMunitionTimer.Start();
        }

        private void enemyMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < enemyMunitions.Length; i++)
            {
                if (enemyMunitions[i].Top < this.Height)
                {
                    enemyMunitions[i].Visible = true;
                    enemyMunitions[i].Top += enemyMunitionSpeed;

                    CollisionWithEnemyMunition();
                }
                else
                {
                    enemyMunitions[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemyMunitions[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        private void CollisionWithEnemyMunition() 
        {
            for (int i = 0; i < enemyMunitions.Length; i++)
            {
                if (enemyMunitions[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemyMunitions[i].Visible = false;
                    explosion.settings.volume = 9;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void replayButton_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }
    }
}