using Scripts.Managers;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemys
{
    [RequireComponent(typeof(NavMeshAgent))]
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

        public static event Action<Enemy> onEnemyEnterField;
        public static event Action<Enemy> onEnemyExitField;

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
            Warp(_spawnLocation);
            Heal();
        }

        public virtual void Warp(Transform warpLocation)
        {
            _navMeshAgent.Warp(warpLocation.position);
            transform.rotation = warpLocation.rotation;
        }

        protected virtual void OnEnable()
        {
            Recycle();
            NavigateTo(_destination);
            onEnemyEnterField(this);
        }

        protected virtual void Destroy()
        {
            onEnemyExitField(this);
        }
    }

    public enum EnemyType
    {
        Biped,
        Quadped
    }
}