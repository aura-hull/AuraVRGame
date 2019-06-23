using System.IO;
using System.Xml;
using ExitGames.Client.Photon;
using Oculus.Platform;
using Photon.Pun;
using Photon.Realtime;
using Application = UnityEngine.Application;
using RoomOptions = Photon.Realtime.RoomOptions;

namespace AuraHull.AuraVRGame
{
    public class Connection : MonoBehaviourPunCallbacks
    {
        private static string _serverSettingsPath = Application.streamingAssetsPath + "/photon-server-settings.xml";

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
                ConfigureAppSettings();
                PhotonNetwork.GameVersion = NetworkController.NETCODE_VERSION;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        private void ConfigureAppSettings()
        {
            AppSettings appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;

            if (!ConfigureLocalHost(ref appSettings))
            {
                // Defaults, name server.
                appSettings.UseNameServer = true;
                appSettings.Server = "";
                appSettings.Port = -1;
                appSettings.Protocol = ConnectionProtocol.Tcp;
            }
        }

        private bool ConfigureLocalHost(ref AppSettings settings)
        {
            if (!File.Exists(_serverSettingsPath)) return false;
            
            XmlNodeList[] elementList = Xml.Read(_serverSettingsPath, "AppSettings");
            if (elementList.Length == 0) return false;

            foreach (XmlElement e in elementList[0])
            {
                if (e.LocalName != "AppSettings") return false;

                settings.UseNameServer = false;

                foreach (XmlNode node in e.ChildNodes)
                {
                    switch (node.LocalName.ToLower())
                    {
                        case "server":
                            settings.Server = node.InnerText;
                            break;

                        case "port":
                            settings.Port = int.Parse(node.InnerText);
                            break;

                        case "protocol":
                            settings.Protocol = ParseProtocol(node.InnerText);
                            break;
                    }
                }

                return true;
            }

            return false;
        }

        private ConnectionProtocol ParseProtocol(string protocol)
        {
            switch (protocol)
            {
                case "tcp": return ConnectionProtocol.Tcp;
                case "udp": return ConnectionProtocol.Udp;
                case "websocket": return ConnectionProtocol.WebSocket;
                case "websocketsecure": return ConnectionProtocol.WebSocketSecure;
                default: return 0;
            }
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