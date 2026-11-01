using UnityEngine;

public class SlingshotPlayer : MonoBehaviour
{
    public float launchPower = 14f;
    public float maxDragDistance = 3.5f;
    public int maxAirJumps = 1;
    public Transform groundCheck;
    public float checkRadius = 0.35f;
    public LayerMask whatIsGround;
    public float groundStopSpeed = 10f;

    public LineRenderer pullLine;
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 25;
    public float timeStep = 0.05f;
    public float airSlowMoScale = 0.25f;
    public float maxSqueeze = 0.3f;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 dragStartPos;
    private Vector2 currentLaunchVelocity;
    private bool isDragging = false;
    private bool isGrounded = false;
    private int airJumpsLeft;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        originalScale = transform.localScale;
        airJumpsLeft = maxAirJumps;

        if (pullLine != null) pullLine.enabled = false;
        if (trajectoryLine != null) trajectoryLine.enabled = false;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            airJumpsLeft = maxAirJumps;
        }

        if (Input.GetMouseButtonDown(0) && (isGrounded || airJumpsLeft > 0))
        {
            StartDrag();
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            UpdateDrag();
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            ReleaseLaunch();
        }

        if (!isDragging)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.unscaledDeltaTime * 12f);
        }
    }

    void FixedUpdate()
    {
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            float newX = Mathf.Lerp(rb.linearVelocity.x, 0f, Time.fixedDeltaTime * groundStopSpeed);
            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
    }

    void StartDrag()
    {
        isDragging = true;
        dragStartPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (pullLine != null) pullLine.enabled = true;
        if (trajectoryLine != null) trajectoryLine.enabled = true;

        if (!isGrounded)
        {
            SetSlowMotion(true);
        }
    }

    void UpdateDrag()
    {
        Vector2 currentMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rawPull = dragStartPos - currentMousePos;
        Vector2 clampedPull = Vector2.ClampMagnitude(rawPull, maxDragDistance);

        currentLaunchVelocity = clampedPull * launchPower;

        DrawPullLine(clampedPull);
        DrawTrajectory(transform.position, currentLaunchVelocity);
        ApplySqueeze(clampedPull.magnitude / maxDragDistance);

        if (!isGrounded)
        {
            SetSlowMotion(true);
        }
    }

    void ReleaseLaunch()
    {
        isDragging = false;
        SetSlowMotion(false);

        if (pullLine != null) pullLine.enabled = false;
        if (trajectoryLine != null) trajectoryLine.enabled = false;

        if (currentLaunchVelocity.magnitude > 1.5f)
        {
            if (!isGrounded)
            {
                airJumpsLeft--;
            }

            rb.linearVelocity = currentLaunchVelocity;
            transform.localScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.25f, originalScale.z);
        }
    }

    void DrawPullLine(Vector2 clampedPull)
    {
        if (pullLine == null) return;

        pullLine.positionCount = 2;
        Vector3 playerPos = transform.position;
        Vector3 pullEndPos = playerPos - (Vector3)clampedPull;

        pullLine.SetPosition(0, playerPos);
        pullLine.SetPosition(1, pullEndPos);
    }

    void DrawTrajectory(Vector2 startPos, Vector2 startVelocity)
    {
        if (trajectoryLine == null) return;

        trajectoryLine.positionCount = trajectoryPoints;
        Vector2 gravity = Physics2D.gravity * rb.gravityScale;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;
            Vector2 pointPos = startPos + (startVelocity * t) + (0.5f * gravity * t * t);
            trajectoryLine.SetPosition(i, pointPos);
        }
    }

    void SetSlowMotion(bool active)
    {
        if (active)
        {
            Time.timeScale = airSlowMoScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    void ApplySqueeze(float pullPercent)
    {
        float scaleX = originalScale.x * (1f + (pullPercent * maxSqueeze));
        float scaleY = originalScale.y * (1f - (pullPercent * maxSqueeze));
        transform.localScale = new Vector3(scaleX, scaleY, originalScale.z);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}