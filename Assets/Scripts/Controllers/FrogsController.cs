using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KillTheFrogs
{
    public class FrogsController
    {
        private readonly SceneAssetsAccessor _sceneAssetsAccessor;
        private FrogFactory _frogFactory;
        private List<FrogView> _frogViews = new List<FrogView>();
        private int _maxHorizontalPosition;
        private bool _frogSpawning;
        private LevelController _levelController;
        private CameraController _cameraController;
        private Coroutine _spawnerCoroutine;
        private LevelDifficultyData _levelDifficultyData;

        private Dictionary<Vector2Int, IHaveGridPosition> _gridPositionToObject = new Dictionary<Vector2Int, IHaveGridPosition>();

        public event Action<FrogView> frogDie;
        public event Action<FrogView> frogMove;
        public event Action<FrogView> frogJumpEnd;

        public FrogsController(SceneAssetsAccessor sceneAssetsAccessor, LevelController levelController,
            CameraController cameraController)
        {
            _sceneAssetsAccessor = sceneAssetsAccessor;
            _frogFactory = sceneAssetsAccessor.frogFactory;
            _maxHorizontalPosition = sceneAssetsAccessor.maxFrogHorizontalPosition;
            _levelController = levelController;
            _cameraController = cameraController;

        }

        public IEnumerator frogSpawnerRoutine(int frogsCount)
        {
            _frogSpawning = true;
            List<Transform> frogStartPositions = new List<Transform>(_sceneAssetsAccessor.frogStartPositions);

            while (_frogSpawning)
            {
                if (frogsCount <= 0)
                {
                    break;
                }
                int startPointIndex = Random.Range(0, frogStartPositions.Count);
                Vector3 startPointPosition = frogStartPositions[startPointIndex].position;
                Vector2Int newFrogPosition = new Vector2Int((int)startPointPosition.x, (int)startPointPosition.z);
                
                if (isGridPositionAvailable(newFrogPosition))
                {
                    FrogView frogView = _frogFactory.getFrog();
                    frogView.setDifficultyData(_levelDifficultyData);
                    
                    Transform frogViewTransform = frogView.transform;
                
                    frogViewTransform.SetParent(_sceneAssetsAccessor.levelParent);
                
                    frogViewTransform.position = startPointPosition;
                    frogViewTransform.localRotation = frogStartPositions[startPointIndex].localRotation;
                
                    frogsCount--;
                
                    _frogViews.Add(frogView);
                    _gridPositionToObject[frogView.getGridPosition()] = frogView;
                    
                    onFrogSpawned(frogView);
                }
                
                yield return new WaitForSeconds(Random.Range(_levelDifficultyData.frogSpawnMinTime, _levelDifficultyData.frogSpawnMaxTime));
            }

            _spawnerCoroutine = null;
        }

        private void onFrogSpawned(FrogView frogView)
        {
            subscribeFrogView(frogView);
            frogView.go(frogView.transform.localPosition);
        }

        public void startFrogSpawner(int frogsCount)
        {
            _spawnerCoroutine = _sceneAssetsAccessor.StartCoroutine(frogSpawnerRoutine(frogsCount));
        }

        public void onFrogDie(FrogView frogView)
        {
            ParticleSystem frogDiePS = Object.Instantiate(_sceneAssetsAccessor.frogDiePS);
            frogDiePS.transform.position = frogView.transform.position;
            _sceneAssetsAccessor.StartCoroutine(destroyParticleEffect(frogDiePS));
            frogDie?.Invoke(frogView);
            
            destroyFrog(frogView);
            setCameraTargetToFirstFrog();
        }

        private IEnumerator destroyParticleEffect(ParticleSystem frogDiePS)
        {
            yield return new WaitForSeconds(frogDiePS.main.duration);
            Object.Destroy(frogDiePS.gameObject);
        }

        private void destroyFrog(FrogView frogView)
        {
            _gridPositionToObject.Remove(frogView.getGridPosition());
            GameObject.Destroy(frogView.gameObject);
            unsubscribeFrogView(frogView);
            _frogViews.Remove(frogView);
        }

        private void subscribeFrogView(FrogView frogView)
        {
            frogView.moveDecided += onFrogMoveDecided;
            frogView.jumpEnded += onFrogJumpEnd;
        }

        private void unsubscribeFrogView(FrogView frogView)
        {
            frogView.moveDecided -= onFrogMoveDecided;
            frogView.jumpEnded -= onFrogJumpEnd;
        }

        private void onFrogJumpEnd(FrogView frogView)
        {
            frogJumpEnd?.Invoke(frogView);
        }

        private void onFrogMoveDecided(FrogView frogView)
        {
            Vector2Int frogGridPosition = frogView.getGridPosition();
            LevelPart currentLevelPart = _levelController.getLevelPartByZPosition(frogGridPosition.y);
            LevelPart levelPartByNextZPosition = _levelController.getLevelPartByZPosition(frogGridPosition.y + 1);
            
            Vector3 moveDirection = Vector3.forward;
            if (frogView.onlyForward)
            {
                moveDirection = Vector3.forward;
            }
            else
            {
                /*if (currentLevelPart != null && currentLevelPart.levelPartType == LevelPartType.Default &&
                    levelPartByNextZPosition != null && levelPartByNextZPosition.levelPartType == LevelPartType.Default
                    && isGridPositionAvailable(levelPartByNextZPosition.getGridPosition()))
                {
                    moveDirection = Vector3.forward;
                } 
                else*/
                {
                    if (Random.Range(0, _levelDifficultyData.frogMoveSidewaysChance) == 0)
                    {
                        moveDirection = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
                    }
                }
            }
            
            if (isMovePossible(frogView, moveDirection))
            {
                moveFrogView(frogView, frogGridPosition, moveDirection);
            }
            else
            {
                moveDirection = getPossibleMove(frogView);
                if (moveDirection != Vector3.zero)
                {
                    moveFrogView(frogView, frogGridPosition, moveDirection);
                }

            }
            setCameraTargetToFirstFrog();
        }

        private void moveFrogView(FrogView frogView, Vector2Int frogGridPosition, Vector3 moveDirection)
        {
            _gridPositionToObject.Remove(frogGridPosition);
            frogView.makeMove(moveDirection);
            frogView.rotateModel(moveDirection);
            frogMove?.Invoke(frogView);
            _gridPositionToObject[frogView.getGridPosition()] = frogView;
        }

        private Vector3 getPossibleMove(FrogView frogView)
        {
            Vector3[] possibleDirections = new Vector3[]
            {
                Vector3.forward, Vector3.left, Vector3.right,
            };

            foreach (Vector3 possibleDirection in possibleDirections)
            {
                if (isMovePossible(frogView, possibleDirection))
                {
                    return possibleDirection;
                }
            }
            return Vector3.zero;
        }

        private void setCameraTargetToFirstFrog()
        {
            Transform firstGoingFrogTransform = null;

            if (_frogViews.Count > 0)
            {
                int maxFrogZPosition = 0;
            
                foreach (FrogView frogView in _frogViews)
                {
                    if (frogView.onlyForward)
                    {
                        continue;
                    }
                    Vector2Int frogGridPosition = frogView.getGridPosition();
                    if (frogGridPosition.y >= maxFrogZPosition)
                    {
                        maxFrogZPosition = frogGridPosition.y;
                        firstGoingFrogTransform = frogView.transform;
                    }
                }
            }
            _cameraController.setTargetPoint(firstGoingFrogTransform);   
        }

        private bool isMovePossible(FrogView frogView, Vector3 moveDirection)
        {
            Vector2Int newFrogPosition = frogView.getGridPosition();
            newFrogPosition.x += (int)moveDirection.x;
            newFrogPosition.y += (int)moveDirection.z;

            return isGridPositionAvailable(newFrogPosition);
        }

        private bool isGridPositionAvailable(Vector2Int newFrogPosition)
        {
            if (_gridPositionToObject.ContainsKey(newFrogPosition) || Mathf.Abs(newFrogPosition.x) > _maxHorizontalPosition)
            {
                return false;
            }

            return true;
        }

        public void onFrogCrossedTheRoad(FrogView frogView)
        {
            frogView.onlyForward = true;
        }

        public void addObstacles(List<ObstacleView> obstacles)
        {
            foreach (ObstacleView obstacleView in obstacles)
            {
                _gridPositionToObject[obstacleView.getGridPosition()] = obstacleView;
            }
        }

        public void clearFrogs()
        {
            foreach (FrogView frogView in _frogViews)
            {
                Object.Destroy(frogView.gameObject);
            }

            if (_spawnerCoroutine != null)
            {
                _sceneAssetsAccessor.StopCoroutine(_spawnerCoroutine);
            }
            _frogViews.Clear();
            _frogSpawning = false;
            _gridPositionToObject.Clear();
        }

        public void setLevelDifficulty(LevelDifficultyData levelDifficultyData)
        {
            _levelDifficultyData = levelDifficultyData;
        }
    }
}