using UnityEngine;

namespace KillTheFrogs
{
    public class ObstacleView : MonoBehaviour, IHaveGridPosition
    {
        public Vector2Int getGridPosition()
        {
            Vector3 localPosition = transform.localPosition;
            
            Vector2Int gridPosition = new Vector2Int((int)localPosition.x,(int)localPosition.z);
            return gridPosition;
            
        }
    }
}