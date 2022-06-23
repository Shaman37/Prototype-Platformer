using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController2D
{
	public class AfterImagePool : Singleton<AfterImagePool>
	{
        [SerializeField] private GameObject _afterImagePrefab;

        private Queue<GameObject> _availableObjects;

        protected override void Awake()
        {
            base.Awake();

            _availableObjects = new Queue<GameObject>();
            GrowPool();
        }

        private void GrowPool()
        {
            for (int i = 0; i < 10; i++)
            {
                var instanceToAdd = Instantiate(_afterImagePrefab);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd);
            }
        }

        public void AddToPool(GameObject instance)
        {
            instance.SetActive(false);
            _availableObjects.Enqueue(instance);
        }

        public GameObject GetFromPool()
        {
            if (_availableObjects.Count == 0)
            {
                GrowPool();
            }

            var instance = _availableObjects.Dequeue();
            instance.SetActive(true);
            
            return instance;
        }
    }
}
