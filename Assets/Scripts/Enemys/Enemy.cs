using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemys
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected int _maxHealth;
        [SerializeField]
        protected int _health;
        [SerializeField]
        protected int _killValue;
        protected NavMeshAgent _agent;

        public virtual void NavigateTo(Transform destination)
        {
            if (_agent == null)
                _agent = GetComponent<NavMeshAgent>();

            _agent.destination = destination.position;
        }

        public virtual void Heal()
        {
            _health = _maxHealth;
        }
    }
}