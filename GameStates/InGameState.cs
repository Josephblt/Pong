using Pong.Controllers;
using Pong.Entities;
using System.Drawing;

namespace Pong.GameStates
{
    public class InGameState : IGameState
    {
        #region Attributes and Properties

        public string Name
        {
            get { return GameStatesManager.IN_GAME_STATE; }
        }

        public float PlayerOneTargetHeight { get; set; }
        public float PlayerTwoTargetHeight { get; set; }

        public bool PlayerOneServing { get; private set; }
        public bool PlayerTwoServing { get; private set; }

        #endregion

        #region Consts

        private const string PLAYER_ONE_WINS = "Player One Wins";
        private const string PLAYER_TWO_WINS = "Player Two Wins";

        #endregion

        #region Private Fields

        private InGameAIController _aiController;
        private InGamePlayerController _playerController;

        private Ball _ball;
        private int _pauseBlink;
        private Racquet _playerOne;
        private int _playerOneScore;
        private Racquet _playerTwo;
        private int _playerTwoScore;
        private bool _postponned;
        private int _serviceMessageBlink;
        private float _serviceSpeed;

        #endregion

        #region Private Methods

        private void RenderInstructions(Graphics graphics)
        {
            if (GameWindow.GetGameWindow().Paused) 
                return;

            if (PlayerOneServing || PlayerTwoServing)
            {
                var smallFont = new Font(FontFamily.GenericSansSerif, 15);

                var keyString = "Press Enter at any time to pause.";
                var keySize = graphics.MeasureString(keyString, smallFont);
                var keyPosition = new PointF(400 - (keySize.Width / 2f), 550 - keySize.Height);

                var keyBrush = new SolidBrush(Color.Blue);
                graphics.DrawString(keyString, smallFont, keyBrush, keyPosition);

                if (_serviceMessageBlink < 250)
                {
                    var clickString = "Click to serve...";
                    var clickBrush = new SolidBrush(Color.Yellow);
                    var clickSize = graphics.MeasureString(clickString, smallFont);
                    var clickPosition = new PointF(400 - (clickSize.Width / 2f), 550 - (clickSize.Height) - 10f - keySize.Height);
                    graphics.DrawString(clickString, smallFont, clickBrush, clickPosition);
                }
                else if (_serviceMessageBlink >= 500)
                    _serviceMessageBlink = 0;

                _serviceMessageBlink++;
            }
            else
                _serviceMessageBlink = 0;
        }

        private void RenderPause(Graphics graphics)
        {
            if (GameWindow.GetGameWindow().Paused)
            {
                var bigFont = new Font(FontFamily.GenericSansSerif, 50);
                var smallFont = new Font(FontFamily.GenericSansSerif, 15);
                var brush = new SolidBrush(Color.Yellow);

                if (_pauseBlink < 250)
                {
                    var pauseString = "Paused";
                    
                    var pauseSize = graphics.MeasureString(pauseString, bigFont);
                    var pausePosition = new PointF(400 - (pauseSize.Width / 2f), 300 - (pauseSize.Height / 2f));
                    graphics.DrawString(pauseString, bigFont, brush, pausePosition);
                }
                else if (_pauseBlink >= 500)
                    _pauseBlink = 0;

                var keyString = "Press Enter to continue.";
                var keySize = graphics.MeasureString(keyString, smallFont);
                var keyPosition = new PointF(400 - (keySize.Width / 2f), 550 - (keySize.Height));
                graphics.DrawString(keyString, smallFont, brush, keyPosition);
                
                _pauseBlink++;
            }
            else
            {
                _pauseBlink = 0;                
            }
        }

        private void RenderScore(Graphics graphics)
        {
            var font = new Font(FontFamily.GenericSansSerif, 50);

            var scoreBrush = new SolidBrush(Color.Blue);
            var positionOne = new PointF(100.0f, 50.0f);
            var positionTwo = new PointF(650.0f, 50.0f);

            graphics.DrawString(_playerOneScore.ToString(), font, scoreBrush, positionOne);
            graphics.DrawString(_playerTwoScore.ToString(), font, scoreBrush, positionTwo);
        }

        private void Score()
        {
            if (_ball.Location.X > 800f)
            {
                _playerOneScore++;
                PlayerOneServing = true;
            }
            if (_ball.Location.X < 0f)
            {
                _playerTwoScore++;
                PlayerTwoServing = true;
            }

            if ((_playerOneScore == 5) && (_playerTwoScore == 5))
                _postponned = true;

            if (_postponned)
            {
                if (_playerOneScore == (_playerTwoScore + 2))
                {
                    GameWindow.GetGameWindow().GameWinner = PLAYER_ONE_WINS;
                    GameWindow.GetGameWindow().GameStatesManager.ChangeToState(GameStatesManager.END_GAME_STATE);
                }

                if (_playerTwoScore == (_playerOneScore + 2))
                {
                    GameWindow.GetGameWindow().GameWinner = PLAYER_TWO_WINS;
                    GameWindow.GetGameWindow().GameStatesManager.ChangeToState(GameStatesManager.END_GAME_STATE);
                }
            }
            else
            {
                if (_playerOneScore == 5)
                {
                    GameWindow.GetGameWindow().GameWinner = PLAYER_ONE_WINS;
                    GameWindow.GetGameWindow().GameStatesManager.ChangeToState(GameStatesManager.END_GAME_STATE);
                }
                if (_playerTwoScore == 5)
                {
                    GameWindow.GetGameWindow().GameWinner = PLAYER_TWO_WINS;
                    GameWindow.GetGameWindow().GameStatesManager.ChangeToState(GameStatesManager.END_GAME_STATE);
                }
            }
        }

        #endregion

        #region Public Methods

        public void EnterState()
        {
            _serviceSpeed = 300f;
            _playerOneScore = 0;
            _playerTwoScore = 0;

            _ball = new Ball(25f, new PointF(400f, 300f), 30f);
            _playerOne = new Racquet(500f, new PointF(50f, 400f), new SizeF(20f, 100f));
            _playerTwo = new Racquet(500f, new PointF(750f, 400f), new SizeF(20f, 100f));

            PlayerOneServing = true;
            PlayerTwoServing = false;

            _aiController.InitializeController(this);
            _playerController.InitializeController(this);
        }

        public void FinalizeState()
        {
            _playerController = null;
        }

        public void InitializeState()
        {
            _aiController = new InGameAIController();
            _playerController = new InGamePlayerController();
        }

        public void LeaveState()
        {
            _playerOne = null;
            _playerTwo = null;
            _playerController.FinalizeController();
            
            if (GameWindow.GetGameWindow().Paused)
                GameWindow.GetGameWindow().TogglePause();
        }

        public void Render(Graphics graphics)
        {
            RenderScore(graphics);
            
            _playerOne.Render(graphics);
            _playerTwo.Render(graphics);

            RenderInstructions(graphics);

            _ball.Render(graphics);

            RenderPause(graphics);
        }

        public void Serve()
        {
            if (GameWindow.GetGameWindow().Paused)
                return;

            if (PlayerOneServing)
                _ball.Serve(_playerOne, _serviceSpeed);

            if (PlayerTwoServing)
                _ball.Serve(_playerTwo, _serviceSpeed);

            PlayerOneServing = false;
            PlayerTwoServing = false;
        }

        public void Update(float deltaTime)
        {
            if (GameWindow.GetGameWindow().Paused)
                return;

            if (PlayerOneServing)
                _ball.PrepareToServe(_playerOne);
            else if (PlayerTwoServing)
                _ball.PrepareToServe(_playerTwo);

            _ball.Update(_playerOne, _playerTwo, deltaTime);
            _playerOne.Update(PlayerOneTargetHeight, deltaTime);
            _playerTwo.Update(PlayerTwoTargetHeight, deltaTime);
            _aiController.Update(_ball, _playerOne, _playerTwo);

            Score();
        }

        #endregion
    }
}
