using UnityEngine;

public class playAudioOnTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            audioSource.Play();
        }
    }
}