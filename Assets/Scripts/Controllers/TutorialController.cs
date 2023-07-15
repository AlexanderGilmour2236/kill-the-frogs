namespace KillTheFrogs
{
    public class TutorialController
    {
        private readonly FrogsController _frogsController;
        private readonly TrapController _trapController;
        private readonly GameUI _gameUI;
        private readonly LevelController _levelController;

        public TutorialController(FrogsController frogsController, TrapController trapController, LevelController levelController, GameUI gameUI)
        {
            _frogsController = frogsController;
            _trapController = trapController;
            _levelController = levelController;
            _gameUI = gameUI;
        }

        public void start()
        {
            _frogsController.frogJumpEnd += onFrogJumpEnd;
            _trapController.setIsTrapsWorking(false);
        }

        private void onFrogJumpEnd(FrogView frogView)
        {
            LevelPart roadLevelPart = _levelController.getLevelPartByZPosition(frogView.getGridPosition().y);
            if (roadLevelPart.levelPartType ==
                LevelPartType.Road)
            {
                _trapController.setIsTrapsWorking(true);
                _frogsController.frogJumpEnd -= onFrogJumpEnd;
                frogView.stop();
                _gameUI.showHandOnObject(roadLevelPart.transform);
                _trapController.trapTap += onTrapTap;
            }
        }

        private void onTrapTap(TapStrategy obj)
        {
            _gameUI.hideHand();
            _trapController.trapTap -= onTrapTap;
        }
    }
}