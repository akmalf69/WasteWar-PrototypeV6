using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcProjectile : MonoBehaviour
{
    /*
    [SerializeField] private string projectileTargetTag = "Enemy";
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject splatterPrefab;

    private int damageAmount;
    private string targetTag;

    private void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
        Vector3 playerPos = PlayerController.Instance.transform.position;
        //Vector3 enemyPos = EnemyAI.Instance.transform.position;
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));
        //StartCoroutine(ProjectileCurveRoutine(transform.position, enemyPos));
        //StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, enemyPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }
        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }

        Destroy(grapeShadow);
    }

    public void Initialize(int damage, string target)
    {
        damageAmount = damage;
        targetTag = target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Destroy(gameObject); // Destroy projectile after hitting the target
            }
        }
    }
    */


    [SerializeField] private string projectileTargetTag = "Enemy";
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject splatterPrefab;

    private int damageAmount;
    private string targetTag;
    private Vector3 targetPosition;

    private void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, targetPosition));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, targetPosition));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            Vector3 currentPosition = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);
            transform.position = currentPosition;

            // Rotate the arrow to face the target
            Vector3 direction = endPosition - currentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            yield return null;
        }
        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }

        Destroy(grapeShadow);
    }

    public void Initialize(int damage, string target, Vector3 targetPos)
    {
        damageAmount = damage;
        targetTag = target;
        targetPosition = targetPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Projectile hit {other.name}");
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Projectile hit the target");
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("EnemyHealth component found, applying damage.");
                enemyHealth.TakeDamage(damageAmount, transform);
                Destroy(gameObject); // Destroy projectile after hitting the target
            }
            else
            {
                Debug.LogError("EnemyHealth component not found on target.");
            }
        }
        else
        {
            Debug.Log("Hit object is not the target");
        }
    }

}
