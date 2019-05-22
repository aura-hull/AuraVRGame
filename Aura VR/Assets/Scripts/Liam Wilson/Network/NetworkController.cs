using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public enum NetworkEvent
    {
        TURBINE_BUILT
    }

    public class NetworkController : MonoBehaviour, IOnEventCallback, IInRoomCallbacks, IMatchmakingCallbacks
    {
        public const string NETCODE_VERSION = "1.0";
        public const int MAX_PLAYERS = 2;

        Connection _connection;

        public static event Action OnGameConnected;
        public static event Action OnTurbineBuilt;
        public static event Action<Player> OnSomePlayerConnected;
        public static event Action<Player> OnSomePlayerDisconnected;
        public static event Action<Player> OnRoundEnded;

        public static NetworkController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
            this._connection = this.GetComponent<Connection>();
        }

        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void StartMultiplayerGame()
        {
            this._connection.Init();
            this._connection.Connect();
        }

        public void EndMultiplayerGame()
        {
            this._connection.Disconnect();
        }

        public void NotifyTurbineBuilt()
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TURBINE_BUILT,
                eventContent: null,
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void OnEvent(EventData photonEvent)
        {
            NetworkEvent receivedNetworkEvent = (NetworkEvent)photonEvent.Code;
            var content = photonEvent[ParameterCode.CustomEventContent];

            switch (receivedNetworkEvent)
            {
                case NetworkEvent.TURBINE_BUILT:
                    if (OnTurbineBuilt != null)
                    {
                        OnTurbineBuilt();
                    }
                    break;
            }
        }

        #region IInRoomCallbacks
        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (OnSomePlayerConnected != null)
            {
                OnSomePlayerConnected(newPlayer);
            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (OnSomePlayerDisconnected != null)
            {
                OnSomePlayerDisconnected(otherPlayer);
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }
        public void OnMasterClientSwitched(Player newMasterClient) { }
        #endregion

        #region IMatchmakingCallbacks
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }
        public void OnCreateRoomFailed(short returnCode, string message) { }

        public void OnCreatedRoom()
        {
            Debug.Log("Called on created room");
        }

        public void OnJoinedRoom()
        {
            Debug.Log("Called on joined room");
            if (OnGameConnected != null)
            {
                OnGameConnected();
            }
        }

        public void OnJoinRoomFailed(short returnCode, string message) { }
        public void OnJoinRandomFailed(short returnCode, string message) { }
        public void OnLeftRoom() { }
        #endregion
    }
}