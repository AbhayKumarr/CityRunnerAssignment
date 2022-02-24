using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{


public List<Pool> pools; 
private Dictionary<string, Queue<GameObject>> poolDictionary;


public static ObjectPooler Instance; // single instance reference
private void Awake()
{
      if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
        }
}



    void Start()
    {

        poolDictionary = new Dictionary<string, Queue<GameObject>>();        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i=0; i<pool.size; i++)
            {
                if(poolDictionary.ContainsKey(pool.tag))
                    continue;
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(gameObject.transform);      // making all instantiated gameobject under this gameObject.
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag,objectPool);
        }
    }

    GameObject objectToSpawn;
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {   
     
          objectToSpawn = poolDictionary[tag].Dequeue();
          objectToSpawn.transform.position = position;
          objectToSpawn.transform.rotation = rotation;
          objectToSpawn.SetActive(true);
          poolDictionary[tag].Enqueue(objectToSpawn); 
          return objectToSpawn;
    }


[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}



    
}
