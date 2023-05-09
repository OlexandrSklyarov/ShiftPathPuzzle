using System;
using System.Collections.Generic;
using System.Linq;
using Common.InputNew;
using Data;
using Gameplay.Player.FSM;
using Gameplay.Player.FSM.States;
using Gameplay.Puzzle;

namespace Gameplay.Player
{
    public class PlayerController : IPlayer, IPlayerContextSwitcher
    {
        TouchInputManager IPlayer.Input => _input;
        IPuzzleController IPlayer.Puzzle => _puzzle;
        PlayerData IPlayer.Config => _config;

        private readonly PlayerData _config;
        private readonly TouchInputManager _input;
        private readonly IPuzzleController _puzzle;
        private readonly List<BasePlayerState> _allStates;
        private BasePlayerState _currentState;
        private bool _isActive;

        public event Action CompletedEvent;
        

        public PlayerController(PlayerData config, TouchInputManager input, IPuzzleController puzzle)
        {       
            _config = config;
            _input = input;
            _puzzle = puzzle;
            
            _allStates =  new List<BasePlayerState>()
            {
                new PrepareState(this, this),
                new ControlState(this, this),
                new CompletedState(this, this)
            };

            _currentState = _allStates[0];
        }


        public void Enable()
        {
            if (_isActive) return;

            _currentState?.OnStart(); 
            _isActive = true;
        }


        public void Disable()
        {
            if (!_isActive) return;

            _currentState?.OnStop(); 
            _isActive = false;
        }


        public void SwitchState<T>() where T : BasePlayerState
        {
            var state = _allStates.FirstOrDefault(s => s is T);

            _currentState?.OnStop();
            _currentState = state;
            _currentState?.OnStart();
        }


        public void OnUpdate()
        {
            if (!_isActive) return;

            _currentState?.OnUpdate();
        }


        public void OnFixedUpdate()
        {
            if (!_isActive) return;
            
            _currentState?.OnFixedUpdate();
        }
        
        
        public void OnLateUpdate()
        {
            if (!_isActive) return;

            _currentState?.OnLateUpdate();
        }
        

        void IPlayer.Complete() => CompletedEvent?.Invoke();
    }
}