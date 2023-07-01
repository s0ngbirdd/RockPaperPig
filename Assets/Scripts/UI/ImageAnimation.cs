using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private int _framesPerSprite = 6;
    [SerializeField] private bool _loop = true;
    [SerializeField] private bool _deactivateOnEnd = false;

    private int _index = 0;
    private Image _image;
    private int _frame = 0;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _index = 0;
        _frame = 0;
    }

    private void Update()
    {
        if (!_loop && _index == _sprites.Length)
        {
            return;
        }
        
        _frame++;

        if (_frame < _framesPerSprite)
        {
            return;
        }
        
        _image.sprite = _sprites[_index];
        _frame = 0;
        _index ++;
        
        if (_index >= _sprites.Length)
        {
            if (_loop)
            {
                _index = 0;
            }

            if (_deactivateOnEnd)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
