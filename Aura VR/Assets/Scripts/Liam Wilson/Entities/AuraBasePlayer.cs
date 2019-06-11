using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using VRTK;

namespace AuraHull.AuraVRGame
{
	public class AuraBasePlayer : MonoBehaviour, IAuraPlayer, IPunInstantiateMagicCallback
	{
		[SerializeField] protected GameObject vrtkRig;
		[SerializeField] protected PhotonView photonView;
        [SerializeField] protected GameObject localOnlyBehaviour;

        public virtual GameObject GameObject
		{
			get { return gameObject; }
		}
        
		public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
		{
            if (!photonView.IsMine)
			{
                vrtkRig.SetActive(false);
                localOnlyBehaviour.SetActive(false);
                return;
			}
		}
		
		public virtual void GameSetup()
		{
            vrtkRig.SetActive(photonView.IsMine);
            localOnlyBehaviour.SetActive(photonView.IsMine);
        }
	}
}