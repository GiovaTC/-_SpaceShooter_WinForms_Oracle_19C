using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceShooter_WinForms_Oracle_19C__
{
    public partial class Form1 : Form
    {
        private Rectangle player;
        private List<Rectangle> asteroids = new();
        private Random rnd = new();
        private int score = 0;
        private bool gameOver = false;
        private System.Windows.Forms.Timer gameTimer = new();
        private Label lblScore = new();
        private Button btnSave = new();
        private int spawnCounter = 0;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 400;
            this.Height = 600;
            this.Text = "Space Shooter";

            player = new Rectangle(this.ClientSize.Width / 2 - 20, this.ClientSize.Height - 60, 40, 40);

            lblScore.Text = "Score: 0";
            lblScore.ForeColor = Color.White;
            lblScore.Dock = DockStyle.Top;
            lblScore.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblScore);

            btnSave.Text = "guardar puntaje";
            btnSave.Dock = DockStyle.Bottom;
            btnSave.Enabled = false;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;

            gameTimer.Interval = 30;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string playerName = Microsoft.VisualBasic.Interaction.InputBox("nombre del jugador:", "guardar puntaje", "jugador1");
            // Puede cambiar esto para obtener el nombre del jugador de otra manera
            DataAccess.SaveCore(playerName, score);
            MessageBox.Show("Puntaje guardado correctamente en oracle.!");
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (gameOver) return;
            int move = 10;
            if (e.KeyCode == Keys.Left && player.X > 0) player.X -= move;
            if (e.KeyCode == Keys.Right && player.X < this.ClientSize.Width - player.Width) player.X += move;
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            spawnCounter++;
            if (spawnCounter >= 20)
            {
                spawnCounter = 0;
                int w = rnd.Next(20, 50);
                int x = rnd.Next(0, this.ClientSize.Width - w);
                asteroids.Add(new Rectangle(x, -w, w, w));
            }

            for (int i = asteroids.Count - 1; i >= 0; i--)
            {
                var r = asteroids[i];
                r.Y += 6 + (score / 50);
                asteroids[i] = r;

                if (r.IntersectsWith(player))
                {
                    GameOver();
                    return;
                }

                if (r.Y > this.ClientSize.Height)
                {
                    asteroids.RemoveAt(i);
                    score += 10;
                    lblScore.Text = "Score: " + score;
                }
            }
            Invalidate();
        }

        private void GameOver()
        {
            gameOver = true;
            gameTimer.Stop();
            btnSave.Enabled = true;
            MessageBox.Show(this, "Game Over! Puntaje: " + score, "Fin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            using (var b = new SolidBrush(Color.Black))
                g.FillRectangle(b, this.ClientRectangle);

            Point[] triangle = new Point[] {
                new Point(player.X + player.Width / 2, player.Y),
                new Point(player.X, player.Y + player.Height),
                new Point(player.X + player.Width, player.Y + player.Height)
            };
            g.FillPolygon(Brushes.Cyan, triangle);

            foreach (var a in asteroids)
                g.FillEllipse(Brushes.Gray, a);

            if (gameOver)
            {
                var msg = "GAME OVER";
                var f = new Font("Segoe UI", 28, FontStyle.Bold);
                var size = g.MeasureString(msg, f);
                g.DrawString(msg, f, Brushes.Red,
                    (ClientSize.Width - size.Width) / 2,
                    (ClientSize.Height - size.Height) / 2);
            }
        }

        private void Form1_Load(object? sender, KeyEventArgs e)
        {
    
        }
    }
}
