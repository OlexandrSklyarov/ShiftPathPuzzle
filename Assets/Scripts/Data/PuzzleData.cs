using System;
using DG.Tweening;
using Gameplay.Puzzle;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "SO/PuzzleData", fileName = "PuzzleConfig")]
    public class PuzzleData : ScriptableObject
    {
        [field: SerializeField] public BallPrefabItem[] BallPrefabs { get; private set; }
        [field: SerializeField] public PathData Path{ get; private set; }
        [field: SerializeField] public SharedZoneData SharedZone{ get; private set; }
    }


    [Serializable]
    public class BallPrefabItem
    {
        [field: SerializeField] public ColorType Type { get; private set; }
        [field: SerializeField] public Ball Prefab { get; private set; }        
    }


    [Serializable]
    public class PathData
    {
        [field: SerializeField, Min(0.1f)] public float ShiftDuration { get; private set; } = 0.2f;
        [field: SerializeField] public Ease ShiftEase { get; private set; } = Ease.InElastic; 
    }


    [Serializable]
    public class SharedZoneData
    {
        [field: SerializeField, Min(0.1f)] public float ShiftDuration { get; private set; } = 0.2f;
        [field: SerializeField] public Ease ShiftEase { get; private set; } = Ease.InElastic; 
    }
}