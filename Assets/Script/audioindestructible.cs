using UnityEngine;

public class audioindestructible : MonoBehaviour
{
    private static audioindestructible instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}