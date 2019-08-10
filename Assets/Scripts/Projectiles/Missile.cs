using UnityEngine;

public class Missile : Projectile
{
    [Header("Missile Properties")]
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float finalSpeed = 20f;
    [SerializeField] private float chaseTime = 0.7f;

    private float currentChaseTime = 0f;
    private Transform target = null;

    private new void Awake()
    {
        base.Awake();
        target = FindObjectOfType<Player>().transform;
    }

    public override void Spawned()
    {
        rigidBody.position = transform.localPosition;
        rigidBody.angularVelocity = 0f;

        currentChaseTime = chaseTime;

        AudioSource.PlayClipAtPoint(base.shootSound, base.mainCamera, base.shootSoundVolume);
    }

    private void FixedUpdate()
    {
        currentChaseTime -= Time.fixedDeltaTime;

        if (currentChaseTime > 0f && target)
        {
            Vector2 direction = (Vector2)target.localPosition - rigidBody.position;
            direction.Normalize();

            float rotationAmount = Vector3.Cross(direction, transform.up).z;

            rigidBody.angularVelocity = rotationAmount * rotationSpeed;

            rigidBody.velocity = -transform.up * moveSpeed;
        }
        else
        {
            rigidBody.velocity = rigidBody.velocity.normalized * finalSpeed;
        }
    }
}
