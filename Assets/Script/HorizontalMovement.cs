using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public bool useWave = true;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (useWave)
        {
            // Smooth oscillation using sine wave
            float newX = startPosition.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(newX, startPosition.y, startPosition.z);
        }
        else
        {
            // Linear left/right (more mechanical/piston-like)
            float cycle = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            transform.position = new Vector3(startPosition.x + cycle, startPosition.y, startPosition.z);
        }
    }
}
