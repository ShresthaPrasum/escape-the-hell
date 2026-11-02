using System.Drawing;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private bool isActivated = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isActivated){
            PlayerRespawn respawnPlayer = collision.GetComponent<PlayerRespawn>();

            respawnPlayer.UpdateRespawnPosition(transform.position);

        

            CheckPoint[] allCheckpoints = FindObjectsOfType<CheckPoint>();

            foreach (CheckPoint cp in allCheckpoints)
            {
                cp.DeactivateCheckPoint();
            }
            
            isActivated = true;

        }

    }

    public void DeactivateCheckPoint()
    {
        isActivated =false;

    }

}