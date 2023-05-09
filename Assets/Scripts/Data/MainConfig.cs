using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "SO/MainConfig",  fileName = "MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [field: SerializeField] public PlayerData Player {get; private set;}
    }
}