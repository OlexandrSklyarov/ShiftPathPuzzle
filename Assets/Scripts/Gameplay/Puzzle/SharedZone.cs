using System;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class SharedZone : MonoBehaviour, IPuzzleInteract
    {
        public event Action InteractEvent;

        public void Init()
        {
            
        }


        void IPuzzleInteract.Interact(Vector3 swipeDirection)
        {
            Util.Debug.Print($"{nameof(GetType)} switch...");
            InteractEvent?.Invoke();
        }
    }
}