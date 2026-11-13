using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class portalPillar : MonoBehaviour
{

    public Transform player;

    public Image fillImage;

    public GameObject visualRoot;

    public float interactDistance =2.5f;

    public float requiredHoldTime = 2.0f;

    private float currentHoldTimer = 0f;

    private bool isActivated = false;
    public Animator animator;

    public GameObject gameObjectToActivate;

    public string NextSceneName;

    public float timeToLoad = 1f;


    void Start()
    {
        ResetHoldProgress();
    }

    void Update()
    {
        if(isActivated || player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= interactDistance)
        {
            if(visualRoot != null && !visualRoot.activeSelf)
            {
                visualRoot.SetActive(true);
            }

            if (Input.GetKey(KeyCode.E))
            {
                currentHoldTimer += Time.deltaTime;

                if(fillImage != null)
                {
                    fillImage.fillAmount = Mathf.Clamp01(currentHoldTimer/ requiredHoldTime);
                }

                if(currentHoldTimer >= requiredHoldTime)
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

            if(visualRoot != null && visualRoot.activeSelf)
            {
                visualRoot.SetActive(false);
            }
        }
    }

    private void ResetHoldProgress()
    {

        gameObjectToActivate.SetActive(false);
        currentHoldTimer = 0f;

        if(fillImage != null)
        {
            fillImage.fillAmount = 0f;
        }
    }

    private void ActivateObject()
    {
        isActivated = true;

        ResetHoldProgress();

        visualRoot.SetActive(false);

        gameObjectToActivate.SetActive(true);

        animator.SetBool("isActivated", true);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}