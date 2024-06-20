using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    // Add AudioClip fields
    [SerializeField] private AudioClip grassWalkSound;
    [SerializeField] private AudioClip cosmeticsWalkSound;
    [SerializeField] private AudioClip foregroundCollisionSound;
    [SerializeField] private AudioClip waterSound;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;
    private AudioSource audioSource;

    private bool facingLeft = false;
    private bool isDashing = false;
    private bool isWalkingOnGrass = false;
    private bool isWalkingOnCosmetics = false;
    private bool isNearWater = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        audioSource = GetComponent<AudioSource>(); // Ensure an AudioSource component is attached
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
        PlayWalkingSound();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private void PlayWalkingSound()
    {
        if (movement.magnitude > 0 && !audioSource.isPlaying)
        {
            if (isWalkingOnGrass)
            {
                audioSource.PlayOneShot(grassWalkSound);
                Debug.Log("Playing grass walk sound");
            }
            else if (isWalkingOnCosmetics)
            {
                audioSource.PlayOneShot(cosmeticsWalkSound);
                Debug.Log("Playing cosmetics walk sound");
            }
            else if (isNearWater)
            {
                audioSource.PlayOneShot(waterSound);
                Debug.Log("Playing water sound");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Foreground"))
        {
            audioSource.PlayOneShot(foregroundCollisionSound);
            Debug.Log("Playing foreground collision sound");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            isWalkingOnGrass = true;
            Debug.Log("Player entered grass");
        }
        else if (other.CompareTag("Cosmetics"))
        {
            isWalkingOnCosmetics = true;
            Debug.Log("Player entered cosmetics");
        }
        else if (other.CompareTag("Water"))
        {
            isNearWater = true;
            Debug.Log("Player entered water area");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            isWalkingOnGrass = false;
            Debug.Log("Player exited grass");
        }
        else if (other.CompareTag("Cosmetics"))
        {
            isWalkingOnCosmetics = false;
            Debug.Log("Player exited cosmetics");
        }
        else if (other.CompareTag("Water"))
        {
            isNearWater = false;
            Debug.Log("Player exited water area");
        }
    }
}
