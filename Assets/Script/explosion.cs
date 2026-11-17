using UnityEngine;
using Unity.Cinemachine;

public class explosionEffector : MonoBehaviour
{
    
    public float explosionRadius = 3f;

    public KeyCode explodeKey = KeyCode.Q;

    public GameObject rootExplosionParticlePrefab;

    public GameObject rockExplosionParticlePrefab;

    public ParticleSystem explosionParticleSystem;

    public AudioSource explosionAudioSource;

    public AudioClip explosionAudioClip;


    [Header("Cooldown")]
    public float cooldownTime = 2f;

    private float nextExplosionTime = 0f;


    [Header("Targeter")]

    public string[] targeter = {"RockExplodable", "RootExplodable"};


    [Header("Shake")]

    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        if (explosionAudioSource == null)
        {
            explosionAudioSource = gameObject.AddComponent<AudioSource>();
        }

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    void Update()
    {
        if (Input.GetKeyDown(explodeKey) && Time.time >= nextExplosionTime)
        {
            TriggerExplosion();
            explosionParticleSystem.Play();
            // explosionParticleSystem.Stop(true);

            nextExplosionTime = Time.time + cooldownTime;
        }
    }

    private void TriggerExplosion()
    {
        if(impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius );

        explosionAudioSource.PlayOneShot(explosionAudioClip);
    
        foreach(Collider2D col in collidersInRange)
        {
            if (HasTargetTag(col))
            {
                
                
                if(col.CompareTag("ExplodableRoot") && rockExplosionParticlePrefab != null)
                {
                    Instantiate(rootExplosionParticlePrefab, col.transform.position, Quaternion.identity);
                }

                else if(col.CompareTag("ExplodableRock") && rootExplosionParticlePrefab != null)
                {
                    Instantiate(rockExplosionParticlePrefab, col.transform.position, Quaternion.identity);
                }
                
                Destroy(col.gameObject);

            }
        }
    }

    private bool HasTargetTag(Collider2D col)
    {
        foreach(string tag in targeter)
        {
            if(col.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}