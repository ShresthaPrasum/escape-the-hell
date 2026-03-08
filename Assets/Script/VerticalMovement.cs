using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 1f;
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
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
        else
        {
            // Linear up/down (more mechanical/piston-like)
            float cycle = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            transform.position = new Vector3(startPosition.x, startPosition.y + cycle, startPosition.z);
        }
    }
}
