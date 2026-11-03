using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager2 : MonoBehaviour
{
    public int maxBaseHealth = 100;
    public int currentBaseHealth;
    public Image healthBar;
    void Start()
    {
        currentBaseHealth = maxBaseHealth;

    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentBaseHealth -= damage;
        UpdateHealthUI();

        if (currentBaseHealth <= 0)
        {
            GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10);
            Destroy(other.gameObject);
        }
    }

    private void UpdateHealthUI()
    {
        float healthPercentage = (float)currentBaseHealth / maxBaseHealth;

        // Store color that health UI should use
        Color healthColor;

        if (healthPercentage > 0.6f)
        {
            healthColor = Color.green;
        }
        else if (healthPercentage > 0.3f)
        {
            healthColor = Color.yellow;
        }
        else
        {
            healthColor = Color.red;
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = healthPercentage; // Change how full health bar is
            healthBar.color = healthColor; // Change bar color
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
