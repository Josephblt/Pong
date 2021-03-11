using Pong.GameStates;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pong
{
    public partial class GameWindow : Form
    {
        #region Constructor

        private GameWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            GameStatesManager = new GameStatesManager();
        }

        #endregion

        #region Singleton

        private static GameWindow instance;

        public static GameWindow GetGameWindow()
        {
            if (instance == null)
                instance = new GameWindow();
            return instance;
        }

        #endregion

        #region Attributes and Properties

        public bool GameRunning { get; set; }

        public GameStatesManager GameStatesManager { get; private set; }

        public string GameWinner { get; set; }

        public bool Paused { get; private set; }

        #endregion        

        #region Overriden Methods

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            GameRunning = false;
            base.OnFormClosing(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GameStatesManager.InitializeManager();
            StartMainLoop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;            

            if (GameStatesManager.CurrentGameState != null)
                GameStatesManager.CurrentGameState.Render(e.Graphics);
        }

        #endregion

        #region Private Methods

        private void StartMainLoop()
        {
            GameRunning = true;
            
            var timer = new Stopwatch();
            timer.Start();

            var lastTime = (float)timer.Elapsed.TotalSeconds;

            while (GameRunning)
            {
                var gameTime = (float)timer.Elapsed.TotalSeconds;
                var elapsedTime = gameTime - lastTime;
                lastTime = gameTime;
                Application.DoEvents();
                
                if (!Paused)
                {
                    if (!timer.IsRunning)
                        timer.Start();

                    if (GameStatesManager.CurrentGameState != null)
                        GameStatesManager.CurrentGameState.Update(elapsedTime);
                }
                else
                    timer.Stop();
                
                Refresh();
            }
            
            timer.Stop();
            GameStatesManager.FinalizeManager();
        }

        #endregion

        #region Public Methods

        public void TogglePause()
        {
            Paused = !Paused;
        }

        #endregion
    }
}
