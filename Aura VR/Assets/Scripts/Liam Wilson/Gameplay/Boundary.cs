using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public class Boundary : MonoBehaviour
    {
        public static float MIN_DISTANCE = 500.0f;

        private Material _materialInstance;
        private float sinceLastUpdateVisual = 0.0f;
        private float lastDistance = MIN_DISTANCE;

        void Awake()
        {
            _materialInstance = GetComponent<MeshRenderer>().material;
        }

        void Update()
        {
            sinceLastUpdateVisual += Time.deltaTime;
            if (lastDistance < MIN_DISTANCE && sinceLastUpdateVisual >= 0.1f)
            {
                // Reset boundary color
                UpdateVisual(MIN_DISTANCE);
            }
        }

        public void UpdateVisual(float distance)
        {
            if (_materialInstance == null)
            {
                return;
            }

            Color color = _materialInstance.GetColor("_Color");
            color.a = 1.0f - Mathf.Clamp01(distance / MIN_DISTANCE);
            _materialInstance.SetColor("_Color", color);

            sinceLastUpdateVisual = 0.0f;
            lastDistance = distance;
        }
    }
}