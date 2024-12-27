using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public class PlayerController : IGameObject
    {
        public PictureBox PlayerPictureBox { get; private set; }
        private Form parentForm;

        private bool isWPressed = false;
        private bool isAPressed = false;
        private bool isSPressed = false;
        private bool isDPressed = false;

        // Событие для создания ловушки
        public event Action<Point> OnTrapCreated;

        // Таймер для задержки создания ловушек
        private Timer trapCooldownTimer = new Timer();
        private bool canCreateTrap = true; // Флаг, разрешающий создание ловушек
        private int trapCooldown = 2000; // Задержка между созданием ловушек (в миллисекундах)

        public PlayerController(PictureBox pictureBox, Form form)
        {
            PlayerPictureBox = pictureBox;
            parentForm = form;

            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
            form.KeyPreview = true;

            // Настройка таймера для задержки создания ловушек
            trapCooldownTimer.Interval = trapCooldown;
            trapCooldownTimer.Tick += TrapCooldownTimer_Tick;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: isWPressed = true; break;
                case Keys.A: isAPressed = true; break;
                case Keys.S: isSPressed = true; break;
                case Keys.D: isDPressed = true; break;
                case Keys.Space:
                    if (canCreateTrap)
                    {
                        // Создаем ловушку при нажатии пробела
                        OnTrapCreated?.Invoke(new Point(PlayerPictureBox.Left, PlayerPictureBox.Top));
                        canCreateTrap = false; // Запрещаем создание ловушек
                        trapCooldownTimer.Start(); // Запускаем таймер
                    }
                    break;
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: isWPressed = false; break;
                case Keys.A: isAPressed = false; break;
                case Keys.S: isSPressed = false; break;
                case Keys.D: isDPressed = false; break;
            }
        }

        private void TrapCooldownTimer_Tick(object sender, EventArgs e)
        {
            canCreateTrap = true; // Разрешаем создание ловушек
            trapCooldownTimer.Stop(); // Останавливаем таймер
        }

        public void Update()
        {
            int step = 5;
            int newLeft = PlayerPictureBox.Left;
            int newTop = PlayerPictureBox.Top;

            if (isWPressed) newTop -= step;
            if (isAPressed) newLeft -= step;
            if (isSPressed) newTop += step;
            if (isDPressed) newLeft += step;

            if (newLeft >= 0 && newLeft + PlayerPictureBox.Width <= parentForm.ClientSize.Width &&
                newTop >= 0 && newTop + PlayerPictureBox.Height <= parentForm.ClientSize.Height)
            {
                PlayerPictureBox.Left = newLeft;
                PlayerPictureBox.Top = newTop;
            }
        }

        public void Draw()
        {
            // Отрисовка игрока (если нужно)
        }

        public Rectangle CollisionBox => PlayerPictureBox.Bounds; // Границы коллизии
    }
}