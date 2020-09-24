using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.FileBase.Missle_Launcher_Dual_Turret.Missle;

namespace GameDevHQ.FileBase.Missle_Launcher_Dual_Turret
{
    public class Missle_Launcher : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private UnityEngine.GameObject[] _misslePositionsLeft; //array to hold the rocket positions on the turret
        [SerializeField]
        private UnityEngine.GameObject[] _misslePositionsRight; //array to hold the rocket positions on the turret
        [SerializeField]
        private float _fireDelay; //fire delay between rockets
        [SerializeField]
        private float _launchSpeed; //initial launch speed of the rocket
        [SerializeField]
        private float _power; //power to apply to the force of the rocket
        [SerializeField]
        private float _fuseDelay; //fuse delay before the rocket launches
        [SerializeField]
        private float _reloadTime; //time in between reloading the rockets
        [SerializeField]
        private float _destroyTime = 10.0f; //how long till the rockets get cleaned up
        private bool _launched; //bool to check if we launched the rockets

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _launched == false) //check for space key and if we launched the rockets
            {
                _launched = true; //set the launch bool to true
                StartCoroutine(FireRocketsRoutine()); //start a coroutine that fires the rockets. 
            }
        }

        IEnumerator FireRocketsRoutine()
        {
            for (int i = 0; i < _misslePositionsLeft.Length; i++) //for loop to iterate through each missle position
            {
                UnityEngine.GameObject rocketLeft = Instantiate(_missilePrefab) as UnityEngine.GameObject; //instantiate a rocket
                UnityEngine.GameObject rocketRight = Instantiate(_missilePrefab) as UnityEngine.GameObject; //instantiate a rocket

                rocketLeft.transform.parent = _misslePositionsLeft[i].transform; //set the rockets parent to the missle launch position 
                rocketRight.transform.parent = _misslePositionsRight[i].transform; //set the rockets parent to the missle launch position 

                rocketLeft.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                rocketRight.transform.localPosition = Vector3.zero; //set the rocket position values to zero

                rocketLeft.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                rocketRight.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction

                rocketLeft.transform.parent = null; //set the rocket parent to null
                rocketRight.transform.parent = null; //set the rocket parent to null

                rocketLeft.GetComponent<GameDevHQ.FileBase.Missle_Launcher_Dual_Turret.Missle.Missle>().AssignMissleRules(_launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 
                rocketRight.GetComponent<GameDevHQ.FileBase.Missle_Launcher_Dual_Turret.Missle.Missle>().AssignMissleRules(_launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 

                _misslePositionsLeft[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired
                _misslePositionsRight[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
            }

            for (int i = 0; i < _misslePositionsLeft.Length; i++) //itterate through missle positions
            {
                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositionsLeft[i].SetActive(true); //enable fake rocket to show ready to fire
                _misslePositionsRight[i].SetActive(true); //enable fake rocket to show ready to fire
            }

            _launched = false; //set launch bool to false
        }
    }
}

