using System;
using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    public event Action<int> OnGoldAmountChanged;

    private TMP_Text goldText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    private void Start()
    {
        InitializeGoldText();
        UpdateGoldUI();
    }

    private void InitializeGoldText()
    {
        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
            if (goldText != null)
            {
                goldText.text = currentGold.ToString("D3");
            }
        }
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = currentGold.ToString("D3");
        }
    }

    public void UpdateCurrentGold()
    {
        currentGold += 1;
        UpdateGoldUI();
        OnGoldAmountChanged?.Invoke(currentGold);
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }

    public void SetCurrentGold(int newGoldAmount)
    {
        currentGold = newGoldAmount;
        UpdateGoldUI();
        OnGoldAmountChanged?.Invoke(currentGold);
    }
}
