using UnityEngine;
using TMPro;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public string playerTag = "Player";
    public string checkpointText = "Level 2";
    
    [Header("Fade Settings")]
    public float fadeInDuration = 1f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1f;
    public float textSize = 48f;

    private bool activated;
    private TextMeshProUGUI textUI;
    private Canvas canvas;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
            return;

        bool isPlayer = other.CompareTag(playerTag) || other.GetComponent<Player>() != null;
        
        if (!isPlayer)
            return;

        activated = true;

        // Update respawn point
        Player.currentRespawnPoint = transform.position;

        // Show fade text
        StartCoroutine(ShowFadeText());
    }

    IEnumerator ShowFadeText()
    {
        // Create canvas if it doesn't exist
        canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("CheckpointCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // Create text
        GameObject textObj = new GameObject("CheckpointText");
        textObj.transform.SetParent(canvas.transform, false);
        
        textUI = textObj.AddComponent<TextMeshProUGUI>();
        textUI.text = checkpointText;
        textUI.fontSize = textSize;
        textUI.alignment = TextAlignmentOptions.Center;
        textUI.color = new Color(1, 1, 1, 0);
        
        RectTransform rectTransform = textUI.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(800, 200);
        rectTransform.anchoredPosition = Vector2.zero;

        // Fade in
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            textUI.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / fadeOutDuration);
            textUI.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Destroy text
        Destroy(textObj);
    }
}
