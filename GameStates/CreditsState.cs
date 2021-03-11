using Pong.Controllers;
using System.Drawing;

namespace Pong.GameStates
{
    public class CreditsState : IGameState
    {
        #region Attributes and Properties

        public string Name
        {
            get { return GameStatesManager.CREDITS_STATE; }
        }

        #endregion

        #region Private Fields

        private CreditsController _controller;

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
            _controller = new CreditsController();
        }

        public void LeaveState()
        {
            _controller.FinalizeController();
        }

        public void Render(Graphics graphics)
        {
            var fontBig = new Font(FontFamily.GenericSansSerif, 35);
            var fontMedium = new Font(FontFamily.GenericSansSerif, 20);
            var fontSmall = new Font(FontFamily.GenericSansSerif, 15);

            var blueBrush = new SolidBrush(Color.Blue);
            var yellowBrush = new SolidBrush(Color.Yellow);
            var redBrush = new SolidBrush(Color.Red);

            var string1 = "All Mighty Creator";
            var string2 = "Wagner Scholl Lemos";
            var string3 = "(A.K.A. Squaschownator)";
            var string4 = "Click to go back...";

            var halfWidth = GameWindow.GetGameWindow().Width / 2f;
            var halfHeight = GameWindow.GetGameWindow().Height/ 2f;
            
            var size1 = graphics.MeasureString(string1, fontMedium);
            var size2 = graphics.MeasureString(string2, fontBig);
            var size3 = graphics.MeasureString(string3, fontMedium);
            var size4 = graphics.MeasureString(string4, fontSmall);

            var position1 = new PointF(
                halfWidth - (size1.Width / 2f),
                halfHeight - (size2.Height / 2f) - 50f - size1.Height
                );
            var position2 = new PointF(
                halfWidth - (size2.Width / 2f),
                halfHeight - (size2.Height/ 2f)
                );
            var position3 = new PointF(
                halfWidth - (size3.Width / 2f),
                halfHeight + (size2.Height / 2f) + 50f
                );
            var position4 = new PointF(
                halfWidth - (size4.Width / 2f),
                GameWindow.GetGameWindow().Height - 50f - (size4.Height)
                );

            graphics.DrawString(string1, fontMedium, blueBrush, position1);
            graphics.DrawString(string2, fontBig, yellowBrush, position2);
            graphics.DrawString(string3, fontMedium, yellowBrush, position3);
            graphics.DrawString(string4, fontSmall, redBrush, position4);
        }

        public void Update(float deltaTime)
        {
        }

        #endregion
    }
}
