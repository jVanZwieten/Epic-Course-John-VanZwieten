using Scripts.Enemys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private List<GameObject> _bipedPool, _quadpedPool;


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

            var enemy = pool.FirstOrDefault(e => !e.activeSelf);
            if (enemy == null)
            {
                enemy = SpawnManager.Instance.SpawnEnemy(enemyType);
                pool.Add(enemy);
            }
                
            return enemy;
        }

    protected override void Awake()
        {
            base.Awake();


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
