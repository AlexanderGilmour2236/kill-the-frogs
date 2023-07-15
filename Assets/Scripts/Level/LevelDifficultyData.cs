using System;

namespace KillTheFrogs
{
    [Serializable]
    public class LevelDifficultyData
    {
        public int levelPartsCount;
        public int minRoadParts;
        public int maxRoadParts;
        public int minRailRoadParts;
        public int maxRailRoadParts;


        public int frogsCount;
        public int frogsCrossedToWin = 3;
        public float frogsMinTimeToMove;
        public float frogsMaxTimeToMove;

        public float frogSpawnMinTime;
        public float frogSpawnMaxTime;
        
        public int frogMoveSidewaysChance = 3;
    }
}