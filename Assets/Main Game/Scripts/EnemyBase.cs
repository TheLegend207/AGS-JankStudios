using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int currencyReward;
    private LevelManager levelManager;
    private bool isDead = false;

    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead == true)
        {
            return;
        }

        isDead = true;

        if (levelManager != null)
        {
            levelManager.AddCurrency(currencyReward);
        }

        Destroy(gameObject);
    }
}