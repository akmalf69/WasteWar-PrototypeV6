using System.Collections;
using UnityEngine;

public class Golem : MonoBehaviour, IEnemy
{
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isAttacking = false;

    [SerializeField] private float stopAttackDistance = 2f; // Distance to stop attacking
    [SerializeField] private Vector2 hitboxSize = new Vector2(1f, 1f); // Size of the hitbox
    [SerializeField] private Vector2 hitboxOffset = new Vector2(1f, 0f); // Offset of the hitbox

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = PlayerController.Instance.transform;
    }

    private void Update()
    {
        if (player == null) return;

        // Check the distance between the Golem and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < stopAttackDistance)
        {
            if (!isAttacking)
            {
                Attack();
            }
        }
        else
        {
            if (isAttacking)
            {
                StopAttack();
            }
        }
    }

    public void Attack()
    {
        // Trigger the attack animation
        myAnimator.SetTrigger(ATTACK_HASH);

        // Adjust the Golem's facing direction based on the player's position
        if (transform.position.x - player.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        isAttacking = true;
    }

    public void StopAttack()
    {
        // Reset the attack trigger to stop the attack animation
        myAnimator.ResetTrigger(ATTACK_HASH);
        myAnimator.SetTrigger("Idle"); // Assuming "Idle" is a valid trigger for the idle animation

        isAttacking = false;
    }

    // This method will be called by an animation event
    public void ApplyDamageAnimEvent()
    {
        // Perform the range check before applying damage
        if (player != null)
        {
            Vector2 hitboxPosition = (Vector2)transform.position + hitboxOffset * (spriteRenderer.flipX ? -1 : 1);
            Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPosition, hitboxSize, 0f);

            foreach (Collider2D hit in hits)
            {
                if (hit.transform == player)
                {
                    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(1, transform); // Pass the Golem's transform as the damage source
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 hitboxPosition = (Vector2)transform.position + hitboxOffset * (spriteRenderer.flipX ? -1 : 1);
        Gizmos.DrawWireCube(hitboxPosition, hitboxSize);
    }
}
