using System;
using UnityEngine;

namespace KillTheFrogs
{
    public class CarTrap : TrapView
    {
        [SerializeField] private float _moveSpeed = 3;

        private bool _trapStarted;
        
        public event Action<FrogView, TrapView> frogCollided;
        public event Action<CarTrap> carDestroy;
        
        private int frogsKilled = 0;

        public void onTrapStart()
        {
            _trapStarted = true;
        }

        private void Update()
        {
            if (_trapStarted)
            {
                transform.localPosition += transform.TransformDirection(Vector3.forward) * _moveSpeed * Time.deltaTime;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagManager.Frog))
            {
                frogsKilled++;
                frogCollided?.Invoke(other.GetComponent<FrogView>(), this);
            }

            if (other.CompareTag(TagManager.DestroyInvisibleWall))
            {
                carDestroy?.Invoke(this);
            }
        }
    }
}