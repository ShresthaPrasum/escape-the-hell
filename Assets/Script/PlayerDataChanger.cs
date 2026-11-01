using UnityEngine;
using UnityEngine.Events;

public class PlayerDataChanger : MonoBehaviour
{
    public Transform player;

    public SlingshotPlayer slingshotPlayer;
    public float interactDistance = 2.5f;
    
    public float requiredHoldTime = 2.0f;
    public UnityEvent onActivated;
    private float currentHoldTimer = 0f;
    private bool isActivated = false;

    void Update()
    {
        if (isActivated || player == null) return;

        
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactDistance)
        {
            
            if (Input.GetKey(KeyCode.E))
            {
                currentHoldTimer += Time.deltaTime;

                
                float progress = Mathf.Clamp01(currentHoldTimer / requiredHoldTime);
                

                
                if (currentHoldTimer >= requiredHoldTime)
                {
                    ActivateObject();
                }
            }
            else
            {
                currentHoldTimer = 0f;
            }
        }
        else
        {    
            currentHoldTimer = 0f;
        }
    }

    private void ActivateObject()
    {
        isActivated = true; 
        currentHoldTimer = 0f;
        slingshotPlayer.maxAirJumps = 1;
        // Debug.Log("Object Activated!");
        // onActivated?.Invoke(); 
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}