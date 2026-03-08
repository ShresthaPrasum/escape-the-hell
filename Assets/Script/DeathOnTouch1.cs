using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathOnTouch : MonoBehaviour
{
    [Header("Player Detection")]
    public string playerTag = "Player";

    [Header("Audio")]
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 0.7f;
    public float respawnDelay = 0.08f;

    private bool isRestarting;

    void OnTriggerEnter2D(Collider2D other)
    {
        TryKill(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryKill(collision.gameObject);
    }

    void TryKill(GameObject other)
    {
        if (isRestarting)
            return;

        bool isPlayerByTag = other.CompareTag(playerTag);
        Player playerScript = other.GetComponent<Player>();
        bool isPlayerByScript = playerScript != null;

        if (!isPlayerByTag && !isPlayerByScript)
            return;

        StartCoroutine(KillAndRespawn(other));
    }

    IEnumerator KillAndRespawn(GameObject player)
    {
        isRestarting = true;

        // Play death sound
        if (deathSound != null)
        {
            Play2DOneShot(deathSound, deathVolume);
        }
        else
        {
            Debug.LogWarning("Death sound is not assigned in the Inspector!");
        }

        if (respawnDelay > 0f)
            yield return new WaitForSeconds(respawnDelay);

        // Respawn at checkpoint instead of reloading scene
        if (Player.currentRespawnPoint != Vector3.zero)
        {
            player.transform.position = Player.currentRespawnPoint;
            
            // Reset velocity
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            
            isRestarting = false;
        }
        else
        {
            // Fallback: reload scene if no checkpoint set
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Play2DOneShot(AudioClip clip, float volume)
    {
        GameObject tempAudio = new GameObject("TempDeathSfx");
        AudioSource source = tempAudio.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = Mathf.Clamp01(volume);
        source.spatialBlend = 0f; // force 2D so listener distance does not mute it
        source.playOnAwake = false;
        source.loop = false;
        source.Play();

        Destroy(tempAudio, clip.length + 0.1f);
    }
}
