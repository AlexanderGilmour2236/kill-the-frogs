using System.Collections.Generic;
using UnityEngine;

namespace KillTheFrogs
{
    public class LevelPartsFactory
    {
        private readonly LevelConfig _levelPartsConfig;
        private readonly Dictionary<LevelPartType, List<LevelPart>> _levelTypeToParts;

        public LevelPartsFactory(SceneAssetsAccessor sceneAssetsAccessor)
        {
            _levelPartsConfig = sceneAssetsAccessor.levelConfig;
            _levelTypeToParts = new Dictionary<LevelPartType, List<LevelPart>>();

            foreach (LevelPart levelPart in _levelPartsConfig.levelParts)
            {
                LevelPartType levelPartType = levelPart.levelPartType;
                if (!_levelTypeToParts.ContainsKey(levelPartType))
                {
                    _levelTypeToParts[levelPartType] = new List<LevelPart>();
                }
                _levelTypeToParts[levelPartType].Add(levelPart);
            }
        }
        
        public LevelPart getLevelPart(LevelPartType levelPartType)
        {
            List<LevelPart> levelTypeToParts = _levelTypeToParts[levelPartType];
            return levelTypeToParts[Random.Range(0, levelTypeToParts.Count)];
        }
    }
}