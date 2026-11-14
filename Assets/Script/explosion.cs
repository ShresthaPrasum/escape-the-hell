using UnityEngine;

public class explosionEffector : MonoBehaviour
{
    
    public float explosionRadius = 3f;

    public KeyCode explodeKey = KeyCode.Q;

    public GameObject explosionParticlePrefab;


    void Update()
    {
        if (Input.GetKeyDown(explodeKey))
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius );
    
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