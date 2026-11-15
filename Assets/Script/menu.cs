using UnityEngine;

using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject pausePanel;

    public Transform playerTransform;

    public SlingshotPlayer slingshot;

    public string mainMenuSceneName ="Menu";

    public AudioClip clickAudioClip;

    public AudioClip saveAudioClip;

    private bool isPaused = false;

    private string sceneName;

    void Start()
    {

        sceneName = SceneManager.GetActiveScene().name;

        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if(PlayerPrefs.GetInt("HasSaveData_"+ sceneName)!=1)
        {
            SaveGame();
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

    public void PlaySaveAudio()
    {
        audioindestructible.instance.PlayUISound(saveAudioClip);
    }
    
    public void SaveGame()
    {
        

        PlayerPrefs.SetFloat("PlayerX_"+ sceneName, playerTransform.position.x);
        PlayerPrefs.SetFloat("PlayerY_"+ sceneName, playerTransform.position.y);

        PlayerPrefs.SetInt("MaxAirJumps_" + sceneName, slingshot.maxAirJumps);

        PlayerPrefs.SetInt("HasSaveData_" + sceneName, 1);

        PlayerPrefs.SetString("LastSavedScene", sceneName);

        PlayerPrefs.Save();

        PlayerRespawn respawn = playerTransform.GetComponent<PlayerRespawn>();

        respawn.UpdateRespawnPosition(new Vector2(playerTransform.position.x, playerTransform.position.y));
    }    

    public void RestartGame()
    {
        audioindestructible.instance.PlayUISound(clickAudioClip);

        PlayerPrefs.DeleteKey("HasSaveData_"+ sceneName);

        PlayerPrefs.DeleteKey("PlayerX_"+ sceneName);

        PlayerPrefs.DeleteKey("PlayerY_"+ sceneName );

        PlayerPrefs.DeleteKey("MaxAirJumps_" + sceneName );

        PlayerPrefs.DeleteKey("DoubleJumpActivated_"+ sceneName);
        Time.timeScale =1f;

        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void ResumeGame()
    {
        audioindestructible.instance.PlayUISound(clickAudioClip);

        isPaused = false;

        pausePanel.SetActive(false);

        slingshot.enabled = true;

        Time.timeScale = 1f;

        Time.fixedDeltaTime = 0.02f;
    }

    public void GoToMainMenu()
    {
        audioindestructible.instance.PlayUISound(clickAudioClip);
        
        Time.timeScale = 1f;

        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void LoadSavedGame()
    {
        if(PlayerPrefs.GetInt("HasSaveData_"+ sceneName, 0)==1)
        {
            float x = PlayerPrefs.GetFloat("PlayerX_"+ sceneName);

            float y = PlayerPrefs.GetFloat("PlayerY_"+ sceneName);

            playerTransform.position = new Vector2(x, y);


            PlayerRespawn respawn = playerTransform.GetComponent<PlayerRespawn>();

            respawn.UpdateRespawnPosition(new Vector2(x, y));

            slingshot.maxAirJumps = PlayerPrefs.GetInt("MaxAirJumps_" + sceneName, slingshot.maxAirJumps);
        }
    }
}