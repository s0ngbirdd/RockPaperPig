using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "BoardObject/BoardObject", fileName = "BoardObject")]
    public class BoardObject : ScriptableObject
    {
        [SerializeField] private Sprite _objectSprite;
        [SerializeField] private LayerMask _objectLayer;
        [SerializeField] private LayerMask _objectToDestroyLayer;
        [SerializeField] private string _objectLayerName;
        [SerializeField] private string _soundNameToPlay;

        public Sprite ObjectSprite => _objectSprite;
        public LayerMask ObjectLayer => _objectLayer;
        public LayerMask ObjectToDestroyLayer => _objectToDestroyLayer;
        public string ObjectLayerName => _objectLayerName;
        public string SoundNameToPlay => _soundNameToPlay;
    }
}
