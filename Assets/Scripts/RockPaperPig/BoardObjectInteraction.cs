using Audio;
using Controllers;
using Pool;
using UI;
using UnityEngine;

namespace RockPaperPig
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoardObjectInteraction : MonoBehaviour
    {
        private LayerMask _objectLayer;
        private LayerMask _objectToDestroyLayer;
        private string _soundNameToPlay;
    
        private GameObject _boardObjectLeft;
        private GameObject _boardObjectRight;
        private RaycastHit2D _raycastHitRight;
        private RaycastHit2D _raycastHitLeft;
        private RaycastHit2D _raycastHitUp;
        private float _extraDistance = 0.2f;
        private float _extraDistanceUp = 0.02f;
        private BoxCollider2D _collider2D;
        private Color _rayColorRight = Color.black;
        private Color _rayColorLeft = Color.black;
        private Color _rayColorUp = Color.black;
        private GameScoreController _gameScoreController;
        
        public bool _isDisabled;

        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            DisableBoardObject();
        }

        private void OnEnable()
        {
            _gameScoreController = FindObjectOfType<GameScoreController>();
            
            ASyncLoader.OnLoadScene += DisableBoardObject;
            GameUIController.OnRestart += DisableBoardObject;
        }

        private void Update()
        {
            _raycastHitRight = Physics2D.Raycast(_collider2D.bounds.center, Vector2.right, _collider2D.bounds.extents.x + _extraDistance, _objectLayer);
            _raycastHitLeft = Physics2D.Raycast(_collider2D.bounds.center, Vector2.left, _collider2D.bounds.extents.x + _extraDistance, _objectLayer);
            _raycastHitUp = Physics2D.Raycast(_collider2D.bounds.center, Vector2.up, _collider2D.bounds.extents.x + _extraDistanceUp, _objectToDestroyLayer);

            if (_raycastHitRight.collider != null)
            {
                _rayColorRight = Color.red;
                _boardObjectRight = _raycastHitRight.collider.gameObject;
            }
            else
            {
                _rayColorRight = Color.black;
                _boardObjectRight = null;
            }

            if (_raycastHitLeft.collider != null)
            {
                _rayColorLeft = Color.red;
                _boardObjectLeft = _raycastHitLeft.collider.gameObject;
            }
            else
            {
                _rayColorLeft = Color.black;
                _boardObjectLeft = null;
            }

            if (_raycastHitUp.collider != null)
            {
                _rayColorUp = Color.red;
                _raycastHitUp.collider.gameObject.SetActive(false);
            }
            else
            {
                _rayColorUp = Color.black;
            }
        }

        private void OnDisable()
        {
            if (_isDisabled)
            {
                _isDisabled = false;
            }
            else
            {
                if (_gameScoreController != null)
                {
                    _gameScoreController.IncreaseScore();
                    
                    GameObject particle = ObjectPool.Instance.GetParticlePooledObject();
                    particle.transform.position = transform.position;
                    particle.SetActive(true);

                    if (!AudioManager.Instance.ReturnAudioSource(_soundNameToPlay).isPlaying)
                    {
                        AudioManager.Instance.PlayOneShot(_soundNameToPlay);
                    }
            
                    if (_boardObjectLeft != null)
                    {
                        _boardObjectLeft.SetActive(false);
                    }

                    if (_boardObjectRight != null)
                    {
                        _boardObjectRight.SetActive(false);
                    }
                }
            }

            // to avoid collisions after enabling
            transform.position = new Vector2(transform.position.x, -4.5f);
            
            ASyncLoader.OnLoadScene -= DisableBoardObject;
            GameUIController.OnRestart -= DisableBoardObject;
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(_collider2D.bounds.center, Vector2.right * (_collider2D.bounds.extents.x + _extraDistance), _rayColorRight);
            Debug.DrawRay(_collider2D.bounds.center, Vector2.left * (_collider2D.bounds.extents.x + _extraDistance), _rayColorLeft);
            Debug.DrawRay(_collider2D.bounds.center, Vector2.up * (_collider2D.bounds.extents.x + _extraDistanceUp), _rayColorUp);
        }

        private void DisableBoardObject()
        {
            _isDisabled = true;
            gameObject.SetActive(false);
            Debug.Log("DIS <===>");
        }

        public void SetObjectBehaviour(ScriptableObjects.BoardObject boardObject)
        {
            GetComponent<SpriteRenderer>().sprite = boardObject.ObjectSprite;
            gameObject.layer = LayerMask.NameToLayer(boardObject.ObjectLayerName);
            _objectLayer = boardObject.ObjectLayer;
            _objectToDestroyLayer = boardObject.ObjectToDestroyLayer;
            _soundNameToPlay = boardObject.SoundNameToPlay;
        }
    }
}
