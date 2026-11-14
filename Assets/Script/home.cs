using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "Hell1"; 

    void Start()
    {
        gameSceneName = PlayerPrefs.GetString("LastSavedScene", gameSceneName);
    }
    public void StartGame()
    {
        Time.timeScale = 1f; 

        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}