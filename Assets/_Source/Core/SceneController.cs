using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneController
    {
        public int LastSceneIndex {get; private set;}
        public int CurrentSceneIndex {get; private set;}

        private readonly int _mainSceneIndex;
        public SceneController(int mainSceneIndex)
        {
            _mainSceneIndex = mainSceneIndex;
        }
        public void ReloadScene()
        {
            LastSceneIndex = SceneManager.GetActiveScene().buildIndex; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void LoadScene(int sceneIndex)
        {
            LastSceneIndex = SceneManager.GetActiveScene().buildIndex; 
            CurrentSceneIndex = sceneIndex;
            
            SceneManager.LoadScene(sceneIndex);
        }
        public void LoadGameScene()
        {
            LastSceneIndex = SceneManager.GetActiveScene().buildIndex; 
            CurrentSceneIndex = _mainSceneIndex;
            
            SceneManager.LoadScene(_mainSceneIndex);
        }
    }
}