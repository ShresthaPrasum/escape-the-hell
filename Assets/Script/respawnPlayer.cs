using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{

    // Position = p;

    private Vector2 respawnP;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        respawnP = transform.position;
    }

    public void UpdateRespawnPosition(Vector2 newP)
    {
        respawnP= newP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathObject"))
        {
            respawnPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathObject"))
        {
            respawnPlayer();
        }
    }

    public void respawnPlayer()
    {
        transform.position = respawnP;

        rb.linearVelocity = Vector2.zero;

        rb.angularVelocity = 0f;
    }

}