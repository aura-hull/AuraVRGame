using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.VR;

namespace AuraHull.AuraVRGame
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField]
        GameObject _boatPlayerPrefab;

        [SerializeField]
        GameObject _titanPlayerPrefab;

        [SerializeField]
        Transform _playerSpawnPoints;

        private GameObject _playerPrefab;

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
            Vector3 spawnSpoint = PlayerSpawnPoints.GetChild(positionIndex).position;

            _playerPrefab = (positionIndex == 0) ? _boatPlayerPrefab : _titanPlayerPrefab;

            GameObject go = PhotonNetwork.Instantiate(_playerPrefab.name, spawnSpoint, Quaternion.identity, 0);
            GameModel.Instance.CurrentPlayer = (IAuraPlayer)go.GetComponent(typeof(IAuraPlayer));

            GameModel.Instance.CurrentPlayer.GameSetup();
        }
    }
}