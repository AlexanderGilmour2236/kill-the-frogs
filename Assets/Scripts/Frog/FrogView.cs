using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KillTheFrogs
{
    public class FrogView : MonoBehaviour, IHaveGridPosition
    {
        [SerializeField] private Transform _modelTransform;
        
        [SerializeField] private float _minTimeToMakeMove = 0.3f;
        [SerializeField] private float _maxTimeToMakeMove = 1;
        [SerializeField] private float _lerpSpeed = 0.3f;
        [SerializeField] private float _jumpMinStrength = 0.3f;
        [SerializeField] private float _jumpManStrength = 0.5f;
        
        private float _timeToMoveLeft;
        private Vector3 _targetPosition;
        private bool _startedMoving;
        private Vector2Int _gridPosition;
        private Tween _jumpTween;
        private bool _onlyForward;


        public event Action<FrogView> moveDecided;
        public event Action<FrogView> jumpEnded;

        public void go(Vector3 startPosition)
        {
            transform.position = startPosition;
            Vector3 frogLocalPosition = transform.localPosition;
            
            _gridPosition = new Vector2Int((int)frogLocalPosition.x,(int)frogLocalPosition.z);
            _targetPosition = frogLocalPosition;
            _startedMoving = true;
            _timeToMoveLeft = getNextMoveTime();
        }

        private float getNextMoveTime()
        {
            return Random.Range(_minTimeToMakeMove, _maxTimeToMakeMove);
        }

        private void Update()
        {
            if (_startedMoving)
            {
                _timeToMoveLeft -= Time.deltaTime;

                if (_timeToMoveLeft <= 0)
                {
                    moveDecided?.Invoke(this);
                    _timeToMoveLeft = getNextMoveTime();
                }

                //transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, _lerpSpeed);
            }
        }

        public void rotateModel(Vector3 moveDirection)
        {
            _modelTransform.LookAt(_modelTransform.position + moveDirection);
        }

        public void makeMove(Vector3 moveDirection)
        {
            if (_jumpTween != null)
            {
                _jumpTween.Complete();
            }
            Vector3 frogLocalPosition = transform.localPosition;
            _targetPosition = frogLocalPosition + moveDirection;
            
            _jumpTween = transform
                .DOJump(_targetPosition, Random.Range(_jumpMinStrength, _jumpManStrength), 1, 0.5f)
                .OnComplete(() =>
                {
                    jumpEnded?.Invoke(this);
                    _jumpTween = null;
                });
            
            _gridPosition.x += (int)moveDirection.x;
            _gridPosition.y += (int)moveDirection.z;
        }

        public Vector2Int getGridPosition()
        {
            return _gridPosition;
        }

        public bool onlyForward
        {
            get { return _onlyForward; }
            set { _onlyForward = value; }
        }

        public void setDifficultyData(LevelDifficultyData levelDifficultyData)
        {
            _minTimeToMakeMove = levelDifficultyData.frogsMinTimeToMove;
            _maxTimeToMakeMove = levelDifficultyData.frogsMaxTimeToMove;
        }

        public void stop()
        {
            _startedMoving = false;
            
        }
    }
}