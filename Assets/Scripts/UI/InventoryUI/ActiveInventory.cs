using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        // You may need to implement some logic here if you want to equip a starting weapon
    }

    public void EquipWeapon(EquippableItemSO equippableItem)
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        GameObject newWeapon = Instantiate(equippableItem.weaponPrefab, ActiveWeapon.Instance.transform);
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());

        // Update the inventory slot to reflect the equipped weapon
        // Assuming the active slot is always at index 0 for simplicity
        Transform childTransform = transform.GetChild(0);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        inventorySlot.SetEquippableItem(equippableItem);
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // Assuming the active slot is always at index 0 for simplicity
        Transform childTransform = transform.GetChild(0);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        EquippableItemSO equippableItem = inventorySlot.GetEquippableItem();

        if (equippableItem == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        EquipWeapon(equippableItem);
    }
}
