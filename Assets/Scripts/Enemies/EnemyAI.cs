using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private float playerAwarenessDistance;
    [SerializeField] private AudioClip proximitySound; // Audio clip to play when player is near
    [SerializeField] private float proximitySoundVolume = 1f; // Volume of the proximity sound

    private bool canAttack = true;
    private bool awareOfPlayer;
    private Vector2 directionToPlayer;
    private AudioSource audioSource; // AudioSource component

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;
    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Transform player;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
        player = FindObjectOfType<PlayerController>().transform;
        audioSource = GetComponent<AudioSource>();

        // Initialize AudioSource settings
        audioSource.clip = proximitySound;
        audioSource.volume = proximitySoundVolume;
        audioSource.loop = true;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        UpdatePlayerAwareness();
        MovementStateControl();
    }

    private void UpdatePlayerAwareness()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;

        awareOfPlayer = enemyToPlayerVector.magnitude <= playerAwarenessDistance;

        if (awareOfPlayer && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!awareOfPlayer && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
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

        enemyPathfinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, player.position) < attackRange)
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
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

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

    public bool AwareOfPlayer
    {
        get { return awareOfPlayer; }
    }

    public Vector2 DirectionToPlayer
    {
        get { return directionToPlayer; }
    }
}
