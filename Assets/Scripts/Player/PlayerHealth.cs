using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    public Transform respawnPoint;
    [SerializeField] private float deathDelay = 2.5f;
    [SerializeField] private AudioClip hitSoundEffect; // Reference to the hit sound effect

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    private Animator animator;
    private AudioSource audioSource; // Reference to the AudioSource component

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void AddHealth(int healthBoost)
    {
        int newHealth = currentHealth + healthBoost;
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        UpdateHealthSlider();
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();

        // Play the hit sound effect
        if (hitSoundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSoundEffect);
        }
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            animator.SetTrigger(DEATH_HASH);
            StartCoroutine(DeathRespawnRoutine());
        }
    }

    private IEnumerator DeathRespawnRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        isDead = false;
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        animator.ResetTrigger(DEATH_HASH);
        animator.Play("Idle");
        UpdateHealthSlider();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
