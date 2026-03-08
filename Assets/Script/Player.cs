using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Slingshot Settings")]
    public float launchForce = 25f;
    public float maxDragDistance = 4f;
    public float launchCooldown = 0f;
    public LineRenderer trajectoryLine;

    [Header("Squash & Stretch")]
    public float squashAmount = 1.2f;
    public float stretchAmount = 0.8f;
    public float squashDuration = 0.2f;

    [Header("Audio")]
    public AudioClip jumpSound;

    private Rigidbody2D rb;
    private Camera mainCam;
    private float cooldownTimer = 0f;
    private Vector3 normalScale;
    private float squashTimer = 0f;
    private AudioSource audioSource;

    // Slingshot state
    private bool isDragging;
    private Vector2 dragStart;

    // Respawn system
    public static Vector3 currentRespawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;  // Lock rotation
        mainCam = Camera.main;
        normalScale = transform.localScale;

        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Configure audio source for sound effects
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 0.3f;

        // Ensure there's an AudioListener in the scene (usually on camera)
        if (FindFirstObjectByType<AudioListener>() == null)
        {
            if (mainCam != null)
                mainCam.gameObject.AddComponent<AudioListener>();
            else
                Debug.LogWarning("No AudioListener found in scene. Sounds won't play!");
        }

        // Set initial respawn point to starting position
        if (currentRespawnPoint == Vector3.zero)
            currentRespawnPoint = transform.position;

        // Auto-create a trajectory line if none assigned
        if (trajectoryLine == null)
        {
            trajectoryLine = gameObject.AddComponent<LineRenderer>();
            trajectoryLine.startWidth = 0.1f;
            trajectoryLine.endWidth = 0.2f;
            trajectoryLine.material = new Material(Shader.Find("Sprites/Default"));
            trajectoryLine.startColor = Color.red;
            trajectoryLine.endColor = Color.yellow;
        }
        trajectoryLine.positionCount = 0;
    }

    void Update()
    {
        // Decrement cooldown
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // Update squash/stretch animation
        if (squashTimer > 0f)
        {
            squashTimer -= Time.deltaTime;
            float progress = squashTimer / squashDuration;
            float scale = Mathf.Lerp(1f, squashAmount, progress);
            transform.localScale = new Vector3(normalScale.x * scale, normalScale.y / scale, normalScale.z);
        }
        else
        {
            transform.localScale = normalScale;
        }

        // --- Slingshot mouse input ---
        if (Mouse.current == null) return;

        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            isDragging = true;
            dragStart = mouseWorld;
        }

        if (isDragging)
        {
            Vector2 dragCurrent = mouseWorld;
            Vector2 dragDelta = dragStart - dragCurrent;

            // Clamp drag distance
            if (dragDelta.magnitude > maxDragDistance)
                dragDelta = dragDelta.normalized * maxDragDistance;

            // Draw trajectory line from player in launch direction
            Vector2 launchDir = dragDelta.normalized;
            float power = (dragDelta.magnitude / maxDragDistance) * launchForce;

            trajectoryLine.positionCount = 2;
            trajectoryLine.SetPosition(0, (Vector2)transform.position);
            trajectoryLine.SetPosition(1, (Vector2)transform.position + launchDir * (power / launchForce) * 2f);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            trajectoryLine.positionCount = 0;

            Vector2 dragEnd = mouseWorld;
            Vector2 dragDelta = dragStart - dragEnd;

            if (dragDelta.magnitude > maxDragDistance)
                dragDelta = dragDelta.normalized * maxDragDistance;

            float power = (dragDelta.magnitude / maxDragDistance) * launchForce;
            Vector2 launchDir = dragDelta.normalized;

            // Launch the player in the opposite direction of the drag
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(launchDir * power, ForceMode2D.Impulse);

            // Play jump/launch sound
            if (jumpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
            else if (jumpSound == null)
            {
                Debug.LogWarning("Jump sound is not assigned in the Inspector!");
            }

            // Start cooldown
            cooldownTimer = launchCooldown;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TriggerSquashAnimation();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerSquashAnimation();
    }

    void TriggerSquashAnimation()
    {
        squashTimer = squashDuration;
    }
}
