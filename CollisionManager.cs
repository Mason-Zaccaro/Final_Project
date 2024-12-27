using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyGame
{
    public class CollisionManager
    {
        private PlayerController playerController;
        private List<IGameObject> gameObjects;
        private Form parentForm;
        private Label scoreLabel;
        private int score;
        private Timer gameTimer;
        private Timer spawnTimer;
        private Timer gameEndTimer;
        private int targetScore;

        public CollisionManager(
            PlayerController playerController,
            List<IGameObject> gameObjects,
            Form parentForm, Label scoreLabel,
            ref int score, Timer gameTimer,
            Timer spawnTimer,
            Timer gameEndTimer,
            int targetScore)
        {
            this.playerController = playerController;
            this.gameObjects = gameObjects;
            this.parentForm = parentForm;
            this.scoreLabel = scoreLabel;
            this.score = score;
            this.gameTimer = gameTimer;
            this.spawnTimer = spawnTimer;
            this.gameEndTimer = gameEndTimer;
            this.targetScore = targetScore;
        }

        public void CheckCollisions()
        {
            // Получаем границы коллизии игрока
            Rectangle playerCollisionBox = playerController.CollisionBox;

            // Проверка столкновений игрока с врагами
            foreach (var enemy in gameObjects.OfType<Enemy>().ToList())
            {
                if (playerCollisionBox.IntersectsWith(enemy.CollisionBox))
                {
                    // Оповещаем о конце игры
                    OnGameOver();
                    return;
                }

                // Проверка столкновений ловушек с врагами
                foreach (var trap in gameObjects.OfType<Trap>().ToList())
                {
                    if (trap.CollisionBox.IntersectsWith(enemy.CollisionBox))
                    {
                        // Удаляем врага и ловушку
                        gameObjects.Remove(enemy);
                        gameObjects.Remove(trap);
                        parentForm.Controls.Remove(enemy.PictureBox);
                        parentForm.Controls.Remove(trap.TrapPictureBox);

                        // Увеличиваем счет
                        score += 10;
                        scoreLabel.Text = $"Очки: {score}/{targetScore}";

                        // Проверяем, достигнут ли целевой счет
                        if (score >= targetScore)
                        {
                            OnGameWin();
                            return; // Выходим из метода, чтобы избежать дальнейших проверок
                        }

                        break;
                    }
                }
            }
        }

        private void OnGameOver()
        {
            gameTimer.Stop();
            spawnTimer.Stop();
            gameEndTimer.Stop();

            // Показываем сообщение о конце игры
            MessageBox.Show("Игра окончена! Вы проиграли.");
        }

        private void OnGameWin()
        {
            gameTimer.Stop();
            spawnTimer.Stop();
            gameEndTimer.Stop();

            MessageBox.Show("Поздравляем! Вы набрали достаточно очков и выиграли!");
        }
    }
}