using System.Collections.Generic;
using UnityEngine;

public class PrefabPool
{
    private Dictionary<GameObject, PoolablePrefabData> activeList = null;
    private Queue<PoolablePrefabData> inactiveList = null;

    public PrefabPool()
    {
        activeList = new Dictionary<GameObject, PoolablePrefabData>();
        inactiveList = new Queue<PoolablePrefabData>();
    }

    public PrefabPool(int quantity)
    {
        if (quantity < 20)
        {
            quantity = 20;
        }

        activeList = new Dictionary<GameObject, PoolablePrefabData>(quantity);
        inactiveList = new Queue<PoolablePrefabData>(quantity);
    }

    public void Prespawn(GameObject prefab, Transform parent)
    {
        GameObject newGameObject = GameObject.Instantiate(prefab, parent);

        PoolablePrefabData data = new PoolablePrefabData
        {
            gameObject = newGameObject,
            poolableComponents = newGameObject.GetComponents<IPoolableComponent>()
        };

        data.gameObject.SetActive(false);
        inactiveList.Enqueue(data);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        PoolablePrefabData data;

        if (inactiveList.Count > 0)
        {
            data = inactiveList.Dequeue();
        }
        else
        {
            GameObject newGameObject = GameObject.Instantiate(prefab, position, rotation, parent);

            data = new PoolablePrefabData
            {
                gameObject = newGameObject,
                poolableComponents = newGameObject.GetComponents<IPoolableComponent>()
            };
        }

        // "Spawn" -------
        data.gameObject.SetActive(true);
        data.gameObject.transform.localPosition = position;
        data.gameObject.transform.localRotation = rotation;

        for (int i = 0; i < data.poolableComponents.Length; i++)
        {
            data.poolableComponents[i].Spawned();
        }

        activeList.Add(data.gameObject, data);
        // ----------------

        return data.gameObject;
    }

    public bool Despawn(GameObject objToDespawn)
    {
        if (!activeList.ContainsKey(objToDespawn))
        {
            Debug.LogWarning("This Object is not managed by this object pool!");
            return false;
        }

        PoolablePrefabData data = activeList[objToDespawn];

        for (int i = 0; i < data.poolableComponents.Length; ++i)
        {
            data.poolableComponents[i].Despawned();
        }

        data.gameObject.SetActive(false);

        activeList.Remove(objToDespawn);
        inactiveList.Enqueue(data);

        return true;
    }
}