using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FusionFuryGame
{
    public class ObjectPoolManager : MonoBehaviour
    {
        // Static instance of the ObjectPoolManager
        private static ObjectPoolManager instance;

        // Property to access the ObjectPoolManager instance
        public static ObjectPoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    // Find the ObjectPoolManager instance in the scene
                    instance = FindObjectOfType<ObjectPoolManager>();

                    // If not found, create a new GameObject and add the ObjectPoolManager script to it
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("ObjectPoolManager");
                        instance = obj.AddComponent<ObjectPoolManager>();
                    }
                }
                return instance;
            }
        }


        // Optional: Ensure the ObjectPoolManager persists between scenes
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Define a dictionary to store object pools
        private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

        // Create or retrieve an object from the pool based on the name of it
        public GameObject GetPooledObject(string objectName)
        {
            if (objectPools.ContainsKey(objectName))
            {
                while (objectPools[objectName].Count > 0)
                {
                    GameObject obj = objectPools[objectName].Dequeue();
                    if (obj && !obj.activeInHierarchy) // Check if the object is not active in the hierarchy
                    {
                        obj.SetActive(true);
                        return obj;
                    }
                }
                // If no inactive objects are available, extend the pool and return a new object
                return ExtendPool(objectName, 1);
            }

            Debug.LogWarning("No object pool exists with the name: " + objectName);
            return null;
        }

        // Return an object to the pool
        public void ReturnToPool(string objectName, GameObject obj)
        {
            obj.SetActive(false);
            objectPools[objectName].Enqueue(obj);

        }

        // Create an object pool for a specific prefab so I can dynamically add object to the pool in runtime
        public void CreateObjectPool(GameObject prefab, int poolSize, string objectName, Transform parent)
        {
            string tag = objectName;

            if (!objectPools.ContainsKey(tag))
            {
                objectPools[tag] = new Queue<GameObject>();

                for (int i = 0; i < poolSize; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.transform.parent = transform;
                    obj.SetActive(false);
                    objectPools[tag].Enqueue(obj);
                }
            }
            else
            {
                Debug.LogWarning("Object pool with tag " + tag + " already exists.");
            }
        }


        private GameObject ExtendPool(string objectName, int extendSize)
        {
            string tag = objectName;

            if (objectPools.ContainsKey(tag))
            {
                for (int i = 0; i < extendSize; i++)
                {
                    GameObject obj = Instantiate(objectPools[tag].Peek().gameObject, transform);
                    obj.transform.parent = transform;
                    obj.SetActive(false);
                    objectPools[tag].Enqueue(obj);
                }

                return objectPools[tag].Dequeue(); // Return an object from the pool
            }
            else
            {
                Debug.LogWarning("Object pool with tag " + tag + " does not exist.");
                return null;
            }
        }
    }
}


public enum PooledObjectNames
{
    FloatingText
}