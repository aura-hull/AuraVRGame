using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BuildPartMatcher : MonoBehaviour
{
    [SerializeField] private int matcherIndex = -1;
    [SerializeField] private GameObject partPrefab;
    [SerializeField] private BuildingSite linkedSite;

    private bool[] _pointsMatched;
    private BuildPartPoint[] _partPoints;

    public string PartName
    {
        get
        {
            if (partPrefab == null) return "";
            return partPrefab.name;
        }
    }

    public int Index
    {
        get { return matcherIndex; }
    }

    void Start()
    {
        if (PartName == "")
        {
            partPrefab = this.gameObject;
        }

        // When you look at this, you ask: "Why not just use GetComponentsInChildren?" and I say... Hey, why not?
        // Of course, Unity says "nah, I will search recursively" and suddenly you find every damn component in existence.
        List<BuildPartPoint> foundPoints = new List<BuildPartPoint>();
        for (int i = 0; i < transform.childCount; i++)
        {
            BuildPartPoint found = transform.GetChild(i).GetComponent<BuildPartPoint>();
            if (found != null)
            {
                foundPoints.Add(found);
            }
        }

        _partPoints = foundPoints.ToArray();
        // ^ So this instead. Stupid Unity.

        for (int i = 0; i < _partPoints.Length; i++)
        {
            _partPoints[i].pointIndex = i;
            _partPoints[i].matcher = this;

            _partPoints[i].OnSuccessfulMatch += OnPartPointMatch;
            _partPoints[i].OnMatchRemoved += OnPartPointMatchRemoved;
        }

        _pointsMatched = new bool[_partPoints.Length];

        BodyPhysicsManager.Instance.AddToIgnoredCollisions(gameObject);
    }

    public void OnPartPointMatch(int pointIndex)
    {
        if (pointIndex < 0 || pointIndex >= _pointsMatched.Length) return;
        _pointsMatched[pointIndex] = true;

        // Check for complete matches.
        CheckMatches();
    }

    public void OnPartPointMatchRemoved(int pointIndex)
    {
        if (pointIndex < 0 || pointIndex >= _pointsMatched.Length) return;
        _pointsMatched[pointIndex] = false;
    }

    private void CheckMatches()
    {
        for (int i = 0; i < _pointsMatched.Length; i++)
        {
            if (_pointsMatched[i] == false) return;
        }

        if (linkedSite != null)
        {
            linkedSite.BuildPart(this);
        }

        PhotonView view = GetComponent<PhotonView>();
        if (view == null || view.Owner != PhotonNetwork.LocalPlayer) return;
        PhotonNetwork.Destroy(view);
    }
}
