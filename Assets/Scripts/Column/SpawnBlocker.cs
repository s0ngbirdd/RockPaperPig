using System;
using UnityEngine;

namespace Column
{
    public class SpawnBlocker : MonoBehaviour
    {
        public static event Action OnSpawnBlock;
        public static event Action OnSpawnUnblock;
    
        [SerializeField] private string _tagToCompare = "RockPaperPig";
        [SerializeField] private BoxCollider2D _boxCollider2D;
    
        private bool _canSpawn = true;

        public bool CanSpawn => _canSpawn;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(_tagToCompare) && _canSpawn)
            {
                _canSpawn = false;
                OnSpawnBlock?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(_tagToCompare))
            {
                _canSpawn = true;
                OnSpawnUnblock?.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
            if (_canSpawn)
            {
                Gizmos.color = Color.white;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireCube(transform.position + (Vector3)_boxCollider2D.offset, _boxCollider2D.size);
        }
    }
}
