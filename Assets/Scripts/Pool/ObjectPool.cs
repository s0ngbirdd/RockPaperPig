using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pool
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        
        [Header("Board Object")]
        [SerializeField] private GameObject _boardObjectPrefab; // object to pool
        [SerializeField] private int _boardObjectAmount; // amount to pool
        
        [Header("Particle")]
        [SerializeField] private GameObject _particleObjectPrefab; // object to pool
        [SerializeField] private int _particleObjectAmount; // amount to pool

        private List<GameObject> _boardPooledObjects; // pooled objects
        private List<GameObject> _particlePooledObjects; // pooled objects

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            _boardPooledObjects = new List<GameObject>();
            _particlePooledObjects = new List<GameObject>();
            GameObject temp;
        
            for (int i = 0; i < _boardObjectAmount; i++)
            {
                temp = Instantiate(_boardObjectPrefab, transform);
                temp.SetActive(false);
                _boardPooledObjects.Add(temp);
            }
        
            for (int i = 0; i < _particleObjectAmount; i++)
            {
                temp = Instantiate(_particleObjectPrefab, transform);
                temp.SetActive(false);
                _particlePooledObjects.Add(temp);
            }
        }
    
        // get pooled object
        public GameObject GetBoardPooledObject()
        {
            for (int i = 0; i < _boardObjectAmount; i++)
            {
                if (!_boardPooledObjects[i].activeInHierarchy)
                {
                    return _boardPooledObjects[i];
                }
            }
        
            return null;
        }
    
        // get pooled object
        public GameObject GetParticlePooledObject()
        {
            for (int i = 0; i < _particleObjectAmount; i++)
            {
                if (!_particlePooledObjects[i].activeInHierarchy)
                {
                    return _particlePooledObjects[i];
                }
            }
        
            return null;
        }
    }
}
