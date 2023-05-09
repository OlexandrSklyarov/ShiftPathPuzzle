using System;
using Common.InputNew;
using Gameplay.Player;
using Gameplay.Puzzle;
using UnityEngine;

namespace Gameplay
{
    public class GameProcess
    {
        private readonly TouchInputManager _input;
        private readonly IPuzzleController _puzzle;
        private readonly PlayerController _playerController;

        public event Action GameCompletedEvent;
        

        public GameProcess(Data.MainConfig config, IPuzzleController puzzle)
        {            
            _input = new TouchInputManager();  
            _puzzle = puzzle;

            _playerController = new PlayerController(config.Player, _input, _puzzle);
            _playerController.CompletedEvent += OnCompletedHandler;
        }      
       

        public void OnUpdate()
        {
            _playerController?.OnUpdate();
            _input?.OnUpdate();
        }
        
        
        public void OnFixedUpdate()
        {
            _playerController?.OnFixedUpdate();
        }
        
        
        public void OnLateUpdate()
        {
            _playerController?.OnLateUpdate();
        }
        

        public void StartProcess()
        {
            _input?.OnEnable();
            _playerController?.Enable();
        }
        
        
        public void StopProcess()
        {            
            _input?.OnDisable();
            _playerController?.Disable();
        }


        public void Clear()
        {
        }
        

        private void OnCompletedHandler()
        {
            Util.Debug.PrintColor($"Game end", Color.green);
            GameCompletedEvent?.Invoke();
        }
    }
}
