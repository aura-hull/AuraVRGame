using UnityEngine;
using System.Collections;
using Photon.Pun.Demo.Cockpit;

public class ProSkaterScript : MonoBehaviour
{
    public float variance;
    public float floatSpeed;
    public Vector3 rotation;
    public Color activeColor = Color.white;
    public float emissionRamp = 0.5f;
    public bool toggleActive;

    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Collider triggerCollider;

    private float t = 0.5f;
    private Vector3 lowerBound;
    private Vector3 upperBound;
    private Material[] originalMaterials;
    private Material[] activeMaterials;

    void Start()
    {
        lowerBound = transform.position - new Vector3(0, variance, 0);
        upperBound = transform.position + new Vector3(0, variance, 0);

        if (meshRenderers != null)
        {
            originalMaterials = new Material[meshRenderers.Length];
            activeMaterials = new Material[meshRenderers.Length];

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                originalMaterials[i] = meshRenderers[i].material;
                activeMaterials[i] = new Material(originalMaterials[i]);
                activeMaterials[i].color = activeColor;
                activeMaterials[i].SetColor("_EmissionColor", activeColor * emissionRamp);
            }
        }

        SetActive(false);
    }

    void Update()
    {
        t += floatSpeed;

        transform.position = new Vector3(transform.position.x,
            Mathf.Lerp(lowerBound.y, upperBound.y, t), transform.position.z);

        if (t >= 1f || t <= 0)
            floatSpeed *= -1;

        transform.Rotate(rotation);

        if (toggleActive && triggerCollider != null)
        {
            SetActive(!triggerCollider.enabled);
            toggleActive = false;
        }
    }

    public void SetActive(bool value)
    {
        if (triggerCollider != null)
        {
            triggerCollider.enabled = value;
        }

        if (meshRenderers == null) return;

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].material = (value ? activeMaterials[i] : originalMaterials[i]);
            }
        }
    }
}