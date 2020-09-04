using Scripts.Enemys;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnLocation, _destination;
        [SerializeField]
        private GameObject[] _enemyPrototypes;
        [SerializeField]
        private int _waves;
        [SerializeField]
        private int _waveDelay;
        [SerializeField]
        private int _individualSpawnDelay;

        private static SpawnManager _instance;
        public static SpawnManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("SpawnManager not instantiated");

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
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
                Enemy enemy = SpawnRandomEnemy();
                enemy.NavigateTo(_destination);
                yield return new WaitForSeconds(_individualSpawnDelay);
            }
        }

        private Enemy SpawnRandomEnemy()
        {
            int randomSelection = UnityEngine.Random.Range(0, _enemyPrototypes.Length);

            return Instantiate(_enemyPrototypes[randomSelection], _spawnLocation.position, _spawnLocation.rotation)
                .GetComponent<Enemy>();
        }
    }
}
