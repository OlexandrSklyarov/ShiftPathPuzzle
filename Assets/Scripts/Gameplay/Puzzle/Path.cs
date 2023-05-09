using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Path : MonoBehaviour
    {
        private Cell[] _cells;
        private Ball[] _balls;

        public Vector3 Position { get; private set; }

        public void Init()
        {
            _balls = GetComponentsInChildren<Ball>();
            _cells = new Cell[_balls.Length];

            for (int i = 0; i < _balls.Length; i++)
            {
                var go = new GameObject("Cell");
                go.transform.SetParent(transform);
                go.transform.position = _balls[i].transform.position;
                _cells[i] = go.AddComponent<Cell>();

                _balls[i].MyIndex = i;
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
            Array.ForEach(_balls, b => 
            {
                b.MyIndex++;

                if (b.MyIndex > _cells.Length - 1) 
                    b.MyIndex = 0;
            });

            MoveBallToNextPosition();            
        }


        [ContextMenu("Rotate back")]
        private void RotateCounterclockwise()
        {
            Array.ForEach(_balls, b => 
            {
                b.MyIndex--;

                if (b.MyIndex < 0) 
                    b.MyIndex = _cells.Length -1;
            });

            MoveBallToNextPosition(); 
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