using System;
using Gameplay.Puzzle;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "SO/PuzzleData", fileName = "PuzzleConfig")]
    public class PuzzleData : ScriptableObject
    {
        [field: SerializeField] public BallPrefabItem[] BallPrefabs { get; private set; }
    }


    [Serializable]
    public class BallPrefabItem
    {
        [field: SerializeField] public ColorType Type { get; private set; }
        [field: SerializeField] public Ball Prefab { get; private set; }        
    }
}