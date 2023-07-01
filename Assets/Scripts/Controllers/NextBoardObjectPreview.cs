using System;
using System.Collections.Generic;
using Column;
using UnityEngine;

namespace Controllers
{
    public class NextBoardObjectPreview : MonoBehaviour
    {
        public static event Action<int> OnGenerateNewRandomObject;
    
        [SerializeField] private List<GameObject> _boardObjects;
    
        private int _randomIndex;

        private void OnEnable()
        {
            BoardObjectSpawner.OnSpawnObject += ActivateRandomObject;
        }

        private void OnDisable()
        {
            BoardObjectSpawner.OnSpawnObject -= ActivateRandomObject;
        }

        private void Start()
        {
            ActivateRandomObject();
        }

        private void ActivateRandomObject()
        {
            foreach (var boardObject in _boardObjects)
            {
                boardObject.SetActive(false);
            }

            _randomIndex = UnityEngine.Random.Range(0, _boardObjects.Count);
            _boardObjects[_randomIndex].SetActive(true);
            OnGenerateNewRandomObject?.Invoke(_randomIndex);
        }
    }
}
