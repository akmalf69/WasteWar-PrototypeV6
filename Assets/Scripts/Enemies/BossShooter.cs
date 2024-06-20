using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger must be enabled for oscillate to function properly.")]
    [SerializeField] private bool oscillate;

    [SerializeField] private EnemyHealth enemyHealth;

    private bool isShooting = false;

    private void OnValidate()
    {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

    public void Attack()
    {
        if (!isShooting)
        {
            if (enemyHealth.CurrentHealth > enemyHealth.StartingHealth / 2)
            {
                StartCoroutine(ShootPattern1());
            }
            else if (enemyHealth.CurrentHealth > enemyHealth.StartingHealth / 4)
            {
                StartCoroutine(ShootPattern2());
            }
            else
            {
                StartCoroutine(ShootPattern3());
            }
        }
    }

    private IEnumerator ShootPattern1()
    {
        yield return ShootPattern(burstCount, projectilesPerBurst, bulletMoveSpeed, angleSpread);
    }

    private IEnumerator ShootPattern2()
    {
        int pattern2BurstCount = burstCount + 1;
        int pattern2ProjectilesPerBurst = projectilesPerBurst + 2;
        float pattern2BulletMoveSpeed = bulletMoveSpeed + 2;
        float pattern2AngleSpread = angleSpread + 20f;

        yield return ShootPattern(pattern2BurstCount, pattern2ProjectilesPerBurst, pattern2BulletMoveSpeed, pattern2AngleSpread);
    }

    private IEnumerator ShootPattern3()
    {
        int pattern3BurstCount = burstCount + 2;
        int pattern3ProjectilesPerBurst = projectilesPerBurst + 4;
        float pattern3BulletMoveSpeed = bulletMoveSpeed + 4;
        float pattern3AngleSpread = angleSpread + 40f;

        yield return ShootPattern(pattern3BurstCount, pattern3ProjectilesPerBurst, pattern3BulletMoveSpeed, pattern3AngleSpread);
    }

    private IEnumerator ShootPattern(int burstCount, int projectilesPerBurst, float bulletMoveSpeed, float angleSpread)
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle, angleSpread);

        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle, angleSpread);
            }

            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle, angleSpread);
            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle, float specificAngleSpread = -1)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;

        float useAngleSpread = (specificAngleSpread >= 0) ? specificAngleSpread : angleSpread;

        if (useAngleSpread != 0)
        {
            angleStep = useAngleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = useAngleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
