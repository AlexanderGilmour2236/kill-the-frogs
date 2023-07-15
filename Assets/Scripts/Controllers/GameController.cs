using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KillTheFrogs
{
    public class GameController
    {
        private readonly SceneAssetsAccessor _sceneAssetsAccessor;
        private readonly LevelController _levelController;
        private readonly FrogsController _frogsController;
        private readonly CameraController _cameraController;
        private readonly TrapController _trapController;
        private TutorialController _tutorialController;

        private readonly GameUI _gameUI;
        private int _frogsLeft;
        private int _frogsCrossedTheRoad;
        
        private Dictionary<int, LevelDifficultyData> _levelIndexToDifficultyData = new Dictionary<int, LevelDifficultyData>();
        private readonly Player _player;
        private LevelDifficultyData _currentLevelDifficultyData;

        public GameController(Player player, SceneAssetsAccessor sceneAssetsAccessor)
        {
            _sceneAssetsAccessor = sceneAssetsAccessor;
            _cameraController = _sceneAssetsAccessor.cameraController;
            _player = player;
            _levelController = new LevelController(_sceneAssetsAccessor);
            _frogsController = new FrogsController(_sceneAssetsAccessor, _levelController, _cameraController);
            _gameUI = sceneAssetsAccessor.gameUI;
            _trapController = new TrapController(_levelController, _frogsController, _cameraController, _gameUI);
            _gameUI.newGameClick += onNewGameClick;
            
            _frogsController.frogDie += onFrogDie;
            _frogsController.frogMove += onFrogMove;
            _frogsController.frogJumpEnd += onFrogJumpEnd;

            for (var index = 0; index < _sceneAssetsAccessor.levelConfig.levelDifficultiesData.Count; index++)
            {
                LevelDifficultyData levelDifficultyData = _sceneAssetsAccessor.levelConfig.levelDifficultiesData[index];
                _levelIndexToDifficultyData[index] = levelDifficultyData;
            }
        }

        private void onNewGameClick()
        {
            startGame();
        }

        public void startGame()
        {

            _levelController.clearLevel();
            _frogsController.clearFrogs();
            _trapController.clearTraps();
            
            if (_player.levelIndex == -1)
            {
                loadTutorialLevel();
                _currentLevelDifficultyData = new LevelDifficultyData();
                _currentLevelDifficultyData.frogsCount = 1;
                _currentLevelDifficultyData.frogSpawnMinTime = 0;
                _currentLevelDifficultyData.frogSpawnMaxTime = 0.1f;
                _currentLevelDifficultyData.frogsMinTimeToMove = 0.5f;
                _currentLevelDifficultyData.frogsMaxTimeToMove = 1.0f;
                _currentLevelDifficultyData.frogMoveSidewaysChance = 10;

                _tutorialController = new TutorialController(_frogsController, _trapController, _levelController, _gameUI);
                _tutorialController.start();
            }
            else
            {
                _currentLevelDifficultyData = _levelIndexToDifficultyData[_player.levelIndex % _levelIndexToDifficultyData.Count];
                _levelController.createLevel(_currentLevelDifficultyData);

            }
            _frogsController.setLevelDifficulty(_currentLevelDifficultyData);
            _frogsLeft = _currentLevelDifficultyData.frogsCount;
            _frogsController.addObstacles(_levelController.currentLevelObstacles);
            _frogsCrossedTheRoad = 0;
            _frogsController.startFrogSpawner(_frogsLeft);
            _trapController.onStartGame();
            _gameUI.setFrogsLeft(_frogsLeft);

            _cameraController.setMaxCameraZOffset(_levelController.getLevelLength() + 4);
            _cameraController.setTargetPoint(_sceneAssetsAccessor.levelParent);
        }

        private void loadTutorialLevel()
        {
            List<LevelPartType> tutorialLevelPartTypes = new List<LevelPartType>()
            {
                LevelPartType.Default,
                LevelPartType.Default,
                LevelPartType.Default,
                LevelPartType.Default,
                LevelPartType.Default,
                LevelPartType.Road,
                LevelPartType.Default,
                LevelPartType.Default,
                LevelPartType.Default,
                LevelPartType.LevelEnd,
            };

            _levelController.loadCertainLevel(tutorialLevelPartTypes);
        }

        private void onFrogMove(FrogView frogView)
        {
        }

        private void checkGameOver()
        {
            if (_frogsLeft <= 0)
            {
                bool levelPass = _frogsCrossedTheRoad <= _currentLevelDifficultyData.frogsCrossedToWin;
                levelPass = true;
                _gameUI.goWinUI(_frogsLeft, _frogsCrossedTheRoad, levelPass);
                if (levelPass)
                {
                    _player.levelIndex++;
                }
                _player.savePlayer();
            }
        }

        private void onFrogDie(FrogView frogView)
        {
            _frogsLeft--;
            _gameUI.setFrogsLeft(_frogsLeft);
            checkGameOver();
        }

        private void onFrogJumpEnd(FrogView frogView)
        {
            if (!frogView.onlyForward && frogView.getGridPosition().y >= _levelController.getLevelLength() - 1)
            {
                _frogsCrossedTheRoad++;
                _frogsController.onFrogCrossedTheRoad(frogView);
                _frogsLeft--;
                _gameUI.setFrogsLeft(_frogsLeft, true);
                checkGameOver();
            }
        }
    }
}