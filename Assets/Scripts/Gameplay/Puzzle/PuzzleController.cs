using System;
using UnityEngine;

namespace Gameplay.Puzzle
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        bool IPuzzleController.IsCompleted => false;

        private Path[] _paths;
        private SharedZone[] _sharedZones;


        public void Init()
        {
            _paths = GetComponentsInChildren<Path>();
            _sharedZones = GetComponentsInChildren<SharedZone>();

            var pathIndex = 0;
            Array.ForEach(_paths, p => p.Init(pathIndex++));
            Array.ForEach(_sharedZones, s => s.Init());
        }
    }
}