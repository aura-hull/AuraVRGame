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
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);
        }
        #endregion
    }
}