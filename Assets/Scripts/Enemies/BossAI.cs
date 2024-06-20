using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float playerAwarenessDistance = 10f;
    [SerializeField] private BossShooter bossShooter; // Reference to the BossShooter component
    [SerializeField] private GameObject attackWarningPrefab; // Prefab for warning indication (optional)
    [SerializeField] private float warningDuration = 1f; // Duration for which the warning is shown (if applicable)

    private bool canAttack = true;
    private bool awareOfPlayer;
    private Vector2 directionToPlayer;
    private Transform player;

    private enum State
    {
        Idle,
        Attacking
    }

    private State state;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }

        if (bossShooter == null)
        {
            bossShooter = GetComponent<BossShooter>();
            if (bossShooter == null)
            {
                Debug.LogError("BossShooter component not found on the boss.");
            }
        }
    }

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        UpdatePlayerAwareness();
        MovementStateControl();
    }

    private void UpdatePlayerAwareness()
    {
        if (player == null) return;

        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;

        awareOfPlayer = enemyToPlayerVector.magnitude <= playerAwarenessDistance;
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Idle()
    {
        if (awareOfPlayer && Vector2.Distance(transform.position, player.position) < attackRange)
        {
            state = State.Attacking;
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            state = State.Idle;
        }
        else if (canAttack)
        {
            canAttack = false;
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        if (attackWarningPrefab != null)
        {
            GameObject warning = Instantiate(attackWarningPrefab, player.position, Quaternion.identity);
            yield return new WaitForSeconds(warningDuration);
            Destroy(warning);
        }
        else
        {
            yield return new WaitForSeconds(warningDuration);
        }

        bossShooter.Attack();

        yield return new WaitForSeconds(attackCooldown - warningDuration);
        canAttack = true;
        state = State.Idle;
    }
}
