using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class ConnectingGameState : BaseGameState
    {
        public override void InitState()
        {
            base.InitState();
            NetworkController.OnGameConnected += this.InitGame;
        }

        public override void FinishState()
        {
            base.FinishState();
            NetworkController.OnGameConnected -= this.InitGame;
        }

        void InitGame()
        {
            this.SetPlayerData();
            GameModel.Instance.ChangeGameState(new InitializingGameState());
        }

        void SetPlayerData()
        {
            List<int> freePositions = new List<int>();
            for (int pos = 0; pos < NetworkController.MAX_PLAYERS; pos++)
            {
                freePositions.Add(pos);
            }

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties["position"] != null)
                {
                    freePositions.Remove((int)player.CustomProperties["position"]);
                }
            }

            string playerName = string.Empty;
            switch (freePositions[0])
            {
                case 0:
                    playerName = "Player BOAT";
                    break;
                case 1:
                    playerName = "Player TITAN";
                    break;
            }

            Hashtable playerInfo = new Hashtable();
            playerInfo.Add("position", freePositions[0]);
            playerInfo.Add("name", playerName);
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerInfo);

            Debug.Log("CustomProperties[\"position\"]: " + PhotonNetwork.LocalPlayer.CustomProperties["position"]);
            Debug.Log("LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}