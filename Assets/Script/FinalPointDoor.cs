using UnityEngine;
using System.Collections;

public class FinalPointDoor : MonoBehaviour
{
    [Header("Player Detection")]
    public string playerTag = "Player";

    [Header("Win UI")]
    public GameObject winPanel;
    public float fadeInDuration = 1.5f;

    private CanvasGroup winCanvasGroup;

    private bool hasWon;

    void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);

            winCanvasGroup = winPanel.GetComponent<CanvasGroup>();
            if (winCanvasGroup == null)
                winCanvasGroup = winPanel.AddComponent<CanvasGroup>();

            winCanvasGroup.alpha = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TryWin(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryWin(collision.gameObject);
    }

    void TryWin(GameObject other)
    {
        if (hasWon)
            return;

        // Handle setups where the collider is on a child object.
        Player player = other.GetComponent<Player>();
        if (player == null)
            player = other.GetComponentInParent<Player>();

        bool isPlayer = other.CompareTag(playerTag) || player != null;
        if (!isPlayer)
            return;

        hasWon = true;

        GameObject playerObject = player != null ? player.gameObject : other;

        // Stop player movement at finish.
        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        if (player != null)
            player.enabled = false;

        if (winPanel != null)
            StartCoroutine(FadeInWinPanel());
    }

    IEnumerator FadeInWinPanel()
    {
        winPanel.SetActive(true);

        if (winCanvasGroup == null)
        {
            winCanvasGroup = winPanel.GetComponent<CanvasGroup>();
            if (winCanvasGroup == null)
                winCanvasGroup = winPanel.AddComponent<CanvasGroup>();
        }

        winCanvasGroup.alpha = 0f;

        if (fadeInDuration <= 0f)
        {
            winCanvasGroup.alpha = 1f;
            yield break;
        }

        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            winCanvasGroup.alpha = Mathf.Clamp01(t / fadeInDuration);
            yield return null;
        }

        winCanvasGroup.alpha = 1f;
    }
}
