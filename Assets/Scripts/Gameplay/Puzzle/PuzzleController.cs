using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using System.Linq;

namespace Gameplay.Puzzle
{
    public class PuzzleController : MonoBehaviour, IPuzzleController
    {
        bool IPuzzleController.IsCompleted => AllPathsCollected();        

        private PuzzleData _config;
        private Path[] _paths;
        private SharedZone[] _sharedZones;


        public void Init(PuzzleData config)
        {
            _config = config;

            InitPaths();
            InitSharedZones();
        }


        private void InitSharedZones()
        {
            _sharedZones = GetComponentsInChildren<SharedZone>();
            Array.ForEach(_sharedZones, s => s.Init());
        }


        private void InitPaths()
        {
            _paths = GetComponentsInChildren<Path>();

            Array.ForEach(_paths, p =>
            {
                p.Init();
                p.SetStartupBalls(CreateBalls(p));
            });

            RandomSwapBalls();
        }


        private void RandomSwapBalls()
        {
            var counter = UnityEngine.Random.Range(5, 10);

            while(counter < 10)
            {
                for (int i = 1; i < _paths.Length; i++)
                {
                    var pathA = _paths[i-1];
                    var pathB = _paths[i];

                    var a = pathA.GetRandomBall();
                    var indexA = a.MyIndex;

                    var b = pathB.GetRandomBall();
                    var indexB = b.MyIndex;

                    pathA.InsertBallToIndex(b, indexA);
                    pathB.InsertBallToIndex(a, indexB);
                }

                counter++;
            }
        }


        private Ball[] CreateBalls(Path p)
        {
            var balls = new List<Ball>();
            var prefab = _config.BallPrefabs.First(i => i.Type == p.Type).Prefab;

            for (int i = 0; i < p.CellCount; i++)
            {
                var ball = Instantiate(prefab, p.transform);
                ball.SwipeItemEvent += OnSwipeBallHandler;
                balls.Add(ball);
            }

            return balls.ToArray();
        }


        private void OnSwipeBallHandler(Ball ball, Vector3 swipeDirection)
        {
            var path = _paths.First(p => p.Type == ball.CurrentPathColor);

            if (path.IsBlocked) return;                        
            
            path.Rotate(swipeDirection, ball.MyIndex);
        }


        private bool AllPathsCollected()
        {
            for (int i = 0; i < _paths.Length; i++)
            {
                if (!_paths[i].IsCollected) return false;
            }            

            return true;
        }
    }
}