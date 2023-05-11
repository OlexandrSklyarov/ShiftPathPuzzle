using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Gameplay.Puzzle
{
    public class Path : MonoBehaviour
    {
        private enum State {READY, WORKING}

        public bool IsBlocked => _isBlocked;
        public bool IsWorking => _state == State.WORKING;
        public ColorType Type => _type;
        public int CellCount => _cells.Length;
        public bool IsCollected => !IsBlocked && AllBallsSomeColor();
        

        [SerializeField] private ColorType _type;

        private Cell[] _cells;
        private Ball[] _balls;
        private State _state;
        private bool _isBlocked;

        private const float ROTATE_DURATION = 0.25f;
       

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
            Refresh();            
        }     


        public void Rotate(Vector3 swipeDirection, int ballIndex)
        {
            var dirType = GetRotateDirection(swipeDirection, ballIndex);  
            StartCoroutine(Rotate((int)dirType));
        }


        private PathRotateDirection GetRotateDirection(Vector3 swipeDirection, int originIndex)
        {
            var forwardIndex = (originIndex == _cells.Length -1) ? 0 : originIndex + 1;
            var originPos = _cells[originIndex].transform.position;
            var forward = (_cells[forwardIndex].transform.position - originPos).normalized;
            var dot = Vector3.Dot(swipeDirection, forward);

            return (dot >= 0) ? PathRotateDirection.FORWARD : PathRotateDirection.BACK;
        }       


        private IEnumerator Rotate(int dir)
        {
            SetState(State.WORKING);
            ShiftBalls(dir);

            yield return new WaitForSeconds(ROTATE_DURATION);
           
            _balls =  (dir > 0) ? 
                _balls.ArrayShiftForwardByOne() :
                _balls.ArrayShiftBackwardByOne();

            SetState(State.READY);
        }

        
        private void ShiftBalls(int dir)
        {
            Array.ForEach(_balls, b => 
            {
                var nextIndex = CollectionExtension.ArrayLoopingIndex(b.MyIndex + dir, _balls.Length);
                b.SetIndex(nextIndex);
            });
            
            MoveBallToNextPosition(ROTATE_DURATION);
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


        public Ball TakeBall(int index) => _balls[index];
      

        public Ball GetRandomBall() => _balls.RandomElement();    


        public void InsertBallToIndex(Ball ball, int index)
        {
            _balls[index] = ball;
            if (ball != null) SetupBall(index);
        }


        private void SetupBall(int index)
        {
            _balls[index].SetIndex(index);
            _balls[index].SetCurrentPathColor(_type);
            _balls[index].transform.position = _cells[index].transform.position;
            _balls[index].transform.SetParent(this.transform);
        }


        public void Refresh()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                SetupBall(i);
            }
        }


        public void Block() => _isBlocked = true;


        public void UnBlock() => _isBlocked = false;
    }


    public enum PathRotateDirection
    {
        FORWARD = 1,
        BACK = -1
    }


    public enum ColorType
    {
        RED,
        BLUE,
        GREEN,
        YELLOW
    }
}