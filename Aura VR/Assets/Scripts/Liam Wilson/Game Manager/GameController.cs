using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void StartMultiplayerGame()
        {
            GameModel.Instance.ChangeGameState(new ConnectingGameState());
            NetworkController.Instance.StartMultiplayerGame();
        }
    }
}