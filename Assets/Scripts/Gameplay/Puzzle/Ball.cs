using System;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Ball : MonoBehaviour, IPuzzleInteract
    {
        public int MyIndex {get; private set;}
        public ColorType MyBaseColorType => _type;
        public ColorType CurrentPathColor {get; private set;}

        [SerializeField] private ColorType _type;

        public event Action<Ball, Vector3> SwipeItemEvent;

        
        void IPuzzleInteract.Interact(Vector3 swipeDirection)
        {
            SwipeItemEvent?.Invoke(this, swipeDirection);
        }


        public void SetIndex(int index) 
        {
            MyIndex = index;
            name = $"Ball_{index}";
        }     


        public void SetCurrentPathColor(ColorType colorType) => CurrentPathColor = colorType;        
    }
}