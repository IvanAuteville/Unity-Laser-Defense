using UnityEngine;

public class Enemy : MonoBehaviour, IPoolableComponent
{
    [Header("Properties")]
    [SerializeField] private int baseHealth = 300;
    [SerializeField] private int baseScore = 100;   

    [SerializeField] internal float minTimeBtwnShots = 0.2f;
    [SerializeField] internal float maxTimeBtwnShots = 2f;

    [Header("Prefabs")]
    [SerializeField] internal GameObject projectilePrefab = null;
    [SerializeField] private GameObject deathVFXPrefab = null;

    [Header("Sound")]
    [SerializeField] private AudioClip deathSound = null;
    [SerializeField] [Range(0f, 1f)] private float deathSoundVolume = 0.5f;

    internal float shotCounter = 0f;
    private Vector3 mainCamera;
    internal Player player = null;

    private int currentHealth = -1;
    private int currentScore = -1;
    internal float damageModifier = -1f;

    public virtual void Start()
    {
        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
        mainCamera = Camera.main.transform.position;
        player = FindObjectOfType<Player>();
    }

    public void Spawned()
    {
        currentHealth = baseHealth;
        currentScore = baseScore;
        damageModifier = 1f;
    }

    public virtual void Update()
    {
        CountDownAndShoot();
    }

    public virtual void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f && player != null)
        {
            Shoot();
            shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
        }
    }

    private void Shoot()
    {
        GameObject projectile = PrefabPoolingSystem.Spawn(projectilePrefab, transform.localPosition - new Vector3(0f, 0.7f, 0f), Quaternion.identity);

        projectile.GetComponent<DamageDealer>().IncreaseDamage(damageModifier);
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
        currentHealth -= damageDealer.GetDamage();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            AudioSource.PlayClipAtPoint(collision.GetComponent<Projectile>().GetImpactSound(),
                mainCamera, collision.GetComponent<Projectile>().GetImpactSoundVolume());


            PrefabPoolingSystem.Spawn(collision.GetComponent<Projectile>().GetImpactVFX(), transform.localPosition, Quaternion.identity);
        }
    }

    internal virtual void Die()
    {
        PrefabPoolingSystem.Spawn(deathVFXPrefab, transform.localPosition, Quaternion.identity);

        AudioSource.PlayClipAtPoint(deathSound, mainCamera, deathSoundVolume);

        GameSession.instance.AddToScore(currentScore);

        PrefabPoolingSystem.Despawn(gameObject);
    }

    virtual internal void IncreaseStats(float factor)
    {
        currentHealth = (int)(baseHealth * factor);
        currentScore = (int)(baseScore * factor);

        damageModifier = factor;
    }

    public void Despawned()
    {
    }
}