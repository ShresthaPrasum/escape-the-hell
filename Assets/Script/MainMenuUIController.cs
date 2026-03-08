using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Optional")]
    public GameObject playPanel;

    [Header("Play Scene")]
    public string gameSceneName = "";
    public int gameSceneBuildIndex = 1;

    void Start()
    {
        if (playPanel != null)
            playPanel.SetActive(true);
    }

    public void PlayGame()
    {
        // Ensure normal time scale in case menu was paused elsewhere.
        Time.timeScale = 1f;

        if (!string.IsNullOrWhiteSpace(gameSceneName))
        {
            if (CanLoadSceneByName(gameSceneName))
            {
                SceneManager.LoadScene(gameSceneName);
                return;
            }

            Debug.LogError($"PlayGame failed: scene name '{gameSceneName}' is not in Build Settings.");
            return;
        }

        if (gameSceneBuildIndex < 0 || gameSceneBuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"PlayGame failed: build index {gameSceneBuildIndex} is invalid. Check File > Build Settings.");
            return;
        }

        SceneManager.LoadScene(gameSceneBuildIndex);
    }

    bool CanLoadSceneByName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFile = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFile == sceneName)
                return true;
        }

        return false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
