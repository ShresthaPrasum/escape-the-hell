using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
