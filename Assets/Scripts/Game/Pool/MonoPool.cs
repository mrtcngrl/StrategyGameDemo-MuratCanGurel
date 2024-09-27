using System;
using System.Collections.Generic;
using Scripts.Helpers;
using UnityEngine;

namespace Game.Pool
{
    public class MonoPool : MonoBehaviour
    {
        [Serializable]
        public class Pool
        {
            public string Name;
            public GameObject Prefab;
            public int Size;
        }
        public static MonoPool Instance;
        [SerializeField] private List<Pool> _pools;
        private Dictionary<string, Queue<GameObject>> _poolDictionary;
        private Transform _poolParent;
        private void Awake()
        {
            _poolParent = new GameObject("PoolObjects").transform;
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }

            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            
            foreach (Pool pool in _pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab,_poolParent);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.Name, objectPool);
            }
        }

        public T SpawnObject<T>(string name, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            if (!_poolDictionary.ContainsKey(name))
            {
                Debug.LogWarning("Name not find: " + name);
                return null;
            }

            GameObject objectToSpawn;

            if (_poolDictionary[name].Count > 0)
            {
                objectToSpawn = _poolDictionary[name].Dequeue();
            }
            else
            {
                objectToSpawn = Instantiate(_pools.Find(p=> p.Name == name).Prefab,_poolParent);
            }

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            return objectToSpawn.GetComponent<T>();
        }

        public void ReturnToPool(string name, GameObject objectToReturn)
        {
            if (!_poolDictionary.ContainsKey(name))
            {
                Debug.LogWarning("This name doesn't exist: " + name);
                return;
            }

            objectToReturn.SetActive(false);
            _poolDictionary[name].Enqueue(objectToReturn);
        }
    }
    }