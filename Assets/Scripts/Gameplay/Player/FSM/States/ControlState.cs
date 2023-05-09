using UnityEngine;
using Common.InputNew;
using Gameplay.Puzzle;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Gameplay.Player.FSM.States
{
    public class ControlState : BasePlayerState
    {
        private IPuzzleInteract _item;
        private Camera _camera;

        public ControlState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {          
            _camera = Camera.main; 
        }


        public override void OnStart()
        {
            _agent.Input.InputTouchEvent += OnInputHandler;
        }


        public override void OnStop()
        {
            _agent.Input.InputTouchEvent -= OnInputHandler;
        }


        private void OnInputHandler(TouchInputManager.InputData data)
        {
            switch(data.Phase)
            {
                case TouchPhase.Began:
                    
                    TryItemSelect(data.StartPosition, out _item);                   

                break;                

                case TouchPhase.Ended:

                    if (_item != null)
                    {
                        var dir = data.EndPosition - data.StartPosition;
                        _item.Interact(dir.normalized);
                        _item = null;
                    }

                    if (_agent.Puzzle.IsCompleted) SwitchToComplete();

                break;
            }
        }
        

        private bool TryItemSelect(Vector3 pointer, out IPuzzleInteract item)
        {
            return Util.RaycastUtil.TryGetComponent(
                GetRay(pointer), out item, _agent.Config.RayDistance, false, _agent.Config.InteractLayerMask);            
        }


        private Ray GetRay(Vector3 point) => _camera.ScreenPointToRay(point);


        private void SwitchToComplete() => _context.SwitchState<CompletedState>();
    }
}