using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class GameModel : MonoBehaviour
    {
        [SerializeField]
        PlayerFactory _playerFactory;

        BaseGameState _activeGameState;

        public BaseGameState ActiveGameState
        {
            get { return this._activeGameState; }
        }

        public IAuraPlayer CurrentPlayer { get; set; }

        public static GameModel Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (this._activeGameState != null)
            {
                this._activeGameState.ExecuteState();
            }
        }

        public void ChangeGameState(BaseGameState newState)
        {
            if (this._activeGameState != null)
            {
                this._activeGameState.FinishState();
            }

            this._activeGameState = newState;
            this._activeGameState.InitState();
        }

        public void BuildPlayer()
        {
            this._playerFactory.Build();
        }
    }
}