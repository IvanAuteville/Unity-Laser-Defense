using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<WaveConfig> waveConfigs = null;
    [SerializeField] private int startIndex = 0;
    [SerializeField] private float statsFactor = 1f;
    [SerializeField] private float cycleFactor = 0.25f;

    private int endIndex = -1;

    private void Start()
    {
        endIndex = waveConfigs.Count - 1;
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        int actualIndex = startIndex;

        while (true)
        {
            yield return StartCoroutine(SpawnAllEnemies(waveConfigs[actualIndex]));

            if (actualIndex == endIndex)
            {
                actualIndex = startIndex;
                statsFactor += cycleFactor;
            }
            else
            {
                actualIndex++;
            }
        }
    }

    private IEnumerator SpawnAllEnemies(WaveConfig waveConfig)
    {
        float timeBtwnSpawns = waveConfig.GetTimeBtwnSpawns();
        GameObject enemyPrefab = waveConfig.GetEnemyPrefab();
        Vector3 startPos = waveConfig.GetWayPoints()[0].transform.localPosition;

        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
            GameObject enemy = PrefabPoolingSystem.Spawn(enemyPrefab, startPos, Quaternion.identity);

            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            enemy.GetComponent<Enemy>().IncreaseStats(statsFactor);

            yield return new WaitForSeconds(timeBtwnSpawns);
        }
    }
}
