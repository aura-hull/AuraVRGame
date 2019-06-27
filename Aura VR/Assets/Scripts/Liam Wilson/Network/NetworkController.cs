using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Oculus.Platform.Samples.VrHoops;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player = Photon.Realtime.Player;

namespace AuraHull.AuraVRGame
{
    public enum NetworkEvent
    {
        GAMESTATE_CHANGED,
        TUTORIAL_NEXT,
        TUTORIAL_CLIENT_READY,
        UPGRADED_OR_DOWNGRADED,
        BUILD_SITE_PLACED,
        BUILD_SITE_DESTROYED,
        TURBINE_PART_BUILT,
        TURBINE_BUILT,
        SYNC_MANAGERS,
        IK_HANDLES_SET,
        SCORE_SAVED
    }

    public class NetworkController : MonoBehaviour, IOnEventCallback, IInRoomCallbacks, IMatchmakingCallbacks
    {
        public const string NETCODE_VERSION = "1.0";
        public const int MAX_PLAYERS = 2;

        Connection _connection;
        
        public static event Action OnGameConnected;
        public static event Action<AuraGameManager.GameState> OnGameStateChanged;
        public static event Action<int> OnPlayNextTutorial;
        public static event Action OnTutorialClientReady;
        public static event Action<float, float, int> OnUpgradedOrDowngraded;
        public static event Action OnTurbineBuildSitePlaced;
        public static event Action<int, int, int> OnTurbinePartBuilt;
        public static event Action<int, string> OnTurbineBuilt;
        public static event Action<float, float, float, float, float> OnSyncManagers;
        public static event Action<float, string, int> OnScoreSaved;
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

        public static int ConnectedPlayers
        {
            get { return PhotonNetwork.CurrentRoom.PlayerCount; }
        }

        public void NotifyGameStateChanged(AuraGameManager.GameState state)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.GAMESTATE_CHANGED,
                eventContent: new object[1] { state },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyPlayNextTutorial(int nextSpeakIndex)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TUTORIAL_NEXT,
                eventContent: new object[1] { nextSpeakIndex },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyTutorialClientReady()
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TUTORIAL_CLIENT_READY,
                eventContent: null,
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyUpgradedOrDowngraded(float powerProductionMultiplier, float powerConsumptionMultiplier, int upgradeLevel)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.UPGRADED_OR_DOWNGRADED,
                eventContent: new object[3] { powerProductionMultiplier, powerConsumptionMultiplier, upgradeLevel },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
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

        public void NotifyBuildSiteDestroyed(int buildSiteViewId)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.BUILD_SITE_DESTROYED,
                eventContent: new object[1] { buildSiteViewId },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyTurbinePartBuilt(int turbinePhotonId, int matcherIndex)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TURBINE_PART_BUILT,
                eventContent: new object[3] { PhotonNetwork.LocalPlayer.ActorNumber, turbinePhotonId, matcherIndex },
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

        public void NotifySyncManagers(float powerProduced, float powerUsed, float powerStored, float playDuration, float score)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.SYNC_MANAGERS,
                eventContent: new object[5] { powerProduced, powerUsed, powerStored, playDuration, score },
                raiseEventOptions: customOptions,
                sendOptions: SendOptions.SendReliable
            );
        }

        public void NotifyScoreSaved(float score, string name, int insertedIndex)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.SCORE_SAVED,
                eventContent: new object[3] { score, name, insertedIndex },
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
                case NetworkEvent.GAMESTATE_CHANGED:
                    OnGameStateChanged?.Invoke((AuraGameManager.GameState)serialize[0]);
                    break;

                case NetworkEvent.TUTORIAL_NEXT:
                    OnPlayNextTutorial?.Invoke((int)serialize[0]);
                    break;

                case NetworkEvent.TUTORIAL_CLIENT_READY:
                    OnTutorialClientReady?.Invoke();
                    break;

                case NetworkEvent.UPGRADED_OR_DOWNGRADED:
                    OnUpgradedOrDowngraded?.Invoke((float)serialize[0], (float)serialize[1], (int)serialize[2]);
                    break;

                case NetworkEvent.BUILD_SITE_PLACED:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.InstantiateSceneObject((string) serialize[0], (Vector3) serialize[1], Quaternion.identity);
                    }
                    OnTurbineBuildSitePlaced?.Invoke();
                    break;

                case NetworkEvent.BUILD_SITE_DESTROYED:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonView buildSite = PhotonView.Find((int)serialize[0]);
                        PhotonNetwork.Destroy(buildSite);
                    }
                    break;

                case NetworkEvent.TURBINE_PART_BUILT:
                    OnTurbinePartBuilt?.Invoke((int)serialize[0], (int)serialize[1], (int)serialize[2]);
                    break;

                case NetworkEvent.TURBINE_BUILT:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonView buildSite = PhotonView.Find((int)serialize[0]);

                        PhotonNetwork.InstantiateSceneObject((string)serialize[1], buildSite.transform.position, buildSite.transform.rotation);
                        PhotonNetwork.Destroy(buildSite);
                    }

                    OnTurbineBuilt?.Invoke((int)serialize[0], (string)serialize[1]);
                    break;

                case NetworkEvent.SYNC_MANAGERS:
                    OnSyncManagers?.Invoke((float)serialize[0], (float)serialize[1], (float)serialize[2], (float)serialize[3], (float)serialize[4]);
                    break;

                case NetworkEvent.SCORE_SAVED:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        OnScoreSaved?.Invoke((float)serialize[0], (string)serialize[1], (int)serialize[2]);
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
            Debug.Log("Created room.");
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