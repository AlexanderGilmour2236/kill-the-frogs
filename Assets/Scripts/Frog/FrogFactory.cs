using UnityEngine;

namespace KillTheFrogs
{
    public class FrogFactory : MonoBehaviour
    {
        [SerializeField] private FrogView _frogViewPrefab;
        
        public FrogView getFrog()
        {
            return Instantiate(_frogViewPrefab);
        }
    }
}