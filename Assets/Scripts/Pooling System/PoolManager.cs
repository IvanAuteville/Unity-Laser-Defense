using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerProjectiles = null;
    [SerializeField] private GameObject[] enemiesProjectiles = null;
    [SerializeField] private GameObject[] VFX = null;
    [SerializeField] private GameObject[] enemies = null;

    private void Awake()
    {
        GameObject playerProjectilesGO = new GameObject("Player Projectiles");
        playerProjectilesGO.transform.SetParent(gameObject.transform);

        GameObject enemiesProjectilesGO = new GameObject("Enemies Projectiles");
        enemiesProjectilesGO.transform.SetParent(gameObject.transform);

        GameObject VFXGO = new GameObject("VFX");
        VFXGO.transform.SetParent(gameObject.transform);

        GameObject enemiesGO = new GameObject("Enemies");
        enemiesGO.transform.SetParent(gameObject.transform);

        // Player Projectiles
        PrefabPoolingSystem.Prespawn(playerProjectiles[0], 6, playerProjectilesGO.transform); 
        PrefabPoolingSystem.Prespawn(playerProjectiles[1], 12, playerProjectilesGO.transform);
        PrefabPoolingSystem.Prespawn(playerProjectiles[2], 4, playerProjectilesGO.transform);

        // Enemies Projectiles
        PrefabPoolingSystem.Prespawn(enemiesProjectiles[0], 20, enemiesProjectilesGO.transform);
        PrefabPoolingSystem.Prespawn(enemiesProjectiles[1], 20, enemiesProjectilesGO.transform);
        PrefabPoolingSystem.Prespawn(enemiesProjectiles[2], 20, enemiesProjectilesGO.transform);

        // VFX
        PrefabPoolingSystem.Prespawn(VFX[0], 10, VFXGO.transform);
        PrefabPoolingSystem.Prespawn(VFX[1], 10, VFXGO.transform);
        PrefabPoolingSystem.Prespawn(VFX[2], 10, VFXGO.transform);
        PrefabPoolingSystem.Prespawn(VFX[3], 10, VFXGO.transform);
        PrefabPoolingSystem.Prespawn(VFX[4], 10, VFXGO.transform);
        PrefabPoolingSystem.Prespawn(VFX[5], 10, VFXGO.transform);
        PrefabPoolingSystem.Prespawn(VFX[6], 10, VFXGO.transform);

        // Enemies
        PrefabPoolingSystem.Prespawn(enemies[0], 10, enemiesGO.transform);
        PrefabPoolingSystem.Prespawn(enemies[1], 10, enemiesGO.transform);
        PrefabPoolingSystem.Prespawn(enemies[2], 6, enemiesGO.transform);
        PrefabPoolingSystem.Prespawn(enemies[3], 6, enemiesGO.transform);
    }
}
