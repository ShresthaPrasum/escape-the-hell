using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer checkpointSprite;
    public Color activeColor = Color.green;
    public bool isActivated = false; 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            
           AutoSave(collision.gameObject);
        }
    }


    private void AutoSave(GameObject player)
    {
        isActivated = true;
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;


        PlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();

        if (playerRespawn != null)
        {
            playerRespawn.UpdateRespawnPosition(transform.position);
        }


        PlayerPrefs.SetFloat("PlayerX_" +sceneName ,transform.position.x);
        PlayerPrefs.SetFloat("PlayerY_" +sceneName ,transform.position.y);

        SlingshotPlayer slingshot = player.GetComponent<SlingshotPlayer>();

        PlayerPrefs.SetInt("MaxAirJumps_"+sceneName, slingshot.maxAirJumps);

        PlayerPrefs.SetInt("HasSaveData_"+ sceneName, 1);
        PlayerPrefs.SetString("LastSavedScene", sceneName);
        PlayerPrefs.Save();

        checkpointSprite.color = activeColor;

        PurgeOldCheckpoints();
    }

    private void PurgeOldCheckpoints()
    {
        Checkpoint[] allCheckpoints = FindObjectsByType<Checkpoint>();
        foreach (Checkpoint cp in allCheckpoints)
        {
            if (cp != this && cp.isActivated)
            {
                Destroy(cp.gameObject);
            }
        }
    }
}