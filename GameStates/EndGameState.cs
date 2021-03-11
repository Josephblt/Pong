using Pong.Controllers;
using System.Drawing;

namespace Pong.GameStates
{
    public class EndGameState : IGameState
    {
        #region Attributes and Properties

        public string Name
        {
            get { return GameStatesManager.END_GAME_STATE; }
        }

        #endregion

        #region Private Fields

        private EndGameController _controller;

        #endregion

        #region Public Methods

        public void EnterState()
        {
            _controller.InitializeController();
        }

        public void FinalizeState()
        {
            _controller = null;
        }

        public void InitializeState()
        {
            _controller = new EndGameController();
        }

        public void LeaveState()
        {
            _controller.FinalizeController();
        }

        public void Render(Graphics graphics)
        {
            var font = new Font(FontFamily.GenericSansSerif, 25);
            var brush1 = new SolidBrush(Color.Yellow);
            var brush2 = new SolidBrush(Color.Blue);

            var string1 = GameWindow.GetGameWindow().GameWinner;
            var string2 = "Click to continue...";

            var halfWidth = GameWindow.GetGameWindow().Width / 2f;            
            var halfHeight = GameWindow.GetGameWindow().Height / 2f;

            var size1 = graphics.MeasureString(string1, font);
            var size2 = graphics.MeasureString(string2, font);

            var position1 = new PointF(
                halfWidth - (size1.Width / 2f),
                halfHeight - 50f - size1.Height
                );
            var position2 = new PointF(
                halfWidth - (size2.Width / 2f),
                halfHeight + 50f
                );

            graphics.DrawString(string1, font, brush1, position1);
            graphics.DrawString(string2, font, brush2, position2);

        }

        public void Update(float deltaTime)
        {
        }

        #endregion
    }
}
