using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KillTheFrogs
{
    public class LevelPart : MonoBehaviour, IHaveGridPosition
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private LevelPartType _levelPartType;
        [SerializeField] private List<TapStrategy> _tapStrategies;
        
        [SerializeField] private int _length;
        [SerializeField] private Vector2Int _gridPosition;
        [SerializeField] private List<Transform> _obstaclesSlots;

        public List<TapStrategy> tapStrategies
        {
            get { return _tapStrategies; }
        }

        public LevelPartType levelPartType
        {
            get { return _levelPartType; }
        }

        public List<Transform> obstaclesSlots
        {
            get { return _obstaclesSlots; }
        }

        public float getLevelPartLength()
        {
            return _boxCollider.size.z;
        }

        public Vector2Int getGridPosition()
        {
            Vector3 levePartPosition = transform.localPosition;
            levePartPosition.z -= _length - 1 * 0.5f;
            _gridPosition = new Vector2Int((int)levePartPosition.x, (int)levePartPosition.z);
            
            return _gridPosition;
        }

        public LevelPart(int length)
        {
            _length = length;
        }
    }
}