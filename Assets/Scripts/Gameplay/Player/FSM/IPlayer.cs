using Common.InputNew;
using Gameplay.Puzzle;
using Data;

namespace Gameplay.Player.FSM
{
    public interface IPlayer
    {
        PlayerData Config {get;}
        TouchInputManager Input {get;}
        IPuzzleController Puzzle {get;}
        
        void Complete();
    }
}