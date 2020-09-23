using Scripts.Towers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class TowerBuildManager : MonoSingleton<TowerBuildManager>
    {
        [SerializeField]
        private List<TowerGhost> _ghostTowers;
        [SerializeField]
        private List<GameObject> _buildableTowers;

        private bool _towerPlacementMode;
        private bool _buildIsAllowed;
        private TowerGhost _towerGhost;

        private bool mouseMoved => Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;

        [SerializeField]
        private Color _buildAllowedColor;
        public Color BuildAllowedColor { get { return _buildAllowedColor; } }
        [SerializeField]
        private Color _buildDisallowedColor;
        public Color BuildDisallowedColor { get { return _buildDisallowedColor; } }

        public delegate void TowerGhostMoveEventHandler(Vector3 towerGhostPosition);
        public static event TowerGhostMoveEventHandler TowerGhostMove;

        public delegate void TowerPlacementModeToggleEventHandler(bool placementMode);
        public static event TowerPlacementModeToggleEventHandler TowerPlacementModeToggle;

        public delegate void TowerBuiltEventHandler(GameObject Tower);
        public static event TowerBuiltEventHandler TowerBuilt;


        void Start()
        {
            _towerPlacementMode = false;
            TowerSlot.TowerGhostSnapToSlot += OnTowerGhostSnapToSlot;
        }

        private void OnTowerGhostSnapToSlot(TowerSlot towerSlot)
        {
            _buildIsAllowed = true;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                EnterPlacementMode(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                EnterPlacementMode(1);
            else if (Input.GetKeyDown(KeyCode.Mouse0) && _buildIsAllowed)
                InstantiateTower(_towerGhost);
            else if (Input.GetKeyDown(KeyCode.Mouse1))
                ExitPlacementMode();

            if (_towerPlacementMode && mouseMoved)
                TowerGhostUpdate();
        }

        private void InstantiateTower(TowerGhost towerGhost)
        {
            var towerOption = _ghostTowers.FindIndex(ghost => ghost == towerGhost);
            var newTowerPrefab = _buildableTowers[towerOption];

            var newTower = Instantiate(newTowerPrefab, _towerGhost.transform.position, _towerGhost.transform.rotation);

            TowerBuilt(newTower);
        }

        private void EnterPlacementMode(int towerSelection)
        {
            _towerPlacementMode = true;

            SetTowerGhost(_ghostTowers[towerSelection]);

            TowerPlacementModeToggle(_towerPlacementMode);
        }

        private void ExitPlacementMode()
        {
            _towerPlacementMode = false;

            TowerPlacementModeToggle(_towerPlacementMode);
        }


        private void SetTowerGhost(TowerGhost towerGhostSelection)
        {
            if (_towerGhost != towerGhostSelection)
            {
                _towerGhost?.gameObject.SetActive(false);
                _towerGhost = towerGhostSelection;
            }

            _towerGhost.gameObject.SetActive(true);
        }

        private void TowerGhostUpdate()
        {
            Vector3? hitPoint = Utils.RaycastToMouse()?.point;

            if (hitPoint.HasValue)
            {
                _towerGhost.transform.position = hitPoint.Value;
                DissallowBuild();

                TowerGhostMove(hitPoint.Value);
            }
        }

        private void DissallowBuild()
        {
            _towerGhost.ChangeColor(BuildDisallowedColor);
            _buildIsAllowed = false;
        }
    }
}
