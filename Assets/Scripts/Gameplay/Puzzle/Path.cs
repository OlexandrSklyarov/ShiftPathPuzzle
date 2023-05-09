using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Path : MonoBehaviour
    {
        private Cell[] _cells;
        private Ball[] _balls;
        private int _pathIndex;

        public Vector3 Position { get; private set; }

        public void Init(int pathIndex)
        {
            _pathIndex = pathIndex;

            _balls = GetComponentsInChildren<Ball>();
            _cells = new Cell[_balls.Length];

            for (int i = 0; i < _balls.Length; i++)
            {
                var go = new GameObject("Cell");
                go.transform.SetParent(transform);
                go.transform.position = _balls[i].transform.position;
                _cells[i] = go.AddComponent<Cell>();

                _balls[i].SetIndex(i);
                _balls[i].SetPath(pathIndex);

            }
        }    


        public void Rotate(PathRotateDirection dirType)
        {
            switch(dirType)
            {
                case PathRotateDirection.FORWARD:
                    RotateClockwise();
                    break;

                case PathRotateDirection.BACK:
                    RotateCounterclockwise();
                    break;
            }
        }


        [ContextMenu("Rotate")]
        private void RotateClockwise()
        {            
            Array.ForEach(_balls, b => b.SetIndex(ChangeIndex(b.MyIndex + 1)));
            MoveBallToNextPosition();            
        }


        [ContextMenu("Rotate back")]
        private void RotateCounterclockwise()
        {
            Array.ForEach(_balls, b => b.SetIndex(ChangeIndex(b.MyIndex - 1)));
            MoveBallToNextPosition(); 
        }


        private int ChangeIndex(int index)
        {
            if (index < 0) index = _cells.Length -1;
            if (index > _cells.Length -1) index = 0;
            return index;
        }


        private void MoveBallToNextPosition()
        {
            for (int i = 0; i < _balls.Length; i++)
            {
                _balls[i].transform.DOMove(_cells[_balls[i].MyIndex].transform.position, 0.5f).SetEase(Ease.InOutSine);
                Debug.DrawLine(_balls[i].transform.position, _cells[_balls[i].MyIndex].transform.position, Color.green, 1f);
            }
        }
    }


    public enum PathRotateDirection
    {
        FORWARD,
        BACK
    }
}