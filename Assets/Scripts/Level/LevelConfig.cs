using System.Collections.Generic;
using UnityEngine;

namespace KillTheFrogs
{
    [CreateAssetMenu(fileName = "levelConfig", menuName = "Data/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<LevelPart> _levelParts;
        [SerializeField] private List<LevelDifficultyData> _levelDifficultiesData;

        public List<LevelDifficultyData> levelDifficultiesData
        {
            get { return _levelDifficultiesData; }
        }

        public List<LevelPart> levelParts
        {
            get { return _levelParts; }
        }
    }
}