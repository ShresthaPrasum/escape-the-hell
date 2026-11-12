using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Animator animator;

    public GameObject playerObject;

    public string NextSceneName;

    public float timeToLoad = 1f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("OnCollision");
            playerObject.SetActive(false);
            
            Invoke("LoadNextScene", timeToLoad);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }

}