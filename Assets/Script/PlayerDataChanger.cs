using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerDataChanger : MonoBehaviour
{
    public Transform player;
    public Image fillImage;
    public GameObject visualRoot;

    public GameObject doubleJumpSignalVisual;
    public GameObject gameObjectToDeactivate;
    public SlingshotPlayer slingshotPlayer;
    public float interactDistance = 2.5f;
    public float requiredHoldTime = 2.0f;
    public UnityEvent onActivated;

    private float currentHoldTimer = 0f;
    private bool isActivated = false;

    void Start()
    {
        
        ResetHoldProgress();
    }

    void Update()
    {
        if (isActivated || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        
        if (distance <= interactDistance)
        {
            if (visualRoot != null && !visualRoot.activeSelf)
                visualRoot.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                currentHoldTimer += Time.deltaTime;
                
                
                if (fillImage != null)
                {
                    fillImage.fillAmount = Mathf.Clamp01(currentHoldTimer / requiredHoldTime);
                }

                if (currentHoldTimer >= requiredHoldTime)
                {
                    ActivateObject();
                }
            }
            else
            {
                
                ResetHoldProgress();
            }
        }
        else
        {
            
            ResetHoldProgress();
            if (visualRoot != null && visualRoot.activeSelf)
                visualRoot.SetActive(false);
        }
    }

    private void ResetHoldProgress()
    {
        currentHoldTimer = 0f;
        if (fillImage != null)
        {
            fillImage.fillAmount = 0f;
        }
    }

    private void ActivateObject()
    {
        isActivated = true;
        ResetHoldProgress();

        if (visualRoot != null)
        {
            visualRoot.SetActive(false);
        }

        if (gameObjectToDeactivate != null)
        {
            gameObjectToDeactivate.SetActive(false);
        }

        slingshotPlayer.maxAirJumps = 1;
        doubleJumpSignalVisual.SetActive(true);
        Debug.Log("Activated!");
        onActivated?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}