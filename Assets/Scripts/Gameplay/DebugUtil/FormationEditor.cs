using System.Diagnostics;
using UnityEngine;

namespace Gameplay.DebugUtil
{
    public class FormationEditor : MonoBehaviour
    {
        [Conditional("UNITY_EDITOR")]
        [ContextMenu("Circle Formation")]
        private void SetCircleFormation()
        {
            var radius = 3f;
            var center = transform.position;
            int count = transform.childCount;

            for (int i = 0; i < count; i++)
            {
                var item = transform.GetChild(i);
                item.position = Util.MathUtility.GetCirclePosition2D(center, 360f, count, i, radius);
            }
        }
    }
}