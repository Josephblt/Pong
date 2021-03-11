using Pong.GameStates;
using System.Windows.Forms;

namespace Pong.Controllers
{
    public class InGamePlayerController
    {
        #region Private Fields

        private InGameState _inGameState;

        #endregion

        #region Public Methods

        public void FinalizeController()
        {
            GameWindow.GetGameWindow().MouseClick -= MouseClick;
            GameWindow.GetGameWindow().MouseMove -= MouseMove;
            GameWindow.GetGameWindow().KeyDown -= KeyDown;
        }

        public void InitializeController(InGameState gameState)
        {
            _inGameState = gameState;
            GameWindow.GetGameWindow().MouseClick += MouseClick;
            GameWindow.GetGameWindow().MouseMove += MouseMove;
            GameWindow.GetGameWindow().KeyDown += KeyDown;
        }

        #endregion

        #region Signed Events

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GameWindow.GetGameWindow().TogglePause();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            _inGameState.Serve();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            _inGameState.PlayerOneTargetHeight = e.Y;
        }

        #endregion
    }
}
