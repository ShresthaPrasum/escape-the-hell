using UnityEngine;

using UnityEngine.EventSystems;

public class ButtonHoverIndicator : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    public GameObject glowIndicator;

    public AudioClip hoverAudioClip;

    public AudioSource audioSource;

    void OnEnable()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        
        if(glowIndicator != null)
        {
            glowIndicator.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        glowIndicator.SetActive(true);
        audioSource.PlayOneShot(hoverAudioClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glowIndicator.SetActive(false);
    }
}