using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncAudio : MonoBehaviour, IPunObservable
{
    [SerializeField] private AudioSource[] sources;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            foreach (AudioSource src in sources)
            {
                stream.SendNext(src.isPlaying);
                stream.SendNext(src.volume);
            }
        }
        else
        {
            foreach (AudioSource src in sources)
            {
                bool isPlaying = (bool)stream.ReceiveNext();
                if (src.isPlaying != isPlaying)
                {
                    if (isPlaying) src.Play();
                    else src.Stop();
                }

                src.volume = (float)stream.ReceiveNext();
            }
        }
    }
}
