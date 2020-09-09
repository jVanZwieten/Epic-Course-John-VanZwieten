using Scripts.Enemys;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField]
        private int _waves;
        [SerializeField]
        private int _waveDelay;
        [SerializeField]
        private int _individualSpawnDelay;
        [SerializeField]
        private GameObject[] _enemyPrefabs;
        [SerializeField]
        private Transform _enemycontainer;

        [SerializeField]
        private Transform _spawnLocation;
        public Transform SpawnLocation { get { return _spawnLocation; } }
        [SerializeField]
        private Transform _enemyDestination;
        public Transform EnemyDestination { get { return _enemyDestination; } }

        
        public GameObject SpawnEnemy(EnemyType enemyType)
        {
            GameObject enemy = Instantiate(_enemyPrefabs[(int)enemyType], SpawnLocation.position, SpawnLocation.rotation);
            enemy.transform.parent = _enemycontainer;

            return enemy;
        }

        private void Start()
        {
            StartCoroutine(SpawnWaves(_waves));
        }

        private IEnumerator SpawnWaves(int waves)
        {
            for (int i = 1; i <= waves; i++)
            {
                StartCoroutine(SpawnWave(i));
                yield return new WaitForSeconds(_waveDelay + i * _individualSpawnDelay);
            }
        }

        private IEnumerator SpawnWave(int currentWave)
        {
            for (int i = 0; i < currentWave; i++)
            {
                GetNewRandomEnemy();
                yield return new WaitForSeconds(_individualSpawnDelay);
            }
        }

        private GameObject GetNewRandomEnemy()
        {
            EnemyType randomType = ChooseRandomEnemyType();
            return PoolManager.Instance.GetNewEnemy(randomType);
        }


        private EnemyType ChooseRandomEnemyType()
        {
            var enemyTypes = (EnemyType[])Enum.GetValues(typeof(EnemyType));
            int random = UnityEngine.Random.Range(0, enemyTypes.Length);
            return enemyTypes[random];
        }
    }
}
