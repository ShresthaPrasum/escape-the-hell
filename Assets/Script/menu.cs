using UnityEngine;

using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject pausePanel;

    public Transform playerTransform;

    public SlingshotPlayer slingshot;

    public string mainMenuSceneName ="Menu";

    private bool isPaused = false;

    void Start()
    {
        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        LoadSavedGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        isPaused = true;

        pausePanel.SetActive(true);
    
        slingshot.enabled = false;

        Time.timeScale = 0f;
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerX", playerTransform.position.x);
        PlayerPrefs.SetFloat("PlayerY", playerTransform.position.y);

        PlayerPrefs.SetInt("MaxAirJumps", slingshot.maxAirJumps);

        PlayerPrefs.SetInt("HasSaveData", 1);

        PlayerPrefs.Save();

        PlayerRespawn respawn = playerTransform.GetComponent<PlayerRespawn>();

        respawn.UpdateRespawnPosition(new Vector2(playerTransform.position.x, playerTransform.position.y));
    }    

    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("HasSaveData");

        PlayerPrefs.DeleteKey("PlayerX");

        PlayerPrefs.DeleteKey("PlayerY");

        PlayerPrefs.DeleteKey("MaxAirJumps");

        Time.timeScale =1f;

        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void ResumeGame()
    {
        isPaused = false;

        pausePanel.SetActive(false);

        slingshot.enabled = true;

        Time.timeScale = 1f;

        Time.fixedDeltaTime = 0.02f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void LoadSavedGame()
    {
        if(PlayerPrefs.GetInt("HasSaveData", 0)==1 && playerTransform != null)
        {
            float x = PlayerPrefs.GetFloat("PlayerX");

            float y = PlayerPrefs.GetFloat("PlayerY");

            playerTransform.position = new Vector2(x, y);


            PlayerRespawn respawn = playerTransform.GetComponent<PlayerRespawn>();

            respawn.UpdateRespawnPosition(new Vector2(x, y));

            slingshot.maxAirJumps = PlayerPrefs.GetInt("MaxAirJumps", slingshot.maxAirJumps);
        }
    }
}