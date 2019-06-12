using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Oculus.Platform.Samples.VrHoops;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Player = Photon.Realtime.Player;

namespace AuraHull.AuraVRGame
{
    public enum NetworkEvent
    {
        BUILD_SITE_PLACED,
        TURBINE_PART_BUILT,
        TURBINE_BUILT,
        SYNC_MANAGERS,
        IK_HANDLES_SET
    }

    public class NetworkController : MonoBehaviour, IOnEventCallback, IInRoomCallbacks, IMatchmakingCallbacks
    {
        public const string NETCODE_VERSION = "1.0";
        public const int MAX_PLAYERS = 2;

        Connection _connection;

        public static event Action OnGameConnected;
        public static event Action<int, string> OnTurbinePartBuilt;
        public static event Action<int, string> OnTurbineBuilt;
        public static event Action<float, float, float, float> OnSyncManagers;
        public static event Action<int, int, int, int> OnIKHandlesSet;
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

        public void NotifyBuildSitePlaced(string prefabName, Vector3 position)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.BUILD_SITE_PLACED,
                eventContent: new object[2] { prefabName, position },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyTurbinePartBuilt(string partName)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TURBINE_PART_BUILT,
                eventContent: new object[2] { PhotonNetwork.LocalPlayer.ActorNumber, partName },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyTurbineBuilt(int buildSiteViewId, string turbinePrefabName)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TURBINE_BUILT,
                eventContent: new object[2] { buildSiteViewId, turbinePrefabName },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifySyncManagers(float playDuration, float score, float powerProduced, float powerUsed)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.SYNC_MANAGERS,
                eventContent: new object[4] { playDuration, score, powerProduced, powerUsed },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyIKHandlesSet(int positionIndex, int headPunId, int leftPunId, int rightPunId)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.IK_HANDLES_SET,
                eventContent: new object[4] { positionIndex, headPunId, leftPunId, rightPunId },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void OnEvent(EventData photonEvent)
        {
            NetworkEvent receivedNetworkEvent = (NetworkEvent)photonEvent.Code;
            var content = photonEvent[ParameterCode.CustomEventContent];
            object[] serialize = content as object[];

            switch (receivedNetworkEvent)
            {
                case NetworkEvent.BUILD_SITE_PLACED:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.InstantiateSceneObject((string) serialize[0], (Vector3) serialize[1], Quaternion.identity);
                    }
                    break;

                case NetworkEvent.TURBINE_PART_BUILT:
                    OnTurbinePartBuilt?.Invoke((int)serialize[0], (string)serialize[1]);
                    break;

                case NetworkEvent.TURBINE_BUILT:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonView buildSite = PhotonView.Find((int)serialize[0]);

                        PhotonNetwork.InstantiateSceneObject((string)serialize[1], buildSite.transform.position, buildSite.transform.rotation);
                        PhotonNetwork.Destroy(buildSite);
                    }

                    if (OnTurbineBuilt != null)
                    {
                        // power stuff
                    }
                    break;

                case NetworkEvent.SYNC_MANAGERS:
                    OnSyncManagers?.Invoke((float)serialize[0], (float)serialize[1], (float)serialize[2], (float)serialize[3]);
                    break;

                case NetworkEvent.IK_HANDLES_SET:
                    OnIKHandlesSet?.Invoke((int) serialize[0], (int) serialize[1], (int) serialize[2], (int)serialize[3]);
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