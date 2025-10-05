using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneController
    {
        private readonly int _mainSceneIndex;
        public SceneController(int mainSceneIndex)
        {
            _mainSceneIndex = mainSceneIndex;
        }
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void LoadGameScene()
        {
            SceneManager.LoadScene(_mainSceneIndex);
        }
    }
}