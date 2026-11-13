using UnityEngine;

public class AnimatedCursor : MonoBehaviour
{
    void Update()
    {
        Cursor.visible = false;
        
        transform.position = Input.mousePosition;
    }
}