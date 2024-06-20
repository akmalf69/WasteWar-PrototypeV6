using Inventory.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopHealthManager : MonoBehaviour
{
    public TMP_Text goldText;
    public EdibleItemSO[] edibleitems;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtn;
    public InventorySO playerInventory;

    private int currentGold;

    private void Start()
    {
        for (int i = 0; i < edibleitems.Length; i++)
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
        for (int i = 0; i < edibleitems.Length; i++)
        {
            myPurchaseBtn[i].interactable = currentGold >= edibleitems[i].baseCost;
        }
    }

    public void PurchaseItem(int btnNo)
    {
        if (currentGold >= edibleitems[btnNo].baseCost)
        {
            currentGold -= edibleitems[btnNo].baseCost;
            goldText.text = currentGold.ToString();
            CheckPurchasable();

            playerInventory.AddItem(edibleitems[btnNo], 1);
            EconomyManager.Instance.SetCurrentGold(currentGold);
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < edibleitems.Length; i++)
        {
            shopPanels[i].titletext.text = edibleitems[i].titleShop;
            shopPanels[i].descriptionText.text = edibleitems[i].descriptionShop;
            shopPanels[i].priceText.text = edibleitems[i].baseCost.ToString();
            shopPanels[i].itemImage.sprite = edibleitems[i].itemImage;
        }
    }
}
