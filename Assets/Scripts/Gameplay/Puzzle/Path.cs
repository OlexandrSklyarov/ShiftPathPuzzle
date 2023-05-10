using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class Path : MonoBehaviour
    {
        private enum State {READY, WORKING}

        public bool IsBlocked => _isBlocked || _state == State.WORKING;
        public ColorType Type => _type;
        public int CellCount => _cells.Length;
        public bool IsCollected => AllBallsSomeColor();
        

        [SerializeField] private ColorType _type;

        private Cell[] _cells;
        private Ball[] _balls;
        private State _state;
        private bool _isBlocked;

        private const float ROTATE_DURATION = 0.5f;
       

        public void Init()
        {

            _cells = GetComponentsInChildren<Cell>();

            for (int i = 0; i < _cells.Length; i++) _cells[i].SetIndex(i);

            SetState(State.READY);
        }    


        private void SetState(State state) => _state = state;


        public void SetStartupBalls(Ball[] balls) 
        {
            _balls = balls; 

            for (int i = 0; i < _cells.Length; i++)
            {
                _balls[i].SetIndex(i);
                _balls[i].SetCurrentPathColor(_type);
                _balls[i].transform.position = _cells[i].transform.position;
            }
        }     


        public void Rotate(Vector3 swipeDirection, int ballIndex)
        {
            var dirType = GetRotateDirection(swipeDirection, ballIndex);            

            switch(dirType)
            {
                case PathRotateDirection.FORWARD:
                    StartCoroutine(RotateClockwise());
                    break;

                case PathRotateDirection.BACK:
                    StartCoroutine(RotateCounterclockwise());
                    break;
            }
        }


        private PathRotateDirection GetRotateDirection(Vector3 swipeDirection, int originIndex)
        {
            var forwardIndex = (originIndex == _cells.Length -1) ? 0 : originIndex + 1;
            var originPos = _cells[originIndex].transform.position;
            var forward = (_cells[forwardIndex].transform.position - originPos).normalized;
            var dot = Vector3.Dot(swipeDirection, forward);

            return (dot >= 0) ? PathRotateDirection.FORWARD : PathRotateDirection.BACK;
        }


        [ContextMenu("Rotate")]
        private IEnumerator RotateClockwise()
        {      
            SetState(State.WORKING);

            Array.ForEach(_balls, b => b.SetIndex(ChangeIndex(b.MyIndex + 1)));
            MoveBallToNextPosition(ROTATE_DURATION); 

            yield return new WaitForSeconds(ROTATE_DURATION);

            SetState(State.READY);                     
        }


        [ContextMenu("Rotate back")]
        private IEnumerator RotateCounterclockwise()
        {
            SetState(State.WORKING);

            Array.ForEach(_balls, b => b.SetIndex(ChangeIndex(b.MyIndex - 1)));
            MoveBallToNextPosition(ROTATE_DURATION); 

            yield return new WaitForSeconds(ROTATE_DURATION);

            SetState(State.READY);
        }


        private int ChangeIndex(int index)
        {
            if (index < 0) index = _cells.Length -1;
            if (index > _cells.Length -1) index = 0;
            return index;
        }


        private void MoveBallToNextPosition(float duration)
        {
            for (int i = 0; i < _balls.Length; i++)
            {
                _balls[i].transform
                    .DOMove(_cells[_balls[i].MyIndex].transform.position, duration)
                    .SetEase(Ease.InOutSine);
            }
        }


        private bool AllBallsSomeColor()
        {
            var checkType = _balls[0].MyBaseColorType;
            return _balls.All(b => b.MyBaseColorType == checkType);
        }
    }


    public enum PathRotateDirection
    {
        FORWARD,
        BACK
    }


    public enum ColorType
    {
        RED,
        BLUE,
        GREEN,
        YELLOW
    }
}