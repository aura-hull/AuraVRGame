using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.VR;

namespace AuraHull.AuraVRGame
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _boatPlayerPrefab;
        [SerializeField] private GameObject _titanPlayerPrefab;
        [SerializeField] private Transform _playerSpawnPoints;

        public Transform PlayerSpawnPoints
        {
            get
            {
                return _playerSpawnPoints;
            }
        }

        public void Build()
        {
            if (GameModel.Instance.ActiveGameState is InitializingGameState)
            {
                BuildPlayerForGame();
            }
        }

        public void BuildPlayerForGame()
        {
            if (GameModel.Instance.CurrentPlayer != null)
            {
                GameObject.DestroyImmediate(GameModel.Instance.CurrentPlayer.GameObject);
            }

            int positionIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["position"];
            Vector3 spawnPoint = PlayerSpawnPoints.GetChild(positionIndex).position;
            Quaternion spawnRot = PlayerSpawnPoints.GetChild(positionIndex).rotation;

            GameObject _playerPrefab = (positionIndex == 0) ? _boatPlayerPrefab : _titanPlayerPrefab;

            GameObject go = PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint, spawnRot, 0);
            GameModel.Instance.CurrentPlayer = (IAuraPlayer)go.GetComponent(typeof(IAuraPlayer));

            GameModel.Instance.CurrentPlayer.GameSetup();
        }
    }
}