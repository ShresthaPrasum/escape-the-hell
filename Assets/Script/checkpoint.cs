using UnityEngine;
using UnityEngine.Timeline;

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

        PlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();

        if (playerRespawn != null)
        {
            playerRespawn.UpdateRespawnPosition(transform.position);
        }

        isActivated = true;


        PlayerPrefs.SetFloat("PlayerX",transform.position.x);
        PlayerPrefs.SetFloat("PlayerY",transform.position.y);

        SlingshotPlayer slingshot = player.GetComponent<SlingshotPlayer>();

        PlayerPrefs.SetInt("MaxAirJumps", slingshot.maxAirJumps);

        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();

        checkpointSprite.color = activeColor;


    }
}