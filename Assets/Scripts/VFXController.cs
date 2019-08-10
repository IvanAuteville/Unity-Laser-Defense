using System.Collections;
using UnityEngine;

public class VFXController : MonoBehaviour, IPoolableComponent
{
    ParticleSystem particles = null;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Spawned()
    {
        particles.Play();

        if (!particles.main.loop)
        {
            _ = StartCoroutine(DespawnOneTimeParticle(particles.main.duration));
        }
    }

    IEnumerator DespawnOneTimeParticle(float time)
    {
        yield return new WaitForSeconds(time);

        PrefabPoolingSystem.Despawn(gameObject);
    }

    public void Despawned()
    {
        particles.Stop();
    }
}