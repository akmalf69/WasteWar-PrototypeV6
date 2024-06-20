using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private bool isBoss = false; // Flag to distinguish bosses
    [SerializeField] private string cutsceneSceneName; // Name of the cutscene scene

    private int currentHealth;
    private bool hasTriggeredCutscene = false;
    private Knockback knockback;
    private Flash flash;

    public int CurrentHealth => currentHealth;
    public int StartingHealth => startingHealth;

    public event Action OnHealthChanged;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage, Transform damageSource)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(); // Notify listeners about health change

        if (!isBoss) // Prevent knockback if it's a boss
        {
            knockback.GetKnockedBack(damageSource, knockBackThrust);
        }

        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());

        // Check if the boss's health is at or below half and trigger cutscene if it hasn't been triggered
        if (isBoss && !hasTriggeredCutscene && currentHealth <= startingHealth / 2)
        {
            hasTriggeredCutscene = true;
            StartCoroutine(TriggerCutsceneRoutine());
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

    private IEnumerator TriggerCutsceneRoutine()
    {
        // Play cutscene animation here (if any)
        // For simplicity, just wait for a few seconds
        yield return new WaitForSeconds(2f); // Adjust the wait time for your cutscene duration

        // Change the scene to the cutscene scene
        SceneManager.LoadScene(cutsceneSceneName);
    }

    internal void TakeDamage(int damageAmount)
    {
        throw new NotImplementedException();
    }
}
