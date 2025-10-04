using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneController
    {
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}