using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashRange = 5f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private TrailRenderer trailRenderer; // Reference to the TrailRenderer component

    private Transform player;
    private bool canDash = true;
    private bool isDashing = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            //Debug.LogError("Player not found in the scene.");
        }

        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
            if (trailRenderer == null)
            {
                //Debug.LogError("TrailRenderer component not found on the lizard.");
            }
        }

        // Disable trail at the start
        trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (player == null || !canDash) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        //Debug.Log($"Distance to player: {distanceToPlayer}");

        if (distanceToPlayer <= dashRange && !isDashing)
        {
            //Debug.Log("Player within dash range. Starting dash.");
            StartCoroutine(DashTowardsPlayer());
        }
    }

    private IEnumerator DashTowardsPlayer()
    {
        canDash = false;
        isDashing = true;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        //Debug.Log($"Dashing towards player. Direction: {directionToPlayer}");

        float dashEndTime = Time.time + dashDuration;

        // Enable trail during dash
        trailRenderer.emitting = true;

        while (Time.time < dashEndTime)
        {
            rb.velocity = directionToPlayer * dashSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        //Debug.Log("Dash complete.");

        // Disable trail after dash
        trailRenderer.emitting = false;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        //Debug.Log("Dash cooldown complete. Can dash again.");
    }
}
