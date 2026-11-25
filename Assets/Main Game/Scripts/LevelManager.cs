using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentCurrency;
    public int enemyReward = 100;
    public TMPro.TMP_Text currencyText;

    void Start()
    {
        currentCurrency = 0;
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = "$" + currentCurrency.ToString();
        }
    }
}
