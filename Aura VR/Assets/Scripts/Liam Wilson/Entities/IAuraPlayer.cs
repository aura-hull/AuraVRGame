using UnityEngine;

namespace AuraHull.AuraVRGame
{
	public interface IAuraPlayer
	{
		GameObject GameObject { get; }

		void GameSetup();
	}
}