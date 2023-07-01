using UI;
using UnityEngine;

namespace Particle
{
    public class ParticleDeactivator : MonoBehaviour
    {
        private void OnEnable()
        {
            GameUIController.OnRestart += DeactivateSelf;
            ASyncLoader.OnLoadScene += DeactivateSelf;
        }

        private void OnDisable()
        {
            GameUIController.OnRestart -= DeactivateSelf;
            ASyncLoader.OnLoadScene -= DeactivateSelf;
        }

        private void DeactivateSelf()
        {
            gameObject.SetActive(false);
        }
    }
}
