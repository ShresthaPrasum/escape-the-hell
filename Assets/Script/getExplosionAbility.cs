using UnityEngine;
using UnityEngine.UI;


public class getExplosionAbility : MonoBehaviour
{
    public Transform player;

    public Image fillImage;

    public GameObject visualRoot;

    public GameObject explosionAbilitySignalVisual;

    public GameObject gameObjectToDeactivate;

    public explosionEffector explosionAbility;

    public getExplosionAbility getExplosionAbilityy;

    public float interactDistance = 2.5f;

    public float requiredHoldTime = 2.0f;

    private float currentHoldTimer = 0f;

    public float waitTimeBeforeDeactivation = 1.0f;

    private bool isActivated = false;

    void Start()
    {
        if(PlayerPrefs.GetInt("ExplosionAbilityActivated", 0) == 1)
        {
            isActivated = true;

            visualRoot.SetActive(false);

            gameObjectToDeactivate.SetActive(false);

            explosionAbility.enabled = true;

            getExplosionAbilityy.enabled = false;

        }

        else
        {
            explosionAbility.enabled = false;
        }

        ResetHoldProgress();

        explosionAbilitySignalVisual.SetActive(false);
    }

    

    void Update()
    {
            if(isActivated || player == null)
        {
            return;
        }

        float distance  = Vector2.Distance(transform.position, player.position); 


        if(distance <= interactDistance)
        {
            if(visualRoot != null && !visualRoot.activeSelf)
            {
                visualRoot.SetActive(true);
            }

            if(Input.GetKey(KeyCode.E))
            {
                currentHoldTimer += Time.deltaTime;

                if(fillImage != null)
                {
                    fillImage.fillAmount = Mathf.Clamp01(currentHoldTimer / requiredHoldTime);
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
        currentHoldTimer = 0f;

        if(fillImage != null)
        {
            fillImage.fillAmount = 0f;
        }
    }

    private void ActivateObject()
    {
        PlayerPrefs.SetInt("ExplosionAbilityActivated", 1);
        
        PlayerPrefs.Save();

        isActivated =true;
        
        ResetHoldProgress();

        visualRoot.SetActive(false);

        explosionAbilitySignalVisual.SetActive(true);

        explosionAbility.enabled = true;
    }


    public void deactivate()
    {
        gameObjectToDeactivate.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}

