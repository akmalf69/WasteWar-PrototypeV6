using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private GameObject shopManagerUI;

    private bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        shopManagerUI.SetActive(false); // Ensure the shop UI is initially hidden
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            ToggleShop();
        }
    }

    private void ToggleShop()
    {
        shopManagerUI.SetActive(!shopManagerUI.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            shopManagerUI.SetActive(false);
        }
    }
}
