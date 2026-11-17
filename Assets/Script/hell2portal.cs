using UnityEngine;
using UnityEngine.SceneManagement;

public class hell2portal : MonoBehaviour
{
    public Animator animator;

    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && animator != null)
        {
            animator.SetTrigger("OnCollision");
            other.gameObject.SetActive(false);
            Invoke("LoadNextScene", 0.5f);
            
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}