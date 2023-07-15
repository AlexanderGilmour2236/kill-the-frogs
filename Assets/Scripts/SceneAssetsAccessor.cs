using System.Collections.Generic;
using UnityEngine;

namespace KillTheFrogs
{
    public class SceneAssetsAccessor : MonoBehaviour
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private Transform _levelStartPoint;
        [SerializeField] private Transform _levelParent;
        [SerializeField] private FrogFactory _frogFactory;
        [SerializeField] private ParticleSystem _frogDiePS;
        [SerializeField] private List<Transform> _frogStartPositions;
        [SerializeField] private List<ObstacleView> _obstaclePrefabs;

        [Header("Frog spawner settings")]
        [SerializeField] private int _maxFrogHorizontalPosition = 4;
        [SerializeField] private float _frogSpawnMinTime = 1;
        [SerializeField] private float _frogSpawnMaxTime = 4;
        [SerializeField] private int _frogsCount = 10;

        public int frogsCount
        {
            get { return _frogsCount; }
        }

        public float frogSpawnMinTime
        {
            get { return _frogSpawnMinTime; }
        }

        public float frogSpawnMaxTime
        {
            get { return _frogSpawnMaxTime; }
        }

        public int maxFrogHorizontalPosition
        {
            get { return _maxFrogHorizontalPosition; }
        }

        public FrogFactory frogFactory
        {
            get { return _frogFactory; }
        }

        public virtual LevelConfig levelConfig
        {
            get { return _levelConfig; }
        }

        public Transform levelStartPoint
        {
            get { return _levelStartPoint; }
        }

        public Transform levelParent
        {
            get { return _levelParent; }
        }

        public List<Transform> frogStartPositions
        {
            get { return _frogStartPositions; }
        }
        
        public ParticleSystem frogDiePS
        {
            get { return _frogDiePS; }
        }
        
        public CameraController cameraController
        {
            get { return _cameraController; }
        }
        
        public GameUI gameUI
        {
            get { return _gameUI; }
        }

        public List<ObstacleView> obstaclePrefabs
        {
            get
            {
                return _obstaclePrefabs;
            }
        }
    }
}