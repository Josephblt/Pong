using System.Drawing;

namespace Pong.GameStates
{
    public interface IGameState
    {
        #region Attributes and Properties

        string Name { get; }

        #endregion

        #region Interface Methods

        void EnterState();

        void FinalizeState();

        void InitializeState();

        void LeaveState();

        void Render(Graphics graphics);

        void Update(float deltaTime);

        #endregion
    }
}
