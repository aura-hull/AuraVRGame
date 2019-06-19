using Photon.Pun;
using Photon.Realtime;

namespace AuraHull.AuraVRGame
{
    public class Connection : MonoBehaviourPunCallbacks
    {
        public void Init()
        {

        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                this.OnConnectedToMaster();
            }
            else
            {
                PhotonNetwork.GameVersion = NetworkController.NETCODE_VERSION;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {
            if (PhotonNetwork.InRoom)
            {
                this.OnJoinedRoom();
            }
            else
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = NetworkController.MAX_PLAYERS;
            options.PlayerTtl = 1;
            options.EmptyRoomTtl = 2;

            PhotonNetwork.CreateRoom(null, options, null);
        }
        #endregion
    }
}