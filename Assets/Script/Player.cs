using UnityEngine;

public class SlingshotPlayer : MonoBehaviour
{
    public float launchPower = 14f;
    public float maxDragDistance = 3.5f;
    public int maxAirJumps = 1;
    public Transform groundCheck;
    public float checkRadius = 0.35f;
    public LayerMask groundlayer;
    public float stopSpeed = 10f;

    public LineRenderer pullLine;

    public float pullLineMulti = 1.5f;
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 25;
    public float timeStep = 0.05f;
    public float maxTrajectoryLength = 5f;
    public float airSlowMotion = 0.25f;
    public float maxSqueeze = 0f;
    public Animator anim; 

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 dragStartPos;
    private Vector2 currentLaunchVelocity;
    private bool isDragging = false;
    private bool isGrounded = false;
    private bool wasGrounded = false; 
    private int airJumpsLeft;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
        mainCam = Camera.main;
        originalScale = transform.localScale;
        airJumpsLeft = maxAirJumps;

        if (pullLine != null) pullLine.enabled = false;
        if (trajectoryLine != null) trajectoryLine.enabled = false;
    }

    void Update()
    {
        
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundlayer);

        
        
        if (!wasGrounded && isGrounded)
        {
            if (anim != null){
                anim.SetBool("Landed", true);
                anim.SetBool("InAir", false);
            }

        }
        
        else if (wasGrounded && !isGrounded)
        {
            if (anim != null){
                anim.SetBool("Landed", false);
                anim.SetBool("InAir", true);
            }
        }

        
        if (anim != null) anim.SetBool("isGrounded", isGrounded);

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
            float newX = Mathf.Lerp(rb.linearVelocity.x, 0f, Time.fixedDeltaTime * stopSpeed);
            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
    }

    void StartDrag()
    {
        isDragging = true;
        dragStartPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        
        if (anim != null){
            anim.SetBool("StartDrag", true);
            anim.SetBool("Release", false);
        }

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

        bool holdingCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (holdingCtrl)
        {
            if (pullLine != null) pullLine.enabled = false;

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = true;
                DrawTrajectory(transform.position, currentLaunchVelocity);
            }
        }
        else
        {
            if (trajectoryLine != null) trajectoryLine.enabled = false;

            if (pullLine != null)
            {
                pullLine.enabled = true;
                DrawPullLine(clampedPull);
            }
        }

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

        
        if (anim != null){
            anim.SetBool("Release", true);
            anim.SetBool("StartDrag", false);
        }

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
        
        Vector3 startPos = new Vector3(dragStartPos.x, dragStartPos.y, 0f);
        Vector3 pullEndPos = startPos - new Vector3(clampedPull.x*pullLineMulti, clampedPull.y*pullLineMulti, 0f);

        pullLine.SetPosition(0, startPos);
        pullLine.SetPosition(1, pullEndPos);
    }

    void DrawTrajectory(Vector2 startPos, Vector2 startVelocity)
    {
        if (trajectoryLine == null) return;

        Vector2 gravity = Physics2D.gravity * rb.gravityScale;
        trajectoryLine.positionCount = 1;
        trajectoryLine.SetPosition(0, startPos);

        float totalDistance = 0f;
        Vector2 prevPos = startPos;

        for (int i = 1; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;
            Vector2 pointPos = startPos + (startVelocity * t) + (0.5f * gravity * t * t);
            float segmentDist = Vector2.Distance(prevPos, pointPos);

            
            if (totalDistance + segmentDist >= maxTrajectoryLength)
            {
                float remaining = maxTrajectoryLength - totalDistance;
                Vector2 dir = (pointPos - prevPos).normalized;
                Vector2 clampedEndPos = prevPos + dir * remaining;

                trajectoryLine.positionCount = i + 1;
                trajectoryLine.SetPosition(i, clampedEndPos);
                break;
            }

            totalDistance += segmentDist;
            trajectoryLine.positionCount = i + 1;
            trajectoryLine.SetPosition(i, pointPos);
            prevPos = pointPos;
        }
    }

    void SetSlowMotion(bool active)
    {
        if (active)
        {
            Time.timeScale = airSlowMotion;
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