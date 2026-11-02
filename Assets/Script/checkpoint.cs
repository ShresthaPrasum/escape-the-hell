using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActivated = false; 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            
            PlayerRespawn playerRespawn = collision.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.UpdateRespawnPosition(transform.position);
            }

            
            Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
            foreach (Checkpoint cp in allCheckpoints)
            {

                if (cp != this && cp.isActivated == true)
             {
                    Destroy(cp.gameObject);
                }
            }
            
            isActivated = true;
        }
    }
}