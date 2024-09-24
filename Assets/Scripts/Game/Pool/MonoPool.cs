using System;
using System.Collections.Generic;
using Scripts.Helpers;
using UnityEngine;

namespace Game.Pool
{
    public class MonoPool : MonoBehaviour
    {
        public static MonoPool Instance;
        
        [Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [SerializeField] private List<Pool> _pools;
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private void Awake()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }

            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            
            foreach (Pool pool in _pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public T SpawnObject<T>(string tag, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Tag not find: " + tag);
                return null;
            }

            GameObject objectToSpawn;

            if (_poolDictionary[tag].Count > 0)
            {
                objectToSpawn = _poolDictionary[tag].Dequeue();
            }
            else
            {
                objectToSpawn = Instantiate(_pools.Find(p=> p.tag == tag).prefab);
            }

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            return objectToSpawn.GetComponent<T>();
        }

        public void ReturnToPool(string tag, GameObject objectToReturn)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("This tag doesn't exist: " + tag);
                return;
            }

            objectToReturn.SetActive(false);
            _poolDictionary[tag].Enqueue(objectToReturn);
        }
    }
    }