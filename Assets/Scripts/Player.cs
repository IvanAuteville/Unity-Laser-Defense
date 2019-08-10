using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int health = 100;
    [SerializeField] private float xPadding = 1f;
    [SerializeField] private float yPadding = 1f;
    [SerializeField] private float yTopPadding = 3f;

    [Header("Projectiles")]
    [SerializeField] private GameObject projectile0Prefab = null;
    [SerializeField] private GameObject projectile1Prefab = null;
    [SerializeField] private GameObject projectile2Prefab = null;

    [Header("Fire Rates")]
    [SerializeField] private float projectileFireRate = 0.3f;
    [SerializeField] private float projectileLvl2FireRate = 0.3f;


    [Header("Death")]
    [SerializeField] private GameObject deathVFXPrefab = null;
    [SerializeField] private AudioClip deathSound = null;
    [SerializeField] [Range(0f, 1f)] private float deathSoundVolume = 0.5f;

    // Level Related
    private int playerLvl = 0;

    #pragma warning disable IDE0052 // Remove unread private members
    private Coroutine fireCoroutineLvl0 = null;
    private Coroutine fireCoroutineLvl2 = null;
    #pragma warning restore IDE0052 // Remove unread private members

    // Boundaries
    private float xMin = -1;
    private float xMax = -1;
    private float yMin = -1;
    private float yMax = -1;

    private Vector3 mainCameraPos;
    private HealthDisplay healthDisplay = null;

    // Movement
    private float newXPos;
    private float newYPos;
    private Vector2 newPos;

    private void Start()
    {
        SetUpBoundaries();

        healthDisplay = FindObjectOfType<HealthDisplay>();
        healthDisplay.SetHealth(health.ToString());



        GameSession.instance.SetPlayer(this);

        fireCoroutineLvl0 = StartCoroutine(FireContinuouslyLvl0());
        fireCoroutineLvl2 = StartCoroutine(FireContinuouslyLvl2());
    }

    private void Update()
    {
        Move();
    }

    private void SetUpBoundaries()
    {

        Camera gameCamera = Camera.main;
        mainCameraPos = gameCamera.transform.position;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - xPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)).y - yTopPadding;
    }

    private void Move()
    {
        newXPos = transform.localPosition.x + Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        newYPos = transform.localPosition.y + Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;

        newXPos = Mathf.Clamp(newXPos, xMin, xMax);
        newYPos = Mathf.Clamp(newYPos, yMin, yMax);

        newPos.Set(newXPos, newYPos);

        transform.localPosition = newPos;
    }

    private IEnumerator FireContinuouslyLvl0()
    {
        while (true)
        {
            if (playerLvl == 0)
            {
                FireLvl0();
            }

            if (playerLvl >= 1)
            {
                FireLvl1();
            }

            yield return new WaitForSeconds(projectileFireRate);
        }
    }

    private IEnumerator FireContinuouslyLvl2()
    {
        while (true)
        {
            switch (playerLvl)
            {
                case 2:
                    FireLvl2();
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(projectileLvl2FireRate);
        }
    }

    private void FireLvl0()
    {
        PrefabPoolingSystem.Spawn(projectile0Prefab, transform.localPosition + new Vector3(0f, 0.7f, 0f), Quaternion.identity);
    }

    private void FireLvl1()
    {
        PrefabPoolingSystem.Spawn(projectile1Prefab, transform.localPosition + new Vector3(-0.5f, 0.3f, 0f), Quaternion.identity);
        PrefabPoolingSystem.Spawn(projectile1Prefab, transform.localPosition + new Vector3(0.5f, 0.3f, 0f), Quaternion.identity);
    }

    private void FireLvl2()
    {
        PrefabPoolingSystem.Spawn(projectile2Prefab, transform.localPosition + new Vector3(0f, 0.7f, 0f), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            TakeDamage(damageDealer, collision);
        }
    }

    private void TakeDamage(DamageDealer damageDealer, Collider2D collision)
    {
        health -= damageDealer.GetDamage();

        if (health <= 0)
        {
            health = 0;

            Die();
        }
        else
        {
            PrefabPoolingSystem.Spawn(collision.GetComponent<Projectile>().GetImpactVFX(), transform.localPosition, Quaternion.identity);

            AudioSource.PlayClipAtPoint(collision.GetComponent<Projectile>().GetImpactSound(), mainCameraPos,
                collision.GetComponent<Projectile>().GetImpactSoundVolume());


            healthDisplay.SetHealth(health.ToString());
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;

            Die();
        }
        else
        {
            healthDisplay.SetHealth(health.ToString());
        }
    }

    private void Die()
    {
        GameObject deathVFX = Instantiate(deathVFXPrefab, transform.localPosition, Quaternion.identity);
        Destroy(deathVFX, 1f);

        healthDisplay.SetHealth(health.ToString());

        AudioSource.PlayClipAtPoint(deathSound, mainCameraPos, deathSoundVolume);

        LevelManager.instance.LoadGameOver();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    internal int GetHealth()
    {
        return health;
    }

    internal void LevelUp()
    {
        playerLvl++;
    }
}