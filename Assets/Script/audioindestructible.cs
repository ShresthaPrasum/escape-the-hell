using UnityEngine;

public class audioindestructible : MonoBehaviour
{
    public static audioindestructible instance;

    private AudioSource audioSource;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

        }

    }

    public void PlayUISound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}