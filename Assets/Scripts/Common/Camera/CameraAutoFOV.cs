using Cinemachine;
using UnityEngine;

namespace Common.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraAutoFOV : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float _targetFOV = 32f;
        [SerializeField, Min(1f)] private float _targetOrthoSize = 32f;


        private void Awake() => OnValidate();


        private void OnValidate() 
        {
            var vc = GetComponent<CinemachineVirtualCamera>();
            var mainCamera = UnityEngine.Camera.main;
            var isOrthographicMode = mainCamera.orthographic;

            if (isOrthographicMode)
                vc.m_Lens.OrthographicSize = Util.CameraExtension.CalculateOrthographicSize(_targetFOV, _targetOrthoSize);
            else
                vc.m_Lens.FieldOfView = Util.CameraExtension.CalculateTargetFOV(_targetFOV, mainCamera.aspect);
        }
    }
}