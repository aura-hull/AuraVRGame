using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SyncChildTransforms : MonoBehaviour, IPunObservable
{
    [SerializeField] private bool syncPositions = true;
    [SerializeField] private bool syncRotations = true;
    [SerializeField] private bool syncScales = false;

    public List<Transform> observed;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            foreach (Transform o in observed)
            {
                if (syncPositions)
                {
                    stream.SendNext(o.position.x);
                    stream.SendNext(o.position.y);
                    stream.SendNext(o.position.z);
                }

                if (syncRotations)
                {
                    stream.SendNext(o.rotation.x);
                    stream.SendNext(o.rotation.y);
                    stream.SendNext(o.rotation.z);
                    stream.SendNext(o.rotation.w);
                }

                if (syncScales)
                {
                    stream.SendNext(o.localScale.x);
                    stream.SendNext(o.localScale.y);
                    stream.SendNext(o.localScale.z);
                }
            }

            Debug.Log("sent: " + stream.Count);
        }
        else
        {
            Debug.Log("received: " + stream.Count);

            foreach (Transform o in observed)
            {
                if (syncPositions)
                {
                    float x = (float)stream.ReceiveNext();
                    float y = (float)stream.ReceiveNext();
                    float z = (float)stream.ReceiveNext();
                    o.position = new Vector3(x, y, z);
                }

                if (syncRotations)
                {
                    float x = (float)stream.ReceiveNext();
                    float y = (float)stream.ReceiveNext();
                    float z = (float)stream.ReceiveNext();
                    float w = (float)stream.ReceiveNext();
                    o.rotation = new Quaternion(x, y, z, w);
                }

                if (syncScales)
                {
                    float x = (float)stream.ReceiveNext();
                    float y = (float)stream.ReceiveNext();
                    float z = (float)stream.ReceiveNext();
                    o.localScale = new Vector3(x, y, z);
                }
            }
        }
    }
}
