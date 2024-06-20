using Inventory.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeaponManager : MonoBehaviour
{
    public TMP_Text goldText;
    public EquippableItemSO[] equippableItems;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtn;
    public InventorySO playerInventory;

    private int currentGold;

    private void Start()
    {
        for (int i = 0; i < equippableItems.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }

        LoadPanels();
        EconomyManager.Instance.OnGoldAmountChanged += UpdateGoldText;
        UpdateGoldText(EconomyManager.Instance.GetCurrentGold());
        CheckPurchasable();
    }

    private void OnDestroy()
    {
        EconomyManager.Instance.OnGoldAmountChanged -= UpdateGoldText;
    }

    private void UpdateGoldText(int newGoldAmount)
    {
        currentGold = newGoldAmount;
        goldText.text = currentGold.ToString();
        CheckPurchasable();
    }

    public void AddGolds()
    {
        EconomyManager.Instance.UpdateCurrentGold();
    }

    public void CheckPurchasable()
    {
        for (int i = 0; i < equippableItems.Length; i++)
        {
            myPurchaseBtn[i].interactable = currentGold >= equippableItems[i].baseCost;
        }
    }

    public void PurchaseItem(int btnNo)
    {
        if (currentGold >= equippableItems[btnNo].baseCost)
        {
            currentGold -= equippableItems[btnNo].baseCost;
            goldText.text = currentGold.ToString();
            CheckPurchasable();

            playerInventory.AddItem(equippableItems[btnNo], 1);
            EconomyManager.Instance.SetCurrentGold(currentGold);
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < equippableItems.Length; i++)
        {
            shopPanels[i].titletext.text = equippableItems[i].titleShop;
            shopPanels[i].descriptionText.text = equippableItems[i].descriptionShop;
            shopPanels[i].priceText.text = equippableItems[i].baseCost.ToString();
            shopPanels[i].itemImage.sprite = equippableItems[i].itemImage;
        }
    }
}
