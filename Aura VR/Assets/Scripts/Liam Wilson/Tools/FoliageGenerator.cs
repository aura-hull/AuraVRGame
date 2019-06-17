using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class FoliageGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private LayerMask paintLayer;
    [SerializeField] private float range = 200.0f;
    [SerializeField] private float minHeight = 0.0f;
    [SerializeField] private float maxHeight = 1000.0f;
    [SerializeField] private int clusterCount = 5;
    [SerializeField] private int maximumPerCluster = 15;
    [SerializeField] private float clusterRange = 50.0f;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private float prefabScale = 1.0f;
    [SerializeField] private bool generate = false;

    void Update()
    {
        if (!generate) return;

        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        int attempts = 0;
        RaycastHit info;

        for (int i = 0; i < clusterCount; i++)
        {
            if (attempts++ >= 50) break;

            Vector3 clusterPosition = transform.position + new Vector3(Random.Range(-range, range), 0.0f, Random.Range(-range, range));
            Ray ray = new Ray(clusterPosition, Vector3.down);

            if (Physics.Raycast(ray, out info, 500.0f, paintLayer))
            {
                for (int j = 0; j < maximumPerCluster; j++)
                {
                    Vector2 circ = (Random.insideUnitCircle * clusterRange);
                    Vector3 treePosition = clusterPosition + new Vector3(circ.x, 0.0f, circ.y);
                    ray = new Ray(treePosition, Vector3.down);

                    if (Physics.Raycast(ray, out info, 500.0f, paintLayer))
                    {
                        if (info.point.y < minHeight || info.point.y > maxHeight) continue;

                        int randomTreeIndex = Random.Range(0, treePrefabs.Length);

                        GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(treePrefabs[randomTreeIndex], transform);
                        newObject.transform.position = info.point;
                        newObject.transform.localScale *= prefabScale;
                    }
                }
            }
            else i--;
        }

        generate = false;
    }
#endif
}
