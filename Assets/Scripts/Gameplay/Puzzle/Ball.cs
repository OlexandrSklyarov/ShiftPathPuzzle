using System;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Ball : MonoBehaviour, IPuzzleInteract
    {
        public int MyIndex {get; private set;}
        public int PathIndex {get; private set;}
        
        void IPuzzleInteract.Interact(Vector3 swipeDirection)
        {
            Util.Debug.Print($"ball {name} swipe dir {swipeDirection}");
        }


        public void SetIndex(int index) => MyIndex = index;


        public void SetPath(int path) => PathIndex = path;
    }
}