using Scripts.Enemys;
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

        private SpawnManager _instance;
        public SpawnManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = this;

                return _instance;
            }
        }

        //private int _currentWave;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            //_currentWave = 1;


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
            int randomSelection = Random.Range(0, _enemyPrototypes.Length);

            return Instantiate(_enemyPrototypes[randomSelection], _spawnLocation.position, _spawnLocation.rotation)
                .GetComponent<Enemy>();
        }
    }
}
