using System;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class SharedZone : MonoBehaviour, IPuzzleInteract
    {     
        [Serializable]
        private class SlotData
        {
            public Cell Cell;
            public Path Path;
            public Transform BallHolder;
            public Ball Ball;
        }

        [SerializeField] private SlotData _slot_A;
        [SerializeField] private SlotData _slot_B;

        private ShiftState _curState;
        private float _pointOffsetDistance;
        private bool _isBlocked;
        private SharedZoneData _config;

        public event Action<SharedZone, Vector3> TryShiftEvent;
        public event Action ShiftCompletedEvent;


        public void Init(Data.SharedZoneData config)
        {
            _config = config;
            
            SetState(ShiftState.CENTER);

            var a = _slot_A.Cell.transform.position;
            var b = _slot_B.Cell.transform.position;

            _pointOffsetDistance = Vector3.Distance(a, b);

            CreatePointHolder(_slot_A);
            CreatePointHolder(_slot_B);
        }


        private void SetState(ShiftState state) 
        {
            _curState = state;
        }


        private void CreatePointHolder(SlotData data)
        {
            var point = new GameObject("BallHolder").transform;
            point.position = data.Cell.transform.position;
            point.SetParent(transform);

            data.BallHolder = point;
        }


        void IPuzzleInteract.Interact(Vector3 swipeDirection)
        {
            if (_isBlocked) return;

            TryShiftEvent?.Invoke(this, swipeDirection);
        }


        public void TryShift(Vector3 shiftDirection)
        {
            var shiftDir = GetShiftDirection(shiftDirection);

            if (!IsCanShift(shiftDir)) return;            

            Shift(shiftDir);
        }


        private void Shift(ShiftDirection dir)
        {
            _isBlocked = true;     

            AttachBalls();
            
            var pos = transform.position + transform.right * (int)dir * _pointOffsetDistance;

            transform
                .DOLocalMove(pos, _config.ShiftDuration)
                .SetEase(_config.ShiftEase)
                .OnComplete(() => 
                {
                    _isBlocked = false;
                    OnShiftCompleted(dir);
                    ShiftCompletedEvent?.Invoke();
                });
        }


        private void AttachBalls()
        {
            switch(_curState)
            {
                case ShiftState.LEFT:

                    var a = _slot_A.Path.TakeBall(_slot_A.Cell.MyIndex);
                    GrabBall(_slot_B, a);

                    break;

                case ShiftState.CENTER:

                    var aa = _slot_A.Path.TakeBall(_slot_A.Cell.MyIndex);
                    GrabBall(_slot_A, aa);

                    var bb = _slot_B.Path.TakeBall(_slot_B.Cell.MyIndex);
                    GrabBall(_slot_B, bb);

                    break;

                case ShiftState.RIGHT:

                    var b = _slot_B.Path.TakeBall(_slot_B.Cell.MyIndex);
                    GrabBall(_slot_A, b);

                    break;
            }
        }


        private void GrabBall(SlotData data, Ball ball)
        {
            ball.transform.position = data.BallHolder.position;
            ball.transform.SetParent(data.BallHolder);
            data.Ball = ball;
        }


        private void OnShiftCompleted(ShiftDirection dir)
        {
            switch(dir)
            {
                case ShiftDirection.LEFT:    

                    SetState((_curState == ShiftState.RIGHT) ? ShiftState.CENTER : ShiftState.LEFT);

                    break;
                
                case ShiftDirection.RIGHT:

                    SetState((_curState == ShiftState.LEFT) ? ShiftState.CENTER : ShiftState.RIGHT);

                    break;
            } 

            switch(_curState)
            {
                case ShiftState.LEFT:

                    _slot_A.Path.InsertBallToIndex(_slot_B.Ball, _slot_A.Cell.MyIndex);  
                    _slot_A.Cell.UnBlock();
                    _slot_B.Cell.Block();

                    break;

                case ShiftState.CENTER:

                    _slot_A.Path.InsertBallToIndex(_slot_A.Ball, _slot_A.Cell.MyIndex);   
                    _slot_B.Path.InsertBallToIndex( _slot_B.Ball, _slot_B.Cell.MyIndex);

                    _slot_A.Cell.UnBlock();
                    _slot_B.Cell.UnBlock();

                    break;

                case ShiftState.RIGHT:

                    _slot_B.Path.InsertBallToIndex(_slot_A.Ball, _slot_B.Cell.MyIndex);
                    _slot_B.Cell.UnBlock();
                    _slot_A.Cell.Block();

                    break;
            }
        }

        
        private bool IsCanShift(ShiftDirection shiftDir)
        {
            if (_curState == ShiftState.LEFT && shiftDir == ShiftDirection.LEFT || 
                _curState == ShiftState.RIGHT && shiftDir == ShiftDirection.RIGHT) 
                return false;

            return true;
        }


        private ShiftDirection GetShiftDirection(Vector3 shiftDir)
        {
            var toRight = (_slot_B.BallHolder.position - _slot_A.BallHolder.position).normalized;
            var dot = Vector3.Dot(shiftDir, toRight);

            return (dot >= 0) ? ShiftDirection.RIGHT : ShiftDirection.LEFT;
        }
    }


    public enum ShiftState
    {
        CENTER,
        LEFT,
        RIGHT
    }


    public enum ShiftDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
}