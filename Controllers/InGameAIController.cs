using Pong.Entities;
using Pong.GameStates;
using Pong.Utility;
using System;

namespace Pong.Controllers
{
    public class InGameAIController
    {
        #region Private Fields

        private InGameState _inGameState;
        private Random _random;
        private bool _decidingService;

        #endregion

        #region Public Methods

        public void FinalizeController()
        {
        }

        public void InitializeController(InGameState gameState)
        {
            _inGameState = gameState;
            _random = new Random();
        }

        #endregion

        #region Public Methods

        public void Update(Ball ball, Racquet left, Racquet right)
        {
            var ballVerGap = 600f - ball.Size.Height;
            var racquetHorGap = right.Location.X - left.Location.X - left.Size.Width - ball.Location.X;
            var racquetVerGap = 600f - right.Size.Height;

            var decider = _random.Next(0, 100);
            if (_inGameState.PlayerTwoServing)
            {
                if (MathFunctions.Equals(_inGameState.PlayerTwoTargetHeight, right.Location.Y))
                    _decidingService = true;
                
                if (_decidingService)
                {
                    _inGameState.PlayerTwoTargetHeight = (right.Size.Height / 2f) + racquetVerGap * (decider / 100f);
                    _decidingService = false;
                }
            }
            else
            {
                _decidingService = true;

                if (decider > 4)
                    return;

                if (MathFunctions.EqualsOrLess(ball.Speed.X , 0))
                    _inGameState.PlayerTwoTargetHeight = 300;
                else
                {
                    var yDistance = ball.Location.Y + ball.Speed.Y * (racquetHorGap / ball.Speed.X);
                    var bounces = Math.Truncate(yDistance / ballVerGap);
                    var even = (bounces % 2) == 0;
                    var reminder = yDistance % ballVerGap;

                    if (even)
                        _inGameState.PlayerTwoTargetHeight = reminder;
                    else
                        _inGameState.PlayerTwoTargetHeight = ballVerGap - reminder;
                }
            }
        }

        #endregion
    }
}
