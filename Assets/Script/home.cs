using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "Level1"; 
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