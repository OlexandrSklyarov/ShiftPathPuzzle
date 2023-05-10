using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Cell : MonoBehaviour
    {
        public int MyIndex { get; private set; }

        public void SetIndex(int index)
        {
            MyIndex = index;
            name = $"Cell_{index}";
        }
    }
}