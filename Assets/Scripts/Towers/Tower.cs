using Scripts.Enemys;
using System;
using System.Collections;
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
        private float _targetVerticalOffset;
        [SerializeField]
        private float _weaponFireDelay;
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
                Retarget();
        }

        private void Retarget()
        {
            CeaceFire();
            target = SelectNewTarget();
            if (target != null)
                StartCoroutine(DelayFire(target, _weaponFireDelay));
        }

        internal virtual void Update()
        {
            if (target != null)
                SlewTo(target);
        }

        private void SlewTo(Enemy target)
        {
            var targetPosition = target.transform.position + new Vector3(0, _targetVerticalOffset);
            var targetDirection = targetPosition - _xRotate.transform.position;

            Debug.DrawRay(_xRotate.transform.position, targetDirection, Color.green);

            var rotation = Quaternion.LookRotation(targetDirection);

            var yRotation = Quaternion.Slerp(_yRotate.transform.rotation, rotation, Time.deltaTime * _rotationRate);
            _yRotate.transform.rotation = Quaternion.Euler(new Vector3(0, yRotation.eulerAngles.y, 0));

            var xRotation = Quaternion.Slerp(_xRotate.transform.rotation, rotation, Time.deltaTime * _rotationRate);
            _xRotate.transform.localRotation = Quaternion.Euler(new Vector3(xRotation.eulerAngles.x, 0, 0));
        }

        protected abstract void Fire(Enemy target);

        public virtual void OnEnemyEnter(Collider collider)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                _enemiesInRange.Add(enemy);

                if (target == null)
                    Retarget();
            }
        }

        public virtual void OnEnemyExit(Collider collider)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                _enemiesInRange.Remove(enemy);

                if (target.Equals(enemy))
                {
                    Retarget();
                }
            }
        }

        protected abstract void CeaceFire();

        private Enemy SelectNewTarget()
        {
            return EnemyManager.Instance.Enemies.FirstOrDefault(enemy => _enemiesInRange.Contains(enemy));
        }

        private IEnumerator DelayFire(Enemy enemy, float delay)
        {
            yield return new WaitForSeconds(delay);
            Fire(enemy);
        }
    }
}
