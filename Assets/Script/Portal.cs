using UnityEngine;

public class respawnPlayer : MonoBehaviour
{
    public Animator animator;

    public GameObject canvas;

    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("OnCollision");
            player.SetActive(false);
            canvas.SetActive(true);
        }
    }
}