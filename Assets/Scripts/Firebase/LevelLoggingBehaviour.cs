using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Firebase
{
    public class LevelLoggingBehaviour : MonoBehaviour
    {
        private int _sceneIndex;
        private string _sceneName;
        
        private void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            _sceneIndex = activeScene.buildIndex;
            _sceneName = activeScene.name;
            
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
                new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
                new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
        }

        private void OnDestroy()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
                new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
        }
    }
}
