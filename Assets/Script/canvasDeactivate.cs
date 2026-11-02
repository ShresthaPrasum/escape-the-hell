using UnityEngine;

public class canvasDeactivate : MonoBehaviour
{
    public GameObject canvasToDeactivate;

    public void DeactivateCanvas()
    {
        if(canvasToDeactivate != null)
        {
            canvasToDeactivate.SetActive(false);
        }
    }
}