using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int currencyReward;
    private LevelManager2 levelManager;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        levelManager = FindFirstObjectByType<LevelManager2>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Enemy died!");

        if (levelManager != null)
        {
            Debug.Log("LevelManager found! Giving currency: " + currencyReward);
            levelManager.AddCurrency(currencyReward);
        }
        else
        {
            Debug.LogError("LevelManager was not found!");
        }

        Destroy(gameObject);
    }
}
