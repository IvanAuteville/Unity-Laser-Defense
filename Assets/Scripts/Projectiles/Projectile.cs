using UnityEngine;

public class Projectile : MonoBehaviour, IPoolableComponent
{
    [SerializeField] internal float moveSpeed = 20f;

    [SerializeField] internal AudioClip shootSound = null;
    [SerializeField] [Range(0f, 1f)] internal float shootSoundVolume = 0.5f;

    [SerializeField] private AudioClip impactSound = null;
    [SerializeField] [Range(0f, 1f)] private float impactSoundVolume = 0.5f;

    [SerializeField] private GameObject impactVFX = null;

    internal Rigidbody2D rigidBody = null;
    internal Vector3 mainCamera;

    internal void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main.transform.localPosition;
    }

    // "Start"
    public virtual void Spawned()
    {
        rigidBody.velocity = new Vector2(0f, moveSpeed);
        AudioSource.PlayClipAtPoint(shootSound, mainCamera, shootSoundVolume);
    }

    // "Destroy"
    public void Despawned()
    {
    }

    private void OnTriggerEnter2D()
    {
        PrefabPoolingSystem.Despawn(gameObject);
    }

    internal AudioClip GetImpactSound()
    {
        return impactSound;
    }

    internal float GetImpactSoundVolume()
    {
        return impactSoundVolume;
    }

    internal GameObject GetImpactVFX()
    {
        return impactVFX;
    }
}