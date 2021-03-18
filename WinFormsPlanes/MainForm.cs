using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsPlanes
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            KeyPress += new KeyPressEventHandler(KeyListener);
        }

        #region Vars

        bool gameIsStarted = false;
        bool isPushed = false;
        int tmrTick = 50;
        Random rnd = new Random();
        int controlsZero = 0;

        bool stopped = true;
        bool alive = false;
        int hpMax = 100;
        int hpCurrent = 0;
        int deathTime = 50;
        int currentDeathTime = 0;

        int spaceshipWidth = 50;
        int spaceshipHeight = 50;
        int positionOfShips = 10;

        int deltaStand = 3;
        int spaceshipSpeed = 20;
        int countOfEnemies = 5;

        string bulletName = "Bullet";
        int bulletsCount = 0;
        int bulletWidth = 14;
        int bulletHeight = 64;
        int bulletsSpeed = 25;
        int bulletsRangeBetween = 5;
        int tmpBulletsRange = 0;
        int damage = 5;
        int qBullets = 2; // p = 1 / qBullets

        string explosionName = "Explosion";
        int explosionsCount = 0;
        int explosionWidth = 64;
        int explosionHeight = 64;
        int explosionTimeOfLife = 3;

        string cloudName = "Cloud";
        int cloudsCount = 0;
        int cloudsLimit = 10;
        int cloudWidth = 232;
        int cloudHeight = 109;
        int cloudSpeed = 4;
        int cloudDeltaSize = 100;
        int qClouds = 50; // p = 1 / qCloud

        bool spaceshipToRight = true;
        int route = 0;
        int[] moveLimits;

        Point spaceshipPoint;
        List<Point> enemiesPoints = new List<Point>();

        #endregion
        #region Events

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowPreviewGui();
        }
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            OnClick();
        }
        private void lblTitle_Click(object sender, EventArgs e)
        {
            OnClick();
        }
        private void lblClick_Click(object sender, EventArgs e)
        {
            OnClick();
        }
        void OnClick()
        {
            if (!isPushed)
            {
                isPushed = true;
                Play();
            }
        }
        void Play()
        {
            ShowPlayingGui();
            if (!gameIsStarted)
            {
                InitializeBitmaps();
                AddPlayerAndEnemies();
                InitializePlayerAndEnemiesPositions();
            }

            mainTimer.Interval = tmrTick;
            mainTimer.Start();

            gameIsStarted = true;
        }
        private void mainTimer_Tick(object sender, EventArgs e)
        {
            if (!stopped)
                Animate();
        }

        #endregion
        #region Gui

        void ShowPreviewGui()
        {
            BackColor = Color.FromArgb(5, 0, 10);

            lblHp.Visible = false;

            lblTitle.Font = new Font("Comic Sans MS", 64);
            lblTitle.ForeColor = Color.FromArgb(200, 200, 220);
            lblTitle.Location = new Point((Size.Width - lblTitle.Width) / 2,
                (Size.Height - lblTitle.Height) / 2 - 200);

            lblClick.Font = new Font("Comic Sans MS", 16);
            lblClick.ForeColor = Color.FromArgb(200, 100, 110);
            lblClick.Location = new Point((Size.Width - lblClick.Width) / 2,
                lblTitle.Location.Y + lblTitle.Height + 10);

            lblInfo.Font = new Font("Comic Sans MS", 12);
            lblInfo.ForeColor = Color.FromArgb(200, 200, 200);
            lblInfo.Location = new Point((Size.Width - lblClick.Width) / 2,
                lblClick.Location.Y + lblClick.Height + 200);
        }
        void ShowPlayingGui()
        {
            RefreshPlayer();

            BackColor = Color.FromArgb(0, 136, 216);

            lblHp.Text = string.Format("HP: {0}%", hpCurrent);
            lblHp.Font = new Font("Comic Sans MS", 16, FontStyle.Bold);
            lblHp.ForeColor = Color.FromArgb(200, 15, 20);
            lblHp.Visible = true;

            lblTitle.Visible = false;
            lblClick.Visible = false;
            lblInfo.Visible = false;
        }
        void ShowGameOverGui()
        {
            isPushed = false;
            BackColor = Color.FromArgb(0, 20, 55);

            lblHp.Visible = false;

            lblTitle.Visible = true;
            lblTitle.ForeColor = Color.FromArgb(200, 200, 220);
            lblTitle.BackColor = Color.FromArgb(100, 0, 0);

            lblClick.Visible = true;
            lblClick.ForeColor = Color.FromArgb(200, 100, 110);
            lblClick.BackColor = Color.FromArgb(100, 0, 0);
        }

        #endregion
        #region Initialize

        Bitmap spaceshipBitmap;
        Bitmap enemyBitmap;
        Bitmap bulletBitmap;
        Bitmap explosionBitmap;
        Bitmap cloudSmallBitmap;

        void InitializeBitmaps()
        {
            spaceshipBitmap = new Bitmap(Properties.Resources.Spaceship, spaceshipWidth, spaceshipHeight);
            enemyBitmap = new Bitmap(Properties.Resources.SpaceshipEnemy, spaceshipWidth, spaceshipHeight);
            bulletBitmap = new Bitmap(Properties.Resources.Bullet, bulletWidth, bulletHeight);
            explosionBitmap = new Bitmap(Properties.Resources.Explosion, explosionWidth, explosionHeight);
            cloudSmallBitmap = new Bitmap(Properties.Resources.CloudSmall, cloudWidth, cloudHeight);
        }
        void AddPlayerAndEnemies()
        {
            controlsZero = Controls.Count;
            PictureBox player = new PictureBox();
            
            player.Image = spaceshipBitmap;
            player.Dock = DockStyle.Fill;
            player.Width = spaceshipWidth;
            player.Height = spaceshipHeight;
            player.Anchor = AnchorStyles.Top;
            player.Region = new Region(BuildTransparentPath(spaceshipBitmap));
            Controls.Add(player);

            for (int i = 0; i < countOfEnemies; i++)
            {
                PictureBox enemy = new PictureBox();

                enemy.Image = enemyBitmap;
                enemy.Dock = DockStyle.Fill;
                enemy.Width = spaceshipWidth;
                enemy.Height = spaceshipHeight;
                enemy.Anchor = AnchorStyles.Top;
                enemy.Region = new Region(BuildTransparentPath(enemyBitmap));
                Controls.Add(enemy);
            }
        }
        void InitializePlayerAndEnemiesPositions()
        {
            positionOfShips = Size.Width / 2 - (countOfEnemies / 2 * (spaceshipWidth + spaceshipWidth / 2));

            // player
            spaceshipPoint = new Point(
                positionOfShips + countOfEnemies / 2 * (spaceshipWidth + spaceshipWidth / 2),
                Size.Height - 2 * spaceshipWidth - 20);
            Controls[controlsZero].Location = spaceshipPoint;

            // enemies
            int tmpPosition = positionOfShips;
            for (int i = 0; i < countOfEnemies; i++)
            {
                Point point = new Point(tmpPosition, 10);
                tmpPosition += spaceshipWidth + spaceshipWidth / 2;
                enemiesPoints.Add(point);

                Controls[controlsZero + i + 1].Location = point;
            }
            tmpPosition -= spaceshipWidth + spaceshipWidth / 2;

            moveLimits = new int[] { positionOfShips, tmpPosition };
        }

        #endregion
        #region Trajectories

        Point ControlRightLeftSpaceshipTrajectory(Point position)
        {
            int delta = spaceshipSpeed * route;
            
            Point res = new Point(position.X + delta, position.Y);
            if (res.X < moveLimits[0] || res.X > moveLimits[1])
                return position;
            return res;
        }
        Point RightLeftSpaceshipTrajectory(Point position)
        {
            int delta = spaceshipSpeed;
            if (position.X < moveLimits[0] || position.X > moveLimits[1])
                spaceshipToRight = !spaceshipToRight;
            if (!spaceshipToRight)
                delta *= -1;
            Point res = new Point(position.X + delta, position.X);
            return res;
        }
        Point StandRandomSpaceshipTrajectory(Point position)
        {
            Point res = new Point(position.X + rnd.Next(-deltaStand, deltaStand),
                position.Y + rnd.Next(-deltaStand, deltaStand));
            return res;
        }
        Point BulletsTrajectory(Point position)
        {
            return new Point(position.X, position.Y + bulletsSpeed);
        }
        Point CloudsTrajectory(Point position, int speed)
        {
            return new Point(position.X, position.Y + speed);
        }

        #endregion
        #region Animation

        void Animate()
        {
            AnimateSpaceship();
            AnimateEnemies();
            AnimateBullets();
            AnimateExplosions();
            AnimateClouds();
            if (!alive) GameOver();
        }
        void AnimateSpaceship()
        {
            spaceshipPoint = ControlRightLeftSpaceshipTrajectory(spaceshipPoint);
            Point pos = StandRandomSpaceshipTrajectory(spaceshipPoint);
            Controls[controlsZero].Location = pos;
        }
        void AnimateEnemies()
        {
            for (int i = 0; i < countOfEnemies; i++)
            {
                Point pos = StandRandomSpaceshipTrajectory(enemiesPoints[i]);
                Controls[controlsZero + 1 + i].Location = pos;
            }
        }
        void AnimateBullets()
        {
            // despawn & moving
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Name.ToString() == bulletName)
                {
                    if (Controls[i].Location.Y > Size.Height)
                    {
                        Controls[i].Dispose();
                        bulletsCount--;
                        i--;
                    }
                    else
                        Controls[i].Location = BulletsTrajectory(Controls[i].Location);
                }
            }

            // spawn
            if (tmpBulletsRange >= bulletsRangeBetween)
            {
                tmpBulletsRange = 0;
                if (rnd.Next(qBullets) == 0)
                {
                    int enemy = rnd.Next(countOfEnemies);

                    Point pos = new Point(
                        Controls[controlsZero + 1 + enemy].Location.X + bulletWidth,
                        Controls[controlsZero + 1 + enemy].Location.Y + bulletHeight);

                    PictureBox bullet = new PictureBox();

                    bullet.Image = bulletBitmap;
                    bullet.Dock = DockStyle.Fill;
                    bullet.Width = bulletWidth;
                    bullet.Height = bulletHeight;
                    bullet.Anchor = AnchorStyles.Top;
                    bullet.Region = new Region(BuildTransparentPath(bulletBitmap));
                    Controls.Add(bullet);
                    bullet.Location = pos;
                    bullet.Name = bulletName;

                    bulletsCount++;
                }
            }
            else tmpBulletsRange++;
        }
        void AnimateExplosions()
        {
            // despawn & moving
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Name.ToString() == explosionName)
                {
                    //if (explosionsTimes[i] > explosionTimeOfLife)

                    if (int.Parse(Controls[i].Tag.ToString()) > explosionTimeOfLife)
                    {
                        Controls[i].Dispose();
                        explosionsCount--;
                        i--;
                    }
                    else
                    {
                        Controls[i].Location = StandRandomSpaceshipTrajectory(Controls[i].Location);
                        Controls[i].Tag = (int.Parse(Controls[i].Tag.ToString()) + 1);
                    }
                }
            }

            // spawn
            for (int i = 0; i < Controls.Count; i++)            
            {
                if (alive &&
                    Controls[i].Name.ToString() == bulletName &&
                    Controls[i].Location.X + bulletWidth > spaceshipPoint.X &&
                    Controls[i].Location.X < spaceshipPoint.X + spaceshipWidth &&
                    Controls[i].Location.Y + bulletHeight > spaceshipPoint.Y &&
                    Controls[i].Location.Y < spaceshipPoint.Y + spaceshipHeight)
                {
                    PictureBox explosion = new PictureBox();

                    explosion.Image = explosionBitmap;
                    explosion.Dock = DockStyle.Fill;
                    explosion.Width = explosionWidth;
                    explosion.Height = explosionHeight;
                    explosion.Anchor = AnchorStyles.Top;
                    explosion.Region = new Region(BuildTransparentPath(explosionBitmap));
                    Controls.Add(explosion);
                    explosion.Location = spaceshipPoint;
                    explosion.Tag = 0;
                    explosion.Name = explosionName;

                    explosionsCount++;

                    MakeDmg();
                }
            }
        }
        void AnimateClouds()
        {
            // despawn & moving
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Name.ToString() == cloudName)
                {
                    if (Controls[i].Location.Y > Size.Height)
                    {
                        Controls[i].Dispose();
                        cloudsCount--;
                        i--;
                    }
                    else
                        Controls[i].Location = CloudsTrajectory(Controls[i].Location, cloudSpeed);
                }
            }

            // spawn
            if (cloudsCount < cloudsLimit)
            {
                if (rnd.Next(qClouds) == 0)
                {
                    int tmpWidth = cloudWidth + 
                        rnd.Next(cloudWidth - cloudDeltaSize, cloudWidth + cloudDeltaSize);
                    int tmpHeight = cloudHeight +
                        rnd.Next(cloudHeight - cloudDeltaSize, cloudHeight + cloudDeltaSize);

                    int rndX = rnd.Next(Size.Width - tmpWidth);
                    
                    PictureBox cloudSmall = new PictureBox();

                    cloudSmall.Image = cloudSmallBitmap;
                    cloudSmall.Dock = DockStyle.Fill;
                    cloudSmall.Width = tmpWidth;
                    cloudSmall.Height = tmpHeight;
                    cloudSmall.Anchor = AnchorStyles.Top;
                    cloudSmall.Region = new Region(BuildTransparentPath(cloudSmallBitmap));
                    Controls.Add(cloudSmall);
                    cloudSmall.Location = new Point(rndX, -tmpHeight);
                    cloudSmall.Name = cloudName;

                    cloudsCount++;
                }
            }
        }

        void AnimateDeath()
        {
            int delta = 40;
            int deltaX = rnd.Next(-delta, delta);
            int deltaY = rnd.Next(-delta, delta);

            // despawn & moving
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Name.ToString() == explosionName)
                {
                    if (int.Parse(Controls[i].Tag.ToString()) > explosionTimeOfLife)
                    {
                        Controls[i].Dispose();
                        explosionsCount--;
                        i--;
                    }
                    else
                    {
                        Controls[i].Location = StandRandomSpaceshipTrajectory(Controls[i].Location);
                        Controls[i].Tag = (int.Parse(Controls[i].Tag.ToString()) + 1);
                    }
                }
            }

            // spawn
            PictureBox explosion = new PictureBox();

            explosion.Image = explosionBitmap;
            explosion.Dock = DockStyle.Fill;
            explosion.Width = explosionWidth;
            explosion.Height = explosionHeight;
            explosion.Anchor = AnchorStyles.Top;
            explosion.Region = new Region(BuildTransparentPath(explosionBitmap));
            Controls.Add(explosion);
            explosion.Location = new Point(
                spaceshipPoint.X + deltaX, spaceshipPoint.Y + deltaY);
            explosion.Tag = 0;
            explosion.Name = explosionName;
        }

        #endregion
        #region Processing

        void MakeDmg()
        {
            hpCurrent -= damage;

            AnimateGui();
            if (hpCurrent <= 0)
                GameOver();
        }
        void AnimateGui()
        {
            lblHp.Text = string.Format("HP: {0}%", hpCurrent);
        }
        void GameOver()
        {
            lblHp.Text = string.Format("HP: {0}%", 0);
            alive = false;
            AnimateDeath();
            currentDeathTime--;
            if (currentDeathTime < 1)
            {
                stopped = true;
                ShowGameOverGui();
                DisposeOldElements();
            }
        }
        void DisposeOldElements()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Name.ToString() == bulletName)
                {
                    Controls[i].Dispose();
                    bulletsCount--;
                    i--;
                }
            }
        }
        void RefreshPlayer()
        {
            alive = true;
            stopped = false;
            hpCurrent = hpMax;
            currentDeathTime = deathTime;
        }

        private void KeyListener(object sender, KeyPressEventArgs ex)
        {
            if (alive)
            {
                if (ex.KeyChar == 'd' || ex.KeyChar == 'D' || ex.KeyChar == 'в' || ex.KeyChar == 'В')
                    route = 1;

                else if (ex.KeyChar == 'a' || ex.KeyChar == 'A' || ex.KeyChar == 'ф' || ex.KeyChar == 'Ф')
                    route = -1;
            }
            else
            {
                if (stopped)
                {
                    if (ex.KeyChar == 'w') OnClick();
                    else OnClick();
                }
            }

            ex.Handled = true;
        }

        static System.Drawing.Drawing2D.GraphicsPath BuildTransparentPath(Bitmap bmp)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            Color mask = Color.FromArgb(0, 0, 0, 0);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (!bmp.GetPixel(i, j).Equals(mask))
                        gp.AddRectangle(new Rectangle(i, j, 1, 1));
                }
            }
            return gp;
        }
        #endregion
    }
}