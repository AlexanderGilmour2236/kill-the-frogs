using System;
using DG.Tweening;
using UnityEngine;

namespace KillTheFrogs
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _cameraLerpSpeed = 0.3f;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _minZOffset = 0;
        [SerializeField] private float _minOffsetToGo = 10;
        
        private float _maxZOffset;

        private Transform _targetPoint;
        private Tween _cameraShakeTween;

        public void doShake(float intensity)
        {
            if (_cameraShakeTween != null)
            {
                _cameraShakeTween.Kill(true);
                _cameraShakeTween = null;
            }
            _cameraShakeTween = _camera.DOShakePosition(0.5f, intensity);
        }

        public void setTargetPoint(Transform targetPoint)
        {
            _targetPoint = targetPoint;
        }

        public void setMaxCameraZOffset(float maxZOffset)
        {
            _maxZOffset = maxZOffset;
        }

        private void Update()
        {
            followTargetPoint();
        }

        private void followTargetPoint()
        {
            if (_targetPoint != null)
            {
                Vector3 targetPosition = _targetPoint.transform.position + _offset;
                Transform cameraTransform = transform;
            
                targetPosition.x = cameraTransform.position.x;
                targetPosition.y = cameraTransform.position.y;
                targetPosition.z -= _minOffsetToGo;
                targetPosition.z = Mathf.Clamp(targetPosition.z, _minZOffset, _maxZOffset);

                transform.position = Vector3.Lerp(cameraTransform.position, targetPosition, _cameraLerpSpeed);
            }
        }
    }
}