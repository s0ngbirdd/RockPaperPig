using System;
using System.Collections;
using Column;
using UnityEngine;

namespace Controllers
{
    public class GameFlowController : MonoBehaviour
    {
        public static event Action OnGameEnd;
    
        [SerializeField] private float _timeBeforeGameEnd = 1f;

        private int _spawnBlocked = 0;
        private bool _canRestart = true;
        private Coroutine _coroutine;

        private void OnEnable()
        {
            SpawnBlocker.OnSpawnBlock += IncreaseSpawnBlocked;
            SpawnBlocker.OnSpawnUnblock += DecreaseSpawnBlocked;
        }

        private void OnDisable()
        {
            SpawnBlocker.OnSpawnBlock -= IncreaseSpawnBlocked;
            SpawnBlocker.OnSpawnUnblock -= DecreaseSpawnBlocked;
        }

        private void Update()
        {
            if (_spawnBlocked >= 5 && _canRestart)
            {
                _canRestart = false;
                _coroutine = StartCoroutine(WaitForGameEnd());
            }
        }

        private void IncreaseSpawnBlocked()
        {
            _spawnBlocked++;
        }

        private void DecreaseSpawnBlocked()
        {
            _spawnBlocked--;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        
            _canRestart = true;
        }

        private IEnumerator WaitForGameEnd()
        {
            yield return new WaitForSeconds(_timeBeforeGameEnd);
        
            OnGameEnd?.Invoke();
        }
    }
}