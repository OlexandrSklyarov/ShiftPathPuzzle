using Data;
using Gameplay.Puzzle;
using UnityEngine;

namespace Gameplay
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private PuzzleController _puzzleController;
        [SerializeField] private MainConfig config;

        private GameProcess _gameProcess;
        private bool _isRunning;       
       

        private void Start()
        {      
            _puzzleController.Init(config.Puzzle);

            _gameProcess = new GameProcess(config, _puzzleController);  

            _gameProcess.GameCompletedEvent += OnGameCompleted;
            StartGame();
        }


        private void OnValidate() 
        {
            _puzzleController = FindObjectOfType<PuzzleController>();
        }

        
        private void StartGame()
        {
            _gameProcess.StartProcess();
            _isRunning = true;
        }   
                

        private void OnGameCompleted()
        {
            _gameProcess.GameCompletedEvent -= OnGameCompleted;
            _gameProcess.StopProcess();
            _isRunning = false;
        }

        
        private void Update()
        {
            if (!_isRunning) return;
            _gameProcess?.OnUpdate();
        }
        
        
        private void FixedUpdate()
        {
            if (!_isRunning) return;
            _gameProcess?.OnFixedUpdate();
        }
        
        
        private void LateUpdate()
        {
            if (!_isRunning) return;
            _gameProcess?.OnLateUpdate();
        }
        

        private void OnDestroy()
        {
            _gameProcess?.Clear();
        }
    }
}
