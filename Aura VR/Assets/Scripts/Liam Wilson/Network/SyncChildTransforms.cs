using System;
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
    [SerializeField, Range(0.05f, 1.0f)] private float syncFrequency = 0.05f;

    public List<Transform> observed;

    private float _timeUntilSync;

    void Start()
    {
        _timeUntilSync = syncFrequency;
    }

    void Update()
    {
        _timeUntilSync -= Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (_timeUntilSync > 0.0f) return;

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
        }
        else
        {
            foreach (Transform o in observed)
            {
                if (syncPositions)
                {
                    float x = (float)stream.ReceiveNext();
                    float y = (float)stream.ReceiveNext();
                    float z = (float)stream.ReceiveNext();

                    StartCoroutine(LerpPosition(o, new Vector3(x, y, z), _timeUntilSync));
                }

                if (syncRotations)
                {
                    float x = (float)stream.ReceiveNext();
                    float y = (float)stream.ReceiveNext();
                    float z = (float)stream.ReceiveNext();
                    float w = (float)stream.ReceiveNext();

                    StartCoroutine(LerpRotation(o, new Quaternion(x, y, z, w), _timeUntilSync));
                }

                if (syncScales)
                {
                    float x = (float)stream.ReceiveNext();
                    float y = (float)stream.ReceiveNext();
                    float z = (float)stream.ReceiveNext();

                    StartCoroutine(LerpScale(o, new Vector3(x, y, z), _timeUntilSync));
                }
            }
        }

        _timeUntilSync = syncFrequency;
    }
    
    private IEnumerator LerpPosition(Transform target, Vector3 end, float duration)
    {
        Vector3 start = target.position;
        float t = 0.0f;

        while (t < duration)
        {
            t += (Time.deltaTime / duration);
            target.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        yield return null;
    }

    private IEnumerator LerpRotation(Transform target, Quaternion end, float duration)
    {
        Quaternion start = target.rotation;
        float t = 0.0f;

        while (t < duration)
        {
            t += (Time.deltaTime / duration);
            target.rotation = Quaternion.Lerp(start, end, t);
            yield return null;
        }

        yield return null;
    }

    private IEnumerator LerpScale(Transform target, Vector3 end, float duration)
    {
        Vector3 start = target.localScale;
        float t = 0.0f;

        while (t < duration)
        {
            t += (Time.deltaTime / duration);
            target.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }

        yield return null;
    }
}
