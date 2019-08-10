using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour, IPoolableComponent
{
    private WaveConfig waveConfig = null;
    private List<Transform> waypoints = null;
    private Vector3 targetPos;

    private int waypointIndex = -1;
    private int finalIndex = -1;

    private float moveSpeed = -1f;
    private float movementThisFrame = -1f;

    private void Start()
    {
        waypoints = waveConfig.GetWayPoints();
        moveSpeed = waveConfig.GetMoveSpeed();
        finalIndex = waypoints.Count;

        transform.localPosition = waypoints[0].transform.localPosition;
        targetPos = waypoints[waypointIndex].transform.localPosition;
    }

    public void Spawned()
    {
        waypointIndex = 1;

        if (waypoints != null)
        {
            transform.localPosition = waypoints[0].transform.localPosition;
            targetPos = waypoints[waypointIndex].transform.localPosition;
        }
    }

    public void Despawned()
    {
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex < finalIndex)
        {
            movementThisFrame = moveSpeed * Time.deltaTime;

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, movementThisFrame);

            if (transform.localPosition == targetPos)
            {
                waypointIndex++;

                if (waypointIndex < finalIndex)
                {
                    targetPos = waypoints[waypointIndex].transform.localPosition;
                }
            }
        }
        else
        {
            PrefabPoolingSystem.Despawn(gameObject);
        }
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }
}