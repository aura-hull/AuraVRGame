﻿using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
	public class AuraBasePlayer : MonoBehaviour, IAuraPlayer, IPunInstantiateMagicCallback
	{
		[SerializeField]
		protected GameObject vrtkRig;

		[SerializeField]
		protected PhotonView photonView;
        
		public virtual GameObject GameObject
		{
			get { return gameObject; }
		}
        
		public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
		{			
			if (!photonView.IsMine)
			{
                vrtkRig.SetActive(false);
				return;
			}
		}

		void Update()
		{
			
		}
		
		public virtual void GameSetup()
		{
            vrtkRig.SetActive(photonView.IsMine);
		}
	}
}