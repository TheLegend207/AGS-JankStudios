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
    public int currentCurrency;
    public int enemyReward = 10;
    public TMPro.TMP_Text currencyText;
    void Start()
    {
        currentBaseHealth = maxBaseHealth;
        currentCurrency = 0;
        currencyText.text = currentCurrency.ToString();

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

            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.Die();
            }
            else
            {
                Destroy(other.gameObject);
            }
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

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        Debug.Log("Currency updated: " + currentCurrency);
        currencyText.text = currentCurrency.ToString();
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
