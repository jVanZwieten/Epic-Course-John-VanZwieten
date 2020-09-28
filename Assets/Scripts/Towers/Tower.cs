using Scripts.Enemys;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rangeSphere;
        [SerializeField]
        private GameObject _yRotate;
        [SerializeField]
        private GameObject _xRotate;
        [SerializeField]
        private float _rotationRate;
        [SerializeField]
        private int _cost;

        private List<Enemy> _enemiesInRange;
        private Enemy target;

        internal virtual void Start()
        {
            _enemiesInRange = new List<Enemy>();
            Enemy.onEnemyExitField += onEnemyExitField;
        }

        private void onEnemyExitField(Enemy enemy)
        {
            if (_enemiesInRange.Contains(enemy))
                _enemiesInRange.Remove(enemy);
            if (target.Equals(enemy))
                SelectNewTarget();
        }

        internal virtual void Update()
        {
            if (target != null)
                SlewTo(target);
        }

        private void SlewTo(Enemy target)
        {
            var targetDirection = target.transform.position - _xRotate.transform.position;

            Debug.DrawRay(_xRotate.transform.position, targetDirection, Color.green);

            var yRotation = Quaternion.LookRotation(targetDirection);
            yRotation = Quaternion.Slerp(_yRotate.transform.rotation, yRotation, Time.deltaTime * _rotationRate);
            _yRotate.transform.rotation = Quaternion.Euler(new Vector3(0, yRotation.eulerAngles.y, 0));

            var xRotation = Quaternion.LookRotation(targetDirection);
            xRotation = Quaternion.Slerp(_xRotate.transform.rotation, xRotation, Time.deltaTime * _rotationRate);
            _xRotate.transform.localRotation = Quaternion.Euler(new Vector3(xRotation.eulerAngles.x, 0, 0));

        }

        public virtual void OnEnemyEnter(Collider collider)
        {
            var enemy=collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                _enemiesInRange.Add(enemy);

                if (target == null)
                    target = enemy;
            }
        }

        public virtual void OnEnemyExit(Collider collider)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                _enemiesInRange.Remove(enemy);

                if (target.Equals(enemy))
                    target = SelectNewTarget();
            }
        }

        private Enemy SelectNewTarget()
        {
            return EnemyManager.Instance.Enemies.FirstOrDefault(enemy => _enemiesInRange.Contains(enemy));
        }
    }
}
