// Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public partial class Form1 : Form
    {
        private List<IGameObject> gameObjects = new List<IGameObject>(); // Список всех объектов
        private PlayerController playerController; // Управление игроком
        private Timer gameTimer; // Таймер для игрового цикла
        private Timer spawnTimer; // Таймер для спавна врагов
        private Timer gameEndTimer; // Таймер для отсчета времени до конца игры
        private int score = 0; // Счет игрока
        private Label scoreLabel; // Label для отображения счета
        private Label timeLabel; // Label для отображения оставшегося времени
        private SpawnManager spawnManager; // Менеджер спавна врагов
        private CollisionManager collisionManager; // Менеджер проверки коллизий
        private int remainingTime = 120; // Оставшееся время в секундах (2 минуты)
        private int targetScore = 100; // Целевое количество очков для победы

        public Form1()
        {
            InitializeComponent();

            // Вычисляем центр формы для спавна игрока
            int playerX = (this.ClientSize.Width - pictureBox1.Width) / 2;
            int playerY = (this.ClientSize.Height - pictureBox1.Height) / 2;
            pictureBox1.Location = new Point(playerX, playerY);

            // Создаем игрока и добавляем его в список
            playerController = new PlayerController(pictureBox1, this);
            gameObjects.Add(playerController);

            // Создаем Label для отображения счета
            scoreLabel = new Label
            {
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 16),
                Text = $"Очки: 0/{targetScore}"
            };
            this.Controls.Add(scoreLabel);

            // Создаем Label для отображения времени
            timeLabel = new Label
            {
                Location = new Point(10, 40), // Располагаем ниже счета
                AutoSize = true,
                Font = new Font("Arial", 16),
                Text = $"Время: {remainingTime}"
            };
            this.Controls.Add(timeLabel);

            // Инициализация менеджера спавна
            spawnManager = new SpawnManager(this, gameObjects);

            // Настройка таймеров
            // Тики игры
            gameTimer = new Timer();
            gameTimer.Interval = 20; // 20 мс (50 FPS)
            gameTimer.Tick += Timer_Tick;

            // Спавн противников
            spawnTimer = new Timer();
            spawnTimer.Interval = 2000; // 2 секунды
            spawnTimer.Tick += SpawnTimer_Tick;

            // Счетчик времени раунда
            gameEndTimer = new Timer();
            gameEndTimer.Interval = 1000; // 1 секунда
            gameEndTimer.Tick += GameEndTimer_Tick;

            // Инициализация менеджера коллизий (после инициализации таймеров)
            collisionManager = new CollisionManager(playerController, gameObjects, this, scoreLabel, ref score, gameTimer, spawnTimer, gameEndTimer, targetScore);

            // Подписываемся на событие создания ловушки
            playerController.OnTrapCreated += CreateNewTrap;

            // Запускаем таймеры
            gameTimer.Start();
            spawnTimer.Start();
            gameEndTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            playerController.Update(); // Обновляем игрока
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(); // Обновляем все объекты
            }
            collisionManager.CheckCollisions(); // Проверяем столкновения
        }

        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            // Используем SpawnManager для создания нового врага
            spawnManager.SpawnEnemy();
        }

        private void GameEndTimer_Tick(object sender, EventArgs e)
        {
            // Уменьшаем оставшееся время
            remainingTime--;
            timeLabel.Text = $"Время: {remainingTime}";

            // Проверяем, не истекло ли время
            if (remainingTime <= 0)
            {
                // Останавливаем все таймеры
                gameTimer.Stop();
                spawnTimer.Stop();
                gameEndTimer.Stop();

                // Показываем сообщение о конце игры
                MessageBox.Show("Время вышло! Игра окончена.");
            }
        }

        private void CreateNewTrap(Point position)
        {
            Trap trap = new Trap(position);
            gameObjects.Add(trap);
            this.Controls.Add(trap.TrapPictureBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Код, который должен выполняться при загрузке формы
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Код, который должен выполняться при клике на pictureBox1
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}