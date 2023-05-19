using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay
{
    public class Pool : MonoBehaviour
    {
        public static Pool poolInstance;

        private void Start()
        {
            poolInstance = this;
        }

        public List<PreAllocation> preAllocations;
        [SerializeField] private List<GameObject> poolEnemies;

        private void Awake()
        {
            foreach (var preAllocation in preAllocations)
            {
                for (int i = 0; i < preAllocation.count; i++)
                {
                    var objectInstantiate = CreateObject(preAllocation.gameObject);
                    poolEnemies.Add(objectInstantiate);
                }
            }
        }

        public GameObject Spawn(string tagger)
        {
            foreach (var t in poolEnemies.Where(t => !t.activeSelf && t.CompareTag(tagger)))
            {
                t.SetActive(true);
                return t;
            }

            foreach (var obj in from t in preAllocations
                where t.gameObject.CompareTag(tag)
                where t.expandable
                select CreateObject(t.gameObject))
            {
                poolEnemies.Add(obj);
                obj.SetActive(true);
                return obj;
            }

            return null;
        }

        private GameObject CreateObject(GameObject item)
        {
            var entity = Instantiate(item, transform);
            entity.SetActive(false);
            return entity;
        }
    }

    [Serializable]
    public class PreAllocation
    {
        public GameObject gameObject;
        public int count;
        public bool expandable;
    }
}