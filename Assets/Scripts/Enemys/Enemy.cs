using Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemys
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected int _maxHealth;
        [SerializeField]
        protected int _killValue;

        protected const int _cleanupDelay = 5;

        protected int _health;
        protected Animator _animator;

        private NavMeshAgent _navMeshAgent;

        private Transform _spawnLocation => SpawnManager.Instance.SpawnLocation;
        private Transform _destination => SpawnManager.Instance.EnemyDestination;

        public static event Action<Enemy> onEnemyEnterField;
        public static event Action<Enemy> onEnemyKilled;

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
            _animator.SetTrigger("Recycle");
        }

        public virtual void Warp(Transform warpLocation)
        {
            _navMeshAgent.Warp(warpLocation.position);
            transform.rotation = warpLocation.rotation;
        }

        protected void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void OnEnable()
        {
            Recycle();
            NavigateTo(_destination);
            onEnemyEnterField?.Invoke(this);
        }

        protected virtual void Kill()
        {
            _animator.SetTrigger("Death");

            _navMeshAgent.isStopped = true;

            Debug.Log($"A {this.GetType()} died.");
            onEnemyKilled?.Invoke(this);

            StartCoroutine(Cleanup(_cleanupDelay));
        }

        protected virtual IEnumerator Cleanup(int cleanupDelay)
        {
            yield return new WaitForSeconds(cleanupDelay);
            Dissolve();
        }

        private void Dissolve()
        {
            this.gameObject.SetActive(false);
        }

        internal void ReceiveDamage(int weaponDamage)
        {
            _health -= weaponDamage;
            if (_health < 0)
                Kill();
        }
    }

    public enum EnemyType
    {
        Biped,
        Quadped
    }
}