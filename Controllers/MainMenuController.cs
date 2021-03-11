using Pong.GameStates;
using System.Windows.Forms;

namespace Pong.Controllers
{
    public class MainMenuController
    {
        #region Private Fields

        private MainMenuState _mainMenuState;

        #endregion

        #region Public Methods

        public void FinalizeController()
        {
            GameWindow.GetGameWindow().MouseMove -= MouseClick;
            GameWindow.GetGameWindow().MouseClick -= MouseClick;
        }

        public void InitializeController(MainMenuState mainMenuState)
        {
            _mainMenuState = mainMenuState;
            GameWindow.GetGameWindow().MouseMove += MouseMove;
            GameWindow.GetGameWindow().MouseClick += MouseClick;
        }

        #endregion

        #region SignedEvents

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (_mainMenuState.StartGameRectangle.Contains(e.Location))
                _mainMenuState.SelectStartGame();
            else if (_mainMenuState.CreditsRectangle.Contains(e.Location))
                _mainMenuState.SelectCreditsGame();
            else
                _mainMenuState.Deselected();

        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (_mainMenuState.StartGameRectangle.Contains(e.Location))
                GameWindow.GetGameWindow().GameStatesManager.ChangeToState(GameStatesManager.IN_GAME_STATE);
            else if (_mainMenuState.CreditsRectangle.Contains(e.Location))
                GameWindow.GetGameWindow().GameStatesManager.ChangeToState(GameStatesManager.CREDITS_STATE);
        }

        #endregion
    }
}
