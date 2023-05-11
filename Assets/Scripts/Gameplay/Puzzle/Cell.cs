using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Cell : MonoBehaviour
    {
        public bool IsBlocked => _isBlocked;
        private bool _isBlocked;

        public int MyIndex { get; private set; }

        public void SetIndex(int index)
        {
            MyIndex = index;
            name = $"Cell_{index}";
        }


        public void Block() => _isBlocked = true;


        public void UnBlock() => _isBlocked = false;
    }
}