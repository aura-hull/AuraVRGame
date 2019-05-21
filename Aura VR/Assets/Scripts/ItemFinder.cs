using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemFinder
{
    public static List<GameObject> FindItems(Transform scanTransform, float scanRadius)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("BuildPart");

        List<GameObject> itemsInRange = new List<GameObject>();

        for (int i = 0; i < items.Length; i += 1)
        {
            // Check range
            Transform itemTransform = items[i].transform;
            Vector2 itemPosition = new Vector2(itemTransform.position.x, itemTransform.position.z);
            Vector2 scanPosition = new Vector2(scanTransform.position.x, scanTransform.position.z);

            float distance = Vector2.Distance(scanPosition, itemPosition);

            if (distance <= scanRadius)
            {
                itemsInRange.Add(items[i]);
            }
        }

        return itemsInRange;
    }
}
