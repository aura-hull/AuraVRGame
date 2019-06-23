using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class GameModel : MonoBehaviour
    {
        [SerializeField] private PlayerFactory _playerFactory;
        [SerializeField] private PartSpawner_Networked[] _partSpawners;
        [SerializeField] private UpgradePoint_Networked[] _upgradePoints;
        [SerializeField] private ColliderManager _colliderManager;
        [SerializeField] private GameObject penguinPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool isLocalTest = false;

        public int RequiredClients
        {
            get
            {
                if (isLocalTest) return 1;
                else return NetworkController.MAX_PLAYERS;
            }
        }

        private BaseGameState _activeGameState;

        public BaseGameState ActiveGameState
        {
            get { return this._activeGameState; }
        }

        public IAuraPlayer CurrentPlayer { get; set; }

        public static GameModel Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (this._activeGameState != null)
            {
                this._activeGameState.ExecuteState();
            }
        }

        public void ChangeGameState(BaseGameState newState)
        {
            if (this._activeGameState != null)
            {
                this._activeGameState.FinishState();
            }

            this._activeGameState = newState;
            this._activeGameState.InitState();
        }

        public void BuildPlayer()
        {
            this._playerFactory.Build();
        }

        public void ConfigureColliders()
        {
            this._colliderManager.Configure();
        }

        public void BuildTutorialPenguin()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.InstantiateSceneObject(penguinPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }

        public void SpawnParts()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            for (int i = 0; i < _partSpawners.Length; i++)
            {
                if (_partSpawners[i] != null)
                {
                    _partSpawners[i].SpawnIfReady();
                }
            }
        }

        public void IssueUpgrades()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            for (int i = 0; i < _upgradePoints.Length; i++)
            {
                if (_upgradePoints[i] != null)
                {
                    _upgradePoints[i].UpgradeIfReady();
                }
            }
        }
    }
}