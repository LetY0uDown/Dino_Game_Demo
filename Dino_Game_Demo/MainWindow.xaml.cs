using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dino_Game_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(10000);
            jumpTimer.Interval = TimeSpan.FromSeconds(1.5);

            enemies.Add(enemy);

            GameField.Children.Add(character.Body);
            GameField.Children.Add(enemy.Body);

            Canvas.SetBottom(character.Body, GAME_FIELD_HEIGHT);
            Canvas.SetLeft(character.Body, CHARACTER_POS);

            Canvas.SetBottom(enemy.Body, GAME_FIELD_HEIGHT);
            Canvas.SetLeft(enemy.Body, ENEMY_START_POS);

            DrawRocks();

            jumpTimer.Tick += JumpTimer_Tick;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private const int ENEMY_START_POS = 780;
        private const int GAME_FIELD_HEIGHT = 162;

        private const int CHARACTER_POS = 50;

        private int scores = 0;

        private List<Entity> enemies = new();

        private DispatcherTimer timer = new();
        private DispatcherTimer jumpTimer = new();

        private Random random = new();

        private Entity enemy = new()
        {
            Body = new Rectangle
            {
                Fill = Brushes.Red,
                Width = 40,
                Height = 40
            },
            Position = new()
            {
                X = ENEMY_START_POS,
                Y = GAME_FIELD_HEIGHT
            }
        };
        private Entity character = new()
        {
            Body = new Rectangle
            {
                Fill = Brushes.Cyan,
                Width = 50,
                Height = 50
            },
            Position = new()
            {
                X = CHARACTER_POS,
                Y = GAME_FIELD_HEIGHT
            }
        };

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (random.Next(0, 500) == 0 || enemies.Count == 0)
                SpawnEnemy();

            foreach (var enemy in enemies.ToList())
            {
                MoveEnemy(enemy);
                CollisionCheck(enemy);
            }

            tbScores.Text = $"Score: {scores++}";
        }

        private void CollisionCheck(Entity enemy)
        {
            if (enemy.Position.X < character.Position.X + CHARACTER_POS && enemy.Position.X + 40 > character.Position.X
                && enemy.Position.Y == character.Position.Y)
            {
                GameField.Children.Remove(character.Body);
                timer.Stop();
                MessageBox.Show("You died!", "ooops");
            }
        }

        private void MoveEnemy(Entity enemy)
        {
            enemy.Position.X -= 2;
            Canvas.SetLeft(enemy.Body, enemy.Position.X);

            if (enemy.Position.X < -20)
                RemoveEnemy(enemy);
        }

        private void SpawnEnemy()
        {
            enemies.Add(new Entity
            {
                Body = new Rectangle()
                {
                    Fill = Brushes.Red,
                    Width = 40,
                    Height = 40
                },
                Position = new()
                {
                    X = ENEMY_START_POS,
                    Y = GAME_FIELD_HEIGHT
                }
            });

            GameField.Children.Add(enemies[^1].Body);

            Canvas.SetBottom(enemies[^1].Body, GAME_FIELD_HEIGHT);
            Canvas.SetLeft(enemies[^1].Body, ENEMY_START_POS);
        }
        private void RemoveEnemy(Entity enemy)
        {
            enemies.Remove(enemy);
            GameField.Children.Remove(enemy.Body);
        }

        private void SetCharacterHeight(int newHeight)
        {
            character.Position.Y = newHeight;
            Canvas.SetBottom(character.Body, newHeight);
        }

        private void Jump()
        {
            jumpTimer.Start();
            character.Position.Y += 100;
            Canvas.SetBottom(character.Body, character.Position.Y);
        }

        private void JumpTimer_Tick(object? sender, EventArgs e)
        {            
            character.Position.Y = GAME_FIELD_HEIGHT;
            Canvas.SetBottom(character.Body, GAME_FIELD_HEIGHT);

            jumpTimer.Stop();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && character.Position.Y == GAME_FIELD_HEIGHT)
                Jump();
        }

        private void DrawRocks()
        {
            for (int i = 0; i <= 15; i++)
            {
                int var = random.Next(4, 8);

                GameField.Children.Add(new Rectangle
                {
                    Fill = Brushes.Gray,
                    Height =  var,
                    Width = var
                });

                Canvas.SetTop(GameField.Children[^1], random.Next(280, 290));
                Canvas.SetLeft(GameField.Children[^1], random.Next(20, 780));
            }
        }
    }
}
