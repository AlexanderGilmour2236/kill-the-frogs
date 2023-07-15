using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillTheFrogs
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private SceneAssetsAccessor _sceneAssetsAccessor;
        [SerializeField] private bool _clearSave;
        private Player _player;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            if (Application.isMobilePlatform)
            {
                QualitySettings.vSyncCount = 0;
            }
        }

        private void Start()
        {

            if (_clearSave)
            {
                PlayerPrefs.DeleteAll();
            }
            _player = new Player();
            GameController gameController = new GameController(_player, _sceneAssetsAccessor);
            _player.loadPlayer();
            gameController.startGame();
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
            }
        }
    }

}
