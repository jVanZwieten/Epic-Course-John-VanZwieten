using Scripts.Enemys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        private List<GameObject> _bipedPool, _quadpedPool;


        private static PoolManager _instance;
        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("PoolManager not instantiated");

                return _instance;
            }
        }

        public GameObject GetNewEnemy(EnemyType enemyType)
        {
            List<GameObject> pool;

            switch (enemyType)
            {
                case EnemyType.Biped:
                    pool = _bipedPool;
                    break;
                case EnemyType.Quadped:
                    pool = _quadpedPool;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var enemy = pool.FirstOrDefault(biped => !biped.activeSelf);
            if (enemy != null)
                enemy.GetComponent<Enemy>().Recycle();
            else
            {
                enemy = SpawnManager.Instance.SpawnEnemy(enemyType);
                pool.Add(enemy);
            }

            return enemy;
        }

        private void Awake()
        {
            _instance = this;


            _bipedPool = new List<GameObject>();
            _quadpedPool = new List<GameObject>();
        }

        private void Update()
        {
            var enemyPool = _bipedPool.Concat(_quadpedPool).ToList();

            if (Input.GetKeyDown(KeyCode.Space))
                for (int i = 0; i < enemyPool.Count; i++)
                {
                    int randomI = UnityEngine.Random.Range(0, enemyPool.Count);
                    if (enemyPool[randomI].activeSelf)
                    {
                        enemyPool[randomI].SetActive(false);
                        return;
                    }
                }
        }
    }
}
