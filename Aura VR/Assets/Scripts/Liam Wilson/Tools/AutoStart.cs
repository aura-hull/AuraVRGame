using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class AutoStart : MonoBehaviour
    {
        void Start()
        {
            GameController.Instance.StartMultiplayerGame();
        }
    }
}