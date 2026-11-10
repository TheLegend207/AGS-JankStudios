using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CaptureSystem : MonoBehaviour
{
    public TMP_Text minigameText;
    public Image minigameBar;
    public Image minigameBarBackground;

    // Capture minigame settings
    public float increaseAmount = 0.08f;
    public float decreaseSpeed = 0.15f;

    private float captureProgress = 0f;

    private bool minigameActive = false;
    private GameObject targetEnemy;

    public bool IsMinigameActive()
    {
        return minigameActive;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!minigameActive)
            return;

        // Continuously decrease capture progress
        captureProgress -= decreaseSpeed * Time.deltaTime;

        // Increase progress when E is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            captureProgress += increaseAmount;
        }

        // Keep progress between 0 and 100%
        captureProgress = Mathf.Clamp01(captureProgress);

        // Update capture bar
        minigameBar.fillAmount = captureProgress;

        // Update percentage text
        minigameText.text = "Spam E to capture";

        // Successfully capture enemy
        if (captureProgress >= 1f)
        {
            EnemyBase capturedEnemy = targetEnemy.GetComponent<EnemyBase>();

            if (capturedEnemy != null)
            {
                capturedEnemy.Captured();
            }

            EndMinigame();
        }
    }

    // Called by Net.cs when net touches an enemy
    public void StartCapture(GameObject enemy)
    {
        if (minigameActive)
            return;

        targetEnemy = enemy;
        captureProgress = 0f;

        minigameActive = true;

        minigameText.gameObject.SetActive(true);
        minigameBar.gameObject.SetActive(true);
        minigameBarBackground.gameObject.SetActive(true);

        minigameBar.fillAmount = 0f;
        minigameText.text = "Spam E to capture";
    }

    void EndMinigame()
    {
        minigameActive = false;
        targetEnemy = null;

        minigameText.gameObject.SetActive(false);
        minigameBar.gameObject.SetActive(false);
        minigameBarBackground.gameObject.SetActive(false);
    }
}