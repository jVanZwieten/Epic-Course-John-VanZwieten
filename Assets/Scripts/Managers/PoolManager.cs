using Scripts.Enemys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _enemyPrefabs;
        [SerializeField]
        private Transform _spawnLocation, _destination;
        [SerializeField]
        private Transform _enemycontainer;
        
        private List<GameObject> _enemyPool;



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

        public GameObject GetNewRandomEnemy()
        {
            foreach (var enemy in _enemyPool)
            {
                if (!enemy.activeSelf)
                    return Recycle(enemy);
            }

            return AddNewRandomEnemy();
        }

        private void Awake()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _enemyPool = new List<GameObject>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                for (int i = 0; i < _enemyPool.Count; i++)
                {
                    int randomI = UnityEngine.Random.Range(0, _enemyPool.Count);
                    if (_enemyPool[randomI].activeSelf)
                    {
                        _enemyPool[randomI].SetActive(false);
                        return;
                    }
                }
        }

        private GameObject Recycle(GameObject enemy)
        {
            enemy.GetComponent<NavMeshAgent>().Warp(_spawnLocation.position);
            enemy.GetComponent<Enemy>().Heal();
            //enemy.GetComponent<Enemy>().NavigateTo(_destination);
            enemy.SetActive(true);

            return enemy;
        }

        private GameObject AddNewRandomEnemy()
        {
            GameObject newRandomEnemy = SpawnRandomEnemy();
            _enemyPool.Add(newRandomEnemy);
            return newRandomEnemy;
        }

        private GameObject SpawnRandomEnemy()
        {
            int randomSelection = UnityEngine.Random.Range(0, _enemyPrefabs.Length);

            GameObject enemy= Instantiate(_enemyPrefabs[randomSelection], _spawnLocation.position, _spawnLocation.rotation);
            enemy.transform.parent = _enemycontainer;
            enemy.GetComponent<Enemy>().NavigateTo(_destination);

            return enemy;
        }
    }
}
