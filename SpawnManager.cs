// SpawnManager.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public class SpawnManager
    {
        private Form parentForm;
        private List<IGameObject> gameObjects;
        private Timer spawnTimer;
        private int enemyCount = 1; // Начальное количество врагов
        private DateTime lastEnemyIncrease = DateTime.Now;

        // Добавленный конструктор
        public SpawnManager(Form parentForm, List<IGameObject> gameObjects)
        {
            this.parentForm = parentForm;
            this.gameObjects = gameObjects;
        }

        public void SpawnEnemy()
        {
            Random random = new Random();
            Point startPosition;

            // Выбираем случайную сторону карты для спавна
            int side = random.Next(0, 4); // 0 - верх, 1 - право, 2 - низ, 3 - лево

            switch (side)
            {
                case 0: // Верх
                    startPosition = new Point(random.Next(0, parentForm.ClientSize.Width - 35), 0);
                    break;
                case 1: // Право
                    startPosition = new Point(parentForm.ClientSize.Width - 35, random.Next(0, parentForm.ClientSize.Height - 35));
                    break;
                case 2: // Низ
                    startPosition = new Point(random.Next(0, parentForm.ClientSize.Width - 35), parentForm.ClientSize.Height - 35);
                    break;
                case 3: // Лево
                    startPosition = new Point(0, random.Next(0, parentForm.ClientSize.Height - 35));
                    break;
                default:
                    startPosition = new Point(0, 0);
                    break;
            }

            Enemy enemy = new Enemy(parentForm.Controls["pictureBox1"] as PictureBox, startPosition);
            gameObjects.Add(enemy);
            parentForm.Controls.Add(enemy.PictureBox);
        }
    }
}