using UnityEngine;

namespace Gameplay.Puzzle
{
    public interface IPuzzleInteract
    {
        void Interact(Vector3 swipeDirection);
    }
}