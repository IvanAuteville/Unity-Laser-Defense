using System.Collections;
using UnityEngine;

public class EnemyLazer : Enemy
{
    [Header("Lazer Enemy Specific")]
    [SerializeField] private int baseLazerDamage = 2;

    [SerializeField] private float minShotDuration = 1f;
    [SerializeField] private float maxShotDuration = 2f;
    [SerializeField] private GameObject laserHitVFXPrefab = null;

    private int currentLazerDamage = -1;
    private LineRenderer lazer = null;
    private RaycastHit2D raycast;
    private AudioSource laserShotPlayer = null;
    private AudioSource laserHitPlayer = null;
    private GameObject laserHitVFX = null;

    private int layerMask = -1;

    public override void Start()
    {
        base.Start();

        laserShotPlayer = GetComponents<AudioSource>()[0];
        laserHitPlayer = GetComponents<AudioSource>()[1];

        lazer = projectilePrefab.GetComponent<LineRenderer>();
        lazer = Instantiate(lazer, transform);

        SetLaserPos();

        // Bitshift to raycast only whith the player
        int playerLayer = LayerMask.NameToLayer("Player");
        layerMask = 1 << playerLayer;
    }

    public new void Spawned()
    {
        base.Spawned();
        currentLazerDamage = baseLazerDamage;
    }

    public override void Update()
    {
        CountDownAndShoot();
        SetLaserPos();

        if (lazer.isVisible)
        {
            raycast = Physics2D.Raycast(transform.localPosition, Vector2.down, 20f, layerMask);

            if (raycast)
            {
                lazer.SetPosition(1, new Vector2(lazer.transform.position.x, player.transform.position.y - 0.05f));

                player.TakeDamage(currentLazerDamage);

                // SOUND
                if (!laserHitPlayer.isPlaying)
                {
                    laserHitPlayer.Play();
                }

                // VFX
                if (laserHitVFX == null)
                {
                    laserHitVFX = PrefabPoolingSystem.Spawn(laserHitVFXPrefab, player.transform.localPosition, Quaternion.identity);
                }
                else
                {
                    laserHitVFX.transform.localPosition = player.transform.localPosition;
                }
            }
            else
            {
                // SOUND
                if (laserHitPlayer.isPlaying)
                {
                    laserHitPlayer.Stop();
                }

                // VFX
                if (laserHitVFX != null)
                {
                    PrefabPoolingSystem.Despawn(laserHitVFX);
                    laserHitVFX = null;
                }
            }
        }
        else
        {
            if (laserHitVFX != null)
            {
                PrefabPoolingSystem.Despawn(laserHitVFX);
                laserHitVFX = null;
            }
        }
    }

    public override void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f && player != null)
        {
            StartCoroutine(ShootRoutine());

            shotCounter = float.MaxValue;
        }
    }

    private IEnumerator ShootRoutine()
    {
        lazer.enabled = true;
        laserShotPlayer.Play();

        yield return new WaitForSeconds(Random.Range(minShotDuration, maxShotDuration));

        lazer.enabled = false;
        laserShotPlayer.Stop();

        if (laserHitPlayer.isPlaying)
        {
            laserHitPlayer.Stop();
        }

        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
    }

    private void SetLaserPos()
    {
        lazer.SetPosition(0, transform.localPosition);
        lazer.SetPosition(1, transform.localPosition + new Vector3(0f, -20f, 0f));
    }

    internal override void IncreaseStats(float factor)
    {
        base.IncreaseStats(factor);
        currentLazerDamage = (int) (baseLazerDamage * damageModifier);
    }

    internal override void Die()
    {
        base.Die();
        
        if(laserHitVFX)
        {
            PrefabPoolingSystem.Despawn(laserHitVFX);
        }
    }
}