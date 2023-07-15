using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillTheFrogs
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private SceneAssetsAccessor _sceneAssetsAccessor;
        [SerializeField] private bool _clearSave;
        private Player _player;
        
        private void Start()
        {
            if (_clearSave)
            {
                PlayerPrefs.DeleteAll();
            }
            Application.targetFrameRate = 60;
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
