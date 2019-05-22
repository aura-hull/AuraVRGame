using UnityEngine;
using AuraHull.AuraVRGame;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private int spawnpointId;

    public void Awake()
    {
        
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        Hashtable props = (Hashtable)playerAndUpdatedProps[1];

        if (!props.ContainsKey("position"))
        {
            return;
        }

        int position = (int)props["position"];
        if (position != spawnpointId)
        {
            return;
        }

        // Correct spawn point for action.
    }

    public void OnPhotonPlayerDisconnected(Photon.Realtime.Player player)
    {
        if (!player.CustomProperties.ContainsKey("position"))
        {
            return;
        }

        int position = (int)player.CustomProperties["position"];

        if (position != spawnpointId)
        {
            return;
        }

        // Correct spawn point for action.
    }

    public void RestoreDefaults()
    {
        // Reset behaviour.
    }
}
