using Scripts.Enemys;
using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemys
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        private List<Enemy> _enemies;
        public List<Enemy> Enemies
        {
            get { return _enemies; }
        }


        // Start is called before the first frame update
        void Start()
        {
            _enemies = new List<Enemy>();
            Enemy.onEnemyEnterField += EnemyEnterField;
            Enemy.onEnemyKilled += EnemyExitField;
        }

        private void EnemyExitField(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void EnemyEnterField(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        private void OnDisable()
        {
            Enemy.onEnemyEnterField -= EnemyEnterField;
            Enemy.onEnemyKilled -= EnemyExitField;
        }
    }
}
