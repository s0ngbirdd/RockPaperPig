using System;
using System.Collections;
using Audio;
using Controllers;
using RockPaperPig;
using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Column
{
    public class BoardObjectSpawner : MonoBehaviour
    {
        public static event Action OnSpawnObject;
    
        [Header("Spawn")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _timeBeforeSpawn = 0.5f;
        [SerializeField] private SpawnBlocker _spawnBlocker;
    
        [Header("Board Objects")]
        [SerializeField] private BoardObject _rockBoardObject;
        [SerializeField] private BoardObject _paperBoardObject;
        [SerializeField] private BoardObject _pigBoardObject;
    
        private int _randomIndex;
        private bool _isCoroutineEnd = true;
        private GameUIController _gameUIController;
        private PlayerInputActions _playerInputActions;
        private bool _isClicked;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            NextBoardObjectPreview.OnGenerateNewRandomObject += SetRandomIndex;
            
            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Click.performed += ClickOnBoard;
        }

        private void OnDisable()
        {
            NextBoardObjectPreview.OnGenerateNewRandomObject -= SetRandomIndex;
            
            _playerInputActions.Player.Disable();
        }
    
        private void Start()
        {
            _gameUIController = FindObjectOfType<GameUIController>();
        }

        private void OnMouseOver()
        {
            if (_isClicked && _isCoroutineEnd && _spawnBlocker.CanSpawn && !_gameUIController.IsPaused)
            {
                SpawnRandomObject();
                OnSpawnObject?.Invoke();

                _isCoroutineEnd = false;
                StartCoroutine(WaitBeforeSpawn());

                if (!AudioManager.Instance.ReturnAudioSource("Spawn").isPlaying)
                {
                    AudioManager.Instance.PlayOneShot("Spawn");
                }
            }
        }

        public void ClickOnBoard(InputAction.CallbackContext context)
        {
            _isClicked = true;

            StartCoroutine(ResetClick());
        }

        private void SpawnRandomObject()
        {
            if (_randomIndex == 0)
            {
                SpawnRock();
            }
            else if (_randomIndex == 1)
            {
                SpawnPaper();
            }
            else if (_randomIndex == 2)
            {
                SpawnPig();
            }
        }

        private void SpawnRock()
        {
            GameObject rock = Pool.ObjectPool.Instance.GetBoardPooledObject();
            rock.GetComponent<BoardObjectInteraction>().SetObjectBehaviour(_rockBoardObject);
            rock.transform.position = _spawnPoint.position;
            rock.SetActive(true);
        }

        private void SpawnPaper()
        {
            GameObject paper = Pool.ObjectPool.Instance.GetBoardPooledObject();
            paper.GetComponent<BoardObjectInteraction>().SetObjectBehaviour(_paperBoardObject);
            paper.transform.position = _spawnPoint.position;
            paper.SetActive(true);
        }

        private void SpawnPig()
        {
            GameObject pig = Pool.ObjectPool.Instance.GetBoardPooledObject();
            pig.GetComponent<BoardObjectInteraction>().SetObjectBehaviour(_pigBoardObject);
            pig.transform.position = _spawnPoint.position;
            pig.SetActive(true);
        }

        private void SetRandomIndex(int index)
        {
            _randomIndex = index;
        }

        private IEnumerator WaitBeforeSpawn()
        {
            yield return new WaitForSeconds(_timeBeforeSpawn);

            _isCoroutineEnd = true;
        }

        private IEnumerator ResetClick()
        {
            yield return new WaitForSecondsRealtime(_timeBeforeSpawn);
            
            _isClicked = false;
        }
    }
}
