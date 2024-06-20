using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private GameObject npcProjectilePrefab; // Reference to the npc projectile prefab
    [SerializeField] private int projectileDamage = 1; // Damage amount of the projectile
    [SerializeField] private string projectileTargetTag = "Boss"; // Tag of the projectile's target
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private float enemyAwarenessDistance;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private bool canAttack = true;
    private bool awareOfEnemy;
    private Vector2 directionToEnemy;
    private Rigidbody2D rb;
    private Vector2 targetDirection;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;
    private Transform enemy;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;
    private State state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = State.Roaming;
        enemy = FindObjectOfType<BossAI>().transform;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        UpdateEnemyAwareness();
        MovementStateControl();
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack) { return; }

        UpdateTargetDirection();
        RotateTowardsTarget();
        MoveCharacter();
    }

    private void UpdateEnemyAwareness()
    {
        Vector2 npcToEnemyVector = enemy.position - transform.position;
        directionToEnemy = npcToEnemyVector.normalized;

        awareOfEnemy = npcToEnemyVector.magnitude <= enemyAwarenessDistance;
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, enemy.position) < attackRange)
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, enemy.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            Attack(); // Call the Attack method to spawn the projectile
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void UpdateTargetDirection()
    {
        if (awareOfEnemy)
        {
            targetDirection = directionToEnemy;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        if (targetDirection == Vector2.zero)
        {
            return;
        }

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }

    private void MoveCharacter()
    {
        if (targetDirection != Vector2.zero)
        {
            moveDir = targetDirection.normalized;
        }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }

    public void StopMoving()
    {
        moveDir = Vector2.zero;
    }

    private void Attack()
    {
        if (npcProjectilePrefab != null)
        {
            GameObject projectile = Instantiate(npcProjectilePrefab, transform.position, Quaternion.identity);
            NpcProjectile npcProjectile = projectile.GetComponent<NpcProjectile>();
            if (npcProjectile != null)
            {
                npcProjectile.Initialize(projectileDamage, projectileTargetTag, enemy.position); // Pass enemy position
            }
            else
            {
                Debug.LogError("Npc projectile prefab does not have NpcProjectile component.");
            }
        }
        else
        {
            Debug.LogError("Npc projectile prefab is not assigned.");
        }
    }


    public bool AwareOfEnemy
    {
        get { return awareOfEnemy; }
    }

    public Vector2 DirectionToEnemy
    {
        get { return directionToEnemy; }
    }
}
