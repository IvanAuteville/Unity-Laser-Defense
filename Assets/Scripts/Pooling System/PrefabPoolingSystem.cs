using System.Collections.Generic;
using UnityEngine;

public static class PrefabPoolingSystem
{
    private static Dictionary<GameObject, PrefabPool> prefabToPoolMap = new Dictionary<GameObject, PrefabPool>();
    private static Dictionary<GameObject, PrefabPool> goToPoolMap = new Dictionary<GameObject, PrefabPool>();

    public static GameObject Spawn(GameObject prefab, Transform parent = null)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
    }

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!prefabToPoolMap.ContainsKey(prefab))
        {
            prefabToPoolMap.Add(prefab, new PrefabPool());
        }

        PrefabPool pool = prefabToPoolMap[prefab];
        GameObject gameObject = pool.Spawn(prefab, position, rotation, parent);

        goToPoolMap.Add(gameObject, pool);

        return gameObject;
    }

    public static bool Despawn(GameObject gameObject)
    {
        if (!goToPoolMap.ContainsKey(gameObject))
        {
            Debug.LogWarning(string.Format("Object {0} not managed by poolsystem!", gameObject.name));
            return false;
        }

        PrefabPool pool = goToPoolMap[gameObject];
        if (pool.Despawn(gameObject))
        {
            goToPoolMap.Remove(gameObject);
            return true;
        }

        return false;
    }

    public static void Prespawn(GameObject prefab, int quantity, Transform parent = null)
    {
        if (!prefabToPoolMap.ContainsKey(prefab))
        {
            prefabToPoolMap.Add(prefab, new PrefabPool(quantity));
        }

        PrefabPool pool = prefabToPoolMap[prefab];

        for (int i = 0; i < quantity; i++)
        { 
            pool.Prespawn(prefab, parent);
        }
    }

    public static void Reset()
    {
        prefabToPoolMap.Clear();
        goToPoolMap.Clear();
    }
}