using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    private int currentHealth;
    private bool isInvincible = true; // Flag to make NPC invincible
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible) // Check if the NPC is invincible
        {
            currentHealth -= damage;
            knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
            StartCoroutine(flash.FlashRoutine());
            StartCoroutine(CheckDetectDeathRoutine());
        }
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>().DropItems();
            Destroy(gameObject);
        }
    }

    // Method to make the NPC invincible
    public void MakeInvincible(bool invincible)
    {
        isInvincible = invincible;
    }
}
