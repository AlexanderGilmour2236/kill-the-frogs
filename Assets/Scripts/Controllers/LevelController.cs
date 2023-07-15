using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KillTheFrogs
{
    public class LevelController
    {
        private readonly SceneAssetsAccessor _sceneAssetsAccessor;
        private List<LevelPart> _currentLevelParts = new List<LevelPart>();

        private Transform _levelParent;
        private LevelPartType _previousLevelPartType;
        private LevelPartsFactory _levelPartsFactory;
        private List<ObstacleView> _obstacleViews = new List<ObstacleView>();


        public LevelController(SceneAssetsAccessor sceneAssetsAccessor)
        {
            _sceneAssetsAccessor = sceneAssetsAccessor;
            _levelParent = sceneAssetsAccessor.levelParent;
            _levelPartsFactory = new LevelPartsFactory(sceneAssetsAccessor);
        }

        public void createLevel(LevelDifficultyData levelDifficultyData)
        {
            clearLevel();
            
            int levelPartsCount = levelDifficultyData.levelPartsCount + 2;
            List<LevelPartType> allLevelPartTypes = new List<LevelPartType>();
            allLevelPartTypes = getNewLevelPartsTypes(levelDifficultyData, levelPartsCount);

            createLevelFromLevelPartTypes(allLevelPartTypes);
        }

        private void createLevelFromLevelPartTypes(List<LevelPartType> allLevelPartTypes)
        {
            Transform levelStart = _sceneAssetsAccessor.levelStartPoint;
            Vector3 nextPartPosition = levelStart.localPosition;

            for (int i = 0; i < allLevelPartTypes.Count; i++)
            {
                LevelPartType nextLevelPartType = allLevelPartTypes[i];
                if (i == allLevelPartTypes.Count - 1)
                {
                    nextLevelPartType = LevelPartType.LevelEnd;
                }
                
                LevelPart createdLevelPart = createNextLevelPart(nextLevelPartType, ref nextPartPosition);
                if (nextLevelPartType == LevelPartType.Default && i % 2 == 0)
                {
                    createObstaclesForLevelPart(createdLevelPart);
                }
                if (nextLevelPartType == LevelPartType.Road)
                {
                    createNextLevelPart(LevelPartType.Default, ref nextPartPosition);
                }
            }
        }

        private void createObstaclesForLevelPart(LevelPart levelPart)
        {
            int _maxObsctacles = 6;
            int obstaclesCount = Random.Range(0, _maxObsctacles);
            List<Transform> obstacleSlots = levelPart.obstaclesSlots.ToList();
            
            for (int i = 0; i < obstaclesCount; i++)
            {
                if (obstacleSlots.Count == 0)
                {
                    break;
                }

                int nextObstacleIndex = Random.Range(0, obstacleSlots.Count);
                ObstacleView obstacleView = Object.Instantiate(getRandomObstaclePrefab(), _levelParent, false);
                obstacleView.transform.position = obstacleSlots[nextObstacleIndex].transform.position;
                
                _obstacleViews.Add(obstacleView);
                obstacleSlots.RemoveAt(nextObstacleIndex);
            }
        }

        private ObstacleView getRandomObstaclePrefab()
        {
            return _sceneAssetsAccessor.obstaclePrefabs[Random.Range(0, _sceneAssetsAccessor.obstaclePrefabs.Count)];
        }

        private List<LevelPartType> getNewLevelPartsTypes(LevelDifficultyData levelDifficultyData, int levelPartsCount)
        {
            int roadPartsCount = Random.Range(levelDifficultyData.minRoadParts, levelDifficultyData.maxRoadParts + 1);
            int railRoadPartCount = Random.Range(levelDifficultyData.minRailRoadParts, levelDifficultyData.maxRailRoadParts + 1);
            
            List<LevelPartType> allLevelPartTypes = new List<LevelPartType>();
            
            for (int l = 0; l < roadPartsCount; l++)
            {
                allLevelPartTypes.Add(LevelPartType.Road);
            }
            
            for (int l = 0; l < railRoadPartCount; l++)
            {
                allLevelPartTypes.Add(LevelPartType.RailRoad);
            }

            for (int l = 0; l < levelPartsCount - roadPartsCount - railRoadPartCount; l++)
            {
                allLevelPartTypes.Add(LevelPartType.Default);
            }

            allLevelPartTypes.Shuffle();
            allLevelPartTypes.Insert(0, LevelPartType.Default);
            allLevelPartTypes.Insert(0, LevelPartType.Default);

            return allLevelPartTypes;
        }

        public void loadCertainLevel(List<LevelPartType> certainLevelPartTypes)
        {
            clearLevel();
            createLevelFromLevelPartTypes(certainLevelPartTypes);
        }

        private LevelPart createNextLevelPart(LevelPartType nextLevelPartType, ref Vector3 nextPartPosition)
        {
            LevelPart levelPart =
                Object.Instantiate(_levelPartsFactory.getLevelPart(nextLevelPartType), _levelParent, false);

            _currentLevelParts.Add(levelPart);

            levelPart.transform.localPosition = nextPartPosition;
            nextPartPosition.z += levelPart.getLevelPartLength();

            return levelPart;
        }

        public LevelPart getLevelPartByZPosition(int grigPositionZ)
        {
            foreach (LevelPart levelPart in _currentLevelParts)
            {
                if (levelPart.getGridPosition().y == grigPositionZ)
                {
                    return levelPart;
                }
            }
            return null;
        }

        public List<LevelPart> currentLevelParts
        {
            get { return _currentLevelParts; }
        }

        public List<ObstacleView> currentLevelObstacles
        {
            get { return _obstacleViews; }
        }

        public int getLevelLength()
        {
            int levelLength = 0;
            foreach (LevelPart levelPart in _currentLevelParts)
            {
                levelLength += (int)levelPart.getLevelPartLength();
            }

            return levelLength;
        }

        public void clearLevel()
        {
            foreach (LevelPart levelPart in _currentLevelParts)
            {
                Object.Destroy(levelPart.gameObject);
            }

            foreach (ObstacleView obstacleView in _obstacleViews)
            {
                Object.Destroy(obstacleView.gameObject);
            }
            _obstacleViews.Clear();
            _currentLevelParts.Clear();
            _previousLevelPartType = LevelPartType.Default;
        }
    }
}