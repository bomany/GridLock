using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;

namespace Grid.Example
{
    public class PickUpSpawner : MonoBehaviour
    {
        public static PickUpSpawner Instance = null;
        public int amount = 1;
        public GameObject pickup;

        int count = 0;

        void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void SpawnPickup()
        {
            GameObject obj = (GameObject)Instantiate(pickup, transform.position, Quaternion.identity);
            GridEntity objEntity = obj.GetComponent<GridEntity>();
            objEntity.SetBlock(GridManager.Instance.GetRandomBlock());
            obj.transform.position = objEntity.block.position;
            obj.transform.parent = transform;
            count++;
        }

        public void ReduceCount(int c)
        {
            count -= c;
        }

        void Awake()
        {
            Singleton();
        }

        void LateUpdate()
        {
            if (count < amount)
            {
                SpawnPickup();
            }
        }
    }
}