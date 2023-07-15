using UnityEngine;

namespace KillTheFrogs
{
    public class Player
    {
        private const string LEVEL_INDEX_KEY = "level_index";

        public int levelIndex = -1;
        
        public void loadPlayer()
        {
            if (PlayerPrefs.HasKey(LEVEL_INDEX_KEY))
            {
                levelIndex = PlayerPrefs.GetInt(LEVEL_INDEX_KEY);
            }
        }        
        
        public void savePlayer()
        {
            PlayerPrefs.SetInt(LEVEL_INDEX_KEY, levelIndex);
        }
    }
}