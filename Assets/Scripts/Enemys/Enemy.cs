using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Enemys
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected int _health;
        [SerializeField]
        protected int _killValue;
        protected NavMeshAgent _agent;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void NavigateTo(Transform destination)
        {
            if (_agent == null)
                _agent = GetComponent<NavMeshAgent>();

            _agent.destination = destination.position;
        }
    }
}