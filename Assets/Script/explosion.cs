using UnityEngine;

public class explosionEffector : MonoBehaviour
{
    
    public float explosionRadius = 3f;

    public KeyCode explodeKey = KeyCode.Q;

    public GameObject explosionParticlePrefab;

    public ParticleSystem explosionParticleSystem;

    public AudioSource explosionAudioSource;

    public AudioClip explosionAudioClip;


    void Update()
    {
        if(explosionAudioSource == null)
        {
            explosionAudioSource = gameObject.AddComponent<AudioSource>();
        }
        if (Input.GetKeyDown(explodeKey))
        {
            TriggerExplosion();
            explosionParticleSystem.Play();
            // explosionParticleSystem.Stop(true);
        }
    }

    private void TriggerExplosion()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius );

        explosionAudioSource.PlayOneShot(explosionAudioClip);
    
        foreach(Collider2D col in collidersInRange)
        {
            if (col.CompareTag("Explodable"))
            {
                
                
                if(explosionParticlePrefab != null)
                {
                    Instantiate(explosionParticlePrefab, col.transform.position, Quaternion.identity);
                }
                
                
                Destroy(col.gameObject);

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}