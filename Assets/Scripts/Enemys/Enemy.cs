using Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemys
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected int _maxHealth;
        [SerializeField]
        protected int _killValue;

        protected int _health;

        private NavMeshAgent _navMeshAgent => GetComponent<NavMeshAgent>();

        private Transform _spawnLocation => SpawnManager.Instance.SpawnLocation;
        private Transform _destination => SpawnManager.Instance.EnemyDestination;

        public virtual void NavigateTo(Transform destination)
        {
            _navMeshAgent.destination = destination.position;
        }

        public virtual void Heal()
        {
            _health = _maxHealth;
        }

        public virtual void Recycle()
        {
            gameObject.SetActive(true);
            Warp(_spawnLocation);
            Heal();
        }

        public virtual void Warp(Transform warpLocation)
        {
            _navMeshAgent.Warp(warpLocation.position);
            transform.rotation = warpLocation.rotation;
        }

        protected virtual void Start()
        {
            NavigateTo(_destination);
        }

        //protected virtual void OnEnable()
        //{
        //    NavigateTo(_destination);
        //}
    }

    public enum EnemyType
    {
        Biped,
        Quadped
    }
}