using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public Camera mainCamera;

    public Transform groundCheck;

    public LayerMask groundLayer;

    public float groundRadius = 0.2f;

    public bool allowAirAiming = true;

    public float maxPullDistance = 3f;

    public float launchSpeedPerUnit = 5f;

    public float maxLaunchSpeed = 15f;

    public float minimumPullDistance = 0.25f;

    public float momentum = 0.5f;
    
    public LineRenderer aimLine;

    public float maxAimLineLength = 4f;

    public float aimStartWidth = 0.18f;

    public float aimEndWidth = 0.015f;

    public LineRenderer aimtrajectoryLine;

    public int aimtrajectorySteps = 30;

    public float aimtrajectoryDuration = 1.5f;

    public bool IsAiming;

    private Vector2 aimOrigin;

    private Vector2 dragStartWorld;

    private Vector2 oldVelocity;

    private float oldGravity;

    private float normalGravity;
    public Vector2 checkPoint;
       

    private void Awake()
    {
        if(rb== null)
        {
            rb =GetComponent<Rigidbody2D>();
        }

        mainCamera = Camera.main;

        normalGravity = rb.gravityScale;

        checkPoint = rb.position;

        if(aimLine != null)
        {
            aimLine.useWorldSpace = true;
        }

        if(aimtrajectoryLine != null)
        {
            aimtrajectoryLine.useWorldSpace =true;
        }



    }

    private void Update()
    {
        if(Keyboard.current != null & Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetPlayer();
            return;
        }

        Vector2 mouseWorld = GetMouseWorld();

        if(!IsAiming && Mouse.current.leftButton.wasPressedThisFrame && IsGrounded())
        {
            BeginAim(mouseWorld);
        }

        if (IsAiming)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                rb.linearVelocity = Vector2.zero;

                rb.angularVelocity = 0f;

                rb.gravityScale = 0f;

                DrawAim(mouseWorld);

                if (isCtrlHeld())
                {
                    DrawTrajectory();
                }

                else if( aimtrajectoryLine != null)
                {
                    aimtrajectoryLine.enabled = false;
                }

                
            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Launch(mouseWorld);
            }
        }
    }

    private Vector2 GetMouseWorld()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();

        float distance = -mainCamera.transform.position.z;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(
            screenPosition.x,
            screenPosition.y,
            distance
        ));

        return new Vector2(
            worldPosition.x,
            worldPosition.y
        );
    }

    private void BeginAim(Vector2 mouseWorld)
    {
        IsAiming = true;

        aimOrigin =rb.position;

        dragStartWorld = mouseWorld;

        oldVelocity = rb.linearVelocity;
        oldGravity = rb.gravityScale;
    }

    private void DrawAim(Vector2 mouseWorld)
    {
        Vector2 dragVector = mouseWorld - dragStartWorld;

        Vector2 limitedEnd = dragStartWorld + Vector2.ClampMagnitude(
            dragVector, Mathf.Max(0f, maxAimLineLength)
        );

        aimLine.enabled = true;
        aimLine.positionCount = 2;

        aimLine.SetPosition(0, dragStartWorld);
        aimLine.SetPosition(1, limitedEnd);
    }

    private void UpdateAimLineStyle()
    {
        aimLine.widthMultiplier= 1f;

        aimLine.widthCurve = new AnimationCurve(
            new Keyframe(0f, Mathf.Max(0.001f, aimStartWidth)),
            new Keyframe(1f, Mathf.Max(0.001f, aimEndWidth))
        );

        aimLine.numCapVertices = 5;
    }

    private bool isCtrlHeld()
    {
        return Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
    }
    
    private Vector2 GetLaunchVelocity(Vector2 pull)
    {
        if(pull.magnitude< minimumPullDistance)
        {
            return Vector2.zero;
        }

        float speed = Mathf.Min(pull.magnitude * launchSpeedPerUnit, maxLaunchSpeed);

        Vector2 launchVelocity = pull.normalized * speed;

        launchVelocity += oldVelocity * momentum;

        return launchVelocity;
    }

    private void DrawTrajectory()
    {
        if(aimtrajectoryLine == null || aimtrajectorySteps < 2)
        {
            return;
        }

        Vector2 pull = Vector2.ClampMagnitude(aimOrigin - GetMouseWorld(),
        maxPullDistance);

        Vector2 launchVelocity = GetLaunchVelocity(pull);

        Vector2 gravity = Physics2D.gravity * oldGravity;

        aimtrajectoryLine.enabled = true;
        aimtrajectoryLine.positionCount = aimtrajectorySteps;

        for (int i = 0; i < aimtrajectorySteps; i++)
        {
            float time = i / (float)(aimtrajectorySteps - 1)* aimtrajectoryDuration;
            Vector2 point =aimOrigin+ launchVelocity * time+ 0.5f * gravity * time * time;

            aimtrajectoryLine.SetPosition(i, point);
        }
        
    }

    private void Launch(Vector2 mouseWorld)
    {
        Vector2 pull = Vector2.ClampMagnitude(
            aimOrigin - mouseWorld,
            maxPullDistance
        );

        rb.gravityScale = oldGravity;
        rb.linearVelocity = GetLaunchVelocity(pull);

        IsAiming = false;

        HideLines();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }
    private void HideLines()
    {
        if(aimLine != null)
        {
            aimLine.enabled = false;
        }

        if(aimtrajectoryLine != null)
        {
            aimtrajectoryLine.enabled= false;
        }
    }
    private void ResetPlayer()
    {
        IsAiming =false;

        rb.gravityScale = normalGravity;

        rb.position = checkPoint;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        HideLines();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundRadius
        );
    }

    private void Onalidate()
    {
        UpdateAimLineStyle();      
    }



}
