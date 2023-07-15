using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KillTheFrogs
{
    public class CarTapStrategy : TapStrategy
    {
        [SerializeField] private List<CarTrap> _carTrapPrefabs;
        [SerializeField] private Transform _carStartPosition;
        [SerializeField] private float _selfCooldown;
        [SerializeField] private GameObject _coolDownIndicator;

        private float _cooldownLeft;
        private List<CarTrap> _currentCarTraps = new List<CarTrap>();

        public override void onTap()
        {
            base.onTap();
            if (_cooldownLeft <= 0)
            {
                if (_coolDownIndicator != null)
                {
                    _coolDownIndicator.gameObject.SetActive(true);
                }
                CarTrap carTrap = Instantiate(_carTrapPrefabs[Random.Range(0,_carTrapPrefabs.Count)], _carStartPosition.position, _carStartPosition.rotation);
                carTrap.onTrapStart();
                _currentCarTraps.Add(carTrap);
                subscribeCarTrap(carTrap);
                _cooldownLeft = _selfCooldown;
            }
            
        }

        private void subscribeCarTrap(CarTrap carTrap)
        {
            carTrap.frogCollided += invokeTrapCollideWithFrog;
            carTrap.carDestroy += destroyCar;
        }

        private void destroyCar(CarTrap carTrap)
        {
            invokeTrapViewDestroyed(carTrap);
            Destroy(carTrap.gameObject);
            _currentCarTraps.Remove(carTrap);
        }

        private void Update()
        {
            _cooldownLeft -= Time.deltaTime;
            if (_cooldownLeft <= 0)
            {
                if (_coolDownIndicator != null)
                {
                    _coolDownIndicator.gameObject.SetActive(false);
                }
            }
        }

        public void destroy()
        {
            foreach (var carTrap in _currentCarTraps.ToList())
            {
                destroyCar(carTrap);
            }
        }
    }
}