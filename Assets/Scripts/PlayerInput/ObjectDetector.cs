using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerInput
{
    public class ObjectDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _objectLayerMask;
        [SerializeField] private float _raycastDistance = 150f;
    
        private PlayerInputActions _playerInputActions;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Click.performed += DetectObject;
        }
    
        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
        }
    
        private void DetectObject(InputAction.CallbackContext context)
        {
            Ray ray = _camera.ScreenPointToRay(_playerInputActions.Player.Position.ReadValue<Vector2>());

            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, _raycastDistance, _objectLayerMask);

            if (hit2D.collider != null)
            {
                //hit2D.collider.gameObject.GetComponent<BoardObjectSpawner>().ClickOnBoard();
            }
        }
    }
}
