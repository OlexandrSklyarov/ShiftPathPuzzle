
using System;

namespace Gameplay.Puzzle
{
    public interface IPuzzleController
    {
        bool IsCompleted { get; }
        event Action CompletedEvent;
    }
}