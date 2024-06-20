using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestS : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>())
        {
            audioManager.PlaySFX(audioManager.chestBreak);
            GetComponent<CoinChestS>().DropCoins();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
