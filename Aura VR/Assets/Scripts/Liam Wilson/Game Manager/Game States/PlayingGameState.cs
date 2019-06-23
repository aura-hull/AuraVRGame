using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class PlayingGameState : BaseGameState
    {
        public override void InitState()
        {
            base.InitState();

            NetworkController.OnSomePlayerConnected += this.SomeoneConnectedMessage;
            NetworkController.OnSomePlayerDisconnected += this.SomeoneDisconnectedMessage;
        }

        public override void FinishState()
        {
            base.FinishState();

            NetworkController.OnSomePlayerConnected -= this.SomeoneConnectedMessage;
            NetworkController.OnSomePlayerDisconnected -= this.SomeoneDisconnectedMessage;
        }

        public override void ExecuteState()
        {
            base.ExecuteState();
            
            AuraGameManager.Instance.Execute();
        }

        void SomeoneConnectedMessage(Player somePlayer)
        {
            Debug.Log("Player ID: " + somePlayer.ActorNumber + " connected");
        }

        void SomeoneDisconnectedMessage(Player somePlayer)
        {
            Debug.Log("Player ID: " + somePlayer.ActorNumber + " disconnected");
        }
    }
}