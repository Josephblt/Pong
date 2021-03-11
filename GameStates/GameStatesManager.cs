using System.Collections.Generic;
using System.Linq;

namespace Pong.GameStates
{
    public class GameStatesManager
    {
        #region Attributes and Properties

        public IGameState CurrentGameState { get; private set; }

        #endregion

        #region Private Fields

        private List<IGameState> _gameStates;

        #endregion

        #region Consts

        public const string MAIN_MENU_STATE = "MainMenu";
        public const string IN_GAME_STATE = "InGame";
        public const string END_GAME_STATE = "EndGame";
        public const string CREDITS_STATE = "Credits";

        #endregion

        #region Private Methods

        private void CreateStates()
        {
            _gameStates = new List<IGameState>
            {
                new MainMenuState(),
                new InGameState(),
                new CreditsState(),
                new EndGameState()
            };
        }

        private IGameState GetState(string gameState)
        {
            return _gameStates
                .Where(state => string.Compare(state.Name, gameState) == 0)
                .FirstOrDefault();
        }

        private void InitializeStates()
        {
            foreach (IGameState state in _gameStates)
                state.InitializeState();
        }

        private void FinalizeStates()
        {
            foreach (IGameState state in _gameStates)
                state.FinalizeState();
        }

        #endregion

        #region Public Methods

        public void ChangeToState(string gameState)
        {
            if (CurrentGameState != null)
                CurrentGameState.LeaveState();
            CurrentGameState = GetState(gameState);
            if (CurrentGameState != null)
                CurrentGameState.EnterState();
        }

        public void InitializeManager()
        {
            CreateStates();
            InitializeStates();
            ChangeToState(MAIN_MENU_STATE);
        }

        public void FinalizeManager()
        {
            FinalizeStates();
        }

        #endregion
    }
}
