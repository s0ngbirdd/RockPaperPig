using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class HintController : MonoBehaviour
    {
        [SerializeField] private GameObject _movingBoardObject;
        [SerializeField] private List<GameObject> _standingBoardObjects = new List<GameObject>();
        [SerializeField] private List<GameObject> _smoke = new List<GameObject>();
        [SerializeField] private List<Sprite> _movingBoardObjectSprites = new List<Sprite>();
        [SerializeField] private List<Sprite> _standingBoardObjectsSprites = new List<Sprite>();
        [SerializeField] private float _moveEndValue = 0.5f;
        [SerializeField] private float _movingBoardObjectMoveSpeed = 1f;
        [SerializeField] private float _timeBeforeRestartAnimation = 0.5f;
        [SerializeField] private bool _startMovingOnStart;

        private Vector3 _startPosition;
        private Tween _tween;
        private int _spriteIndex = 0;

        private void OnEnable()
        {
            GameUIController.OnEnableHintPopup += StartMoving;
            GameUIController.OnDisableHintPopup += StopMoving;
        }

        private void OnDisable()
        {
            GameUIController.OnEnableHintPopup -= StartMoving;
            GameUIController.OnDisableHintPopup -= StopMoving;
        }

        private void Start()
        {
            _movingBoardObject.GetComponent<Image>().sprite = _movingBoardObjectSprites[_spriteIndex];

            foreach (GameObject boardObject in _standingBoardObjects)
            {
                boardObject.GetComponent<Image>().sprite = _standingBoardObjectsSprites[_spriteIndex];
            }
            
            _startPosition = _movingBoardObject.transform.position;

            if (_startMovingOnStart)
            {
                StartMoving();
            }
        }

        private void StartMoving()
        {
            _tween.Kill();
            
            _tween = _movingBoardObject.transform.DOMoveY(_moveEndValue, _movingBoardObjectMoveSpeed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
            {
                for (int i = 0; i < _standingBoardObjects.Count; i++)
                {
                    _standingBoardObjects[i].SetActive(false);
                    _smoke[i].SetActive(true);
                }
                
                EndMoving();
            }).SetUpdate(true);
        }
        
        private void EndMoving()
        {
            _tween.Kill();
            
            _tween = _movingBoardObject.transform.DOMoveY(_moveEndValue + 0.5f, _movingBoardObjectMoveSpeed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
            {
                StartCoroutine(RestartAnimation());
            }).SetUpdate(true);
        }

        private IEnumerator RestartAnimation()
        {
            yield return new WaitForSecondsRealtime(_timeBeforeRestartAnimation);
            
            _spriteIndex++;
            
            if (_spriteIndex > _movingBoardObjectSprites.Count - 1)
            {
                _spriteIndex = 0;
            }
            
            _movingBoardObject.transform.position = _startPosition;
            
            _movingBoardObject.GetComponent<Image>().sprite = _movingBoardObjectSprites[_spriteIndex];
            
            foreach (GameObject boardObject in _standingBoardObjects)
            {
                boardObject.GetComponent<Image>().sprite = _standingBoardObjectsSprites[_spriteIndex];
                boardObject.SetActive(true);
            }

            StartMoving();
        }

        private void StopMoving()
        {
            _tween.Kill();
        }
    }
}
