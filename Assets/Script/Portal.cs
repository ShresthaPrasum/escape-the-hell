using UnityEngine;

using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Animator animator;

    public GameObject talkPortal;

    public GameObject player;

    public string NextSceneName;

    public float timeToLoad = 1f;

    public GameObject portal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("OnCollision");
            player.SetActive(false);
            
            Invoke("LoadNextScene", timeToLoad);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }
}