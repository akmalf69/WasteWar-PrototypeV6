using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private EnemyHealth bossHealth;

    private void Awake()
    {
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }

        if (bossHealth == null)
        {
            Debug.LogError("Boss health not assigned.");
        }
    }

    private void OnEnable()
    {
        // Subscribe to the boss health changed event
        bossHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        // Unsubscribe from the boss health changed event
        bossHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void Start()
    {
        healthSlider.maxValue = bossHealth.StartingHealth;
        healthSlider.value = bossHealth.CurrentHealth;
    }

    private void UpdateHealthBar()
    {
        if (bossHealth != null)
        {
            healthSlider.value = bossHealth.CurrentHealth;
        }
    }
}
