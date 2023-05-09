using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Ball : MonoBehaviour, IPuzzleInteract
    {
        public int MyIndex;
        
        void IPuzzleInteract.Interact(Vector3 swipeDirection)
        {
            Util.Debug.Print($"ball {name} swipe dir {swipeDirection}");
        }
    }
}