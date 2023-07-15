using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace KillTheFrogs
{
    public class TrapController
    {
        private LevelController _levelController;
        private FrogsController _frogsController;
        private CameraController _cameraController;
        private bool _isTrapsWorking = true;
        private Dictionary<TrapView, int> _trapViewToKilledFrogs = new Dictionary<TrapView, int>();
        
        private readonly GameUI _gameUI;

        public event Action<TapStrategy> trapTap;

        public TrapController(LevelController levelController, FrogsController frogsController, CameraController cameraController, GameUI gameUI)
        {
            _levelController = levelController;
            _frogsController = frogsController;
            _cameraController = cameraController;
            _gameUI = gameUI;
        }
        
        public void onStartGame()
        {
            foreach (LevelPart levelPart in _levelController.currentLevelParts)
            {
                foreach (TapStrategy tapStrategy in levelPart.tapStrategies)
                {
                    subscribeTapStrategy(tapStrategy);
                }
            }
        }
        
        private void subscribeTapStrategy(TapStrategy tapStrategy)
        {
            tapStrategy.tap += onTapStrategyTap;
            tapStrategy.onTrapCollideWithFrog += onTrapCollidedWithFrog;
            tapStrategy.trapViewDestroyed += onTrapViewDestroyed;
        }

        private void onTrapViewDestroyed(TrapView trapView)
        {
            _trapViewToKilledFrogs.Remove(trapView);
            _gameUI.destroyComboIndicator(trapView);
        }

        private void onTrapCollidedWithFrog(FrogView frogView, TrapView trapView)
        {
            if (!_trapViewToKilledFrogs.ContainsKey(trapView))
            {
                _trapViewToKilledFrogs[trapView] = 0;
            }
            _trapViewToKilledFrogs[trapView]++;
            _gameUI.showFrogsComboIndicator(trapView, _trapViewToKilledFrogs[trapView], frogView);
            _frogsController.onFrogDie(frogView);
            _cameraController.doShake(Random.Range(0.4f,1));
        }
        
        public void clearTraps()
        {
            _trapViewToKilledFrogs.Clear();
        }
        
        private void onTapStrategyTap(TapStrategy tapStrategy)
        {
            if (_isTrapsWorking)
            {
                tapStrategy.onTap();
                trapTap?.Invoke(tapStrategy);
            }
        }

        public void setIsTrapsWorking(bool isTrapsWorking)
        {
            _isTrapsWorking = isTrapsWorking;
        }
    }
}