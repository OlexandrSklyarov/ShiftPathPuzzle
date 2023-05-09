using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "SO/PlayerData",  fileName = "PlayerConfig")]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public LayerMask InteractLayerMask {get; private set;}        
        [field: SerializeField, Min(1f)] public float RayDistance {get; private set;} = 500f;       
    }
}