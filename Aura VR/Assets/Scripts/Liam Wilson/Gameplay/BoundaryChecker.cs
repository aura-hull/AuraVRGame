using Photon.Pun;
using UnityEngine;
using VRTK;

namespace AuraHull.AuraVRGame
{
    public class BoundaryChecker : MonoBehaviour
    {
        private static Vector3[] checkDirs =
        {
            Vector3.forward, Vector3.back,
            Vector3.left, Vector3.right
        };

        void FixedUpdate()
        {
            int checkMask = LayerMask.GetMask("WorldBoundaries");
            RaycastHit info;

            foreach (Vector3 dir in checkDirs)
            {
                if (Physics.Raycast(transform.position, dir, out info, Boundary.MIN_DISTANCE, checkMask))
                {
                    Boundary b = info.transform.GetComponent<Boundary>();

                    if (b != null)
                    {
                        b.UpdateVisual(info.distance);
                    }
                }
            }
        }
    }
}