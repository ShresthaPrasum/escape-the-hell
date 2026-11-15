using UnityEngine;

using UnityEngine.EventSystems;

public class ButtonHoverIndicator : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    public GameObject glowIndicator;

    public AudioSource audioSource;

    public AudioClip hoverAudioClip;

    void OnEnable()
    {
        if(glowIndicator != null)
        {
            glowIndicator.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        glowIndicator.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glowIndicator.SetActive(false);
    }
}