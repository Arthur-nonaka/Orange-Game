using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    [Header("Outline Settings")]
    public Color outlineColor = Color.white;
    public float outlineIntensity = 2f;

    private Renderer objectRenderer;
    private Material[] originalMaterials;
    private Color[] originalEmissionColors;
    private bool[] hadEmission;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<Renderer>();
        }

        if (objectRenderer != null)
        {
            originalMaterials = objectRenderer.materials;
            originalEmissionColors = new Color[originalMaterials.Length];
            hadEmission = new bool[originalMaterials.Length];

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                if (originalMaterials[i].HasProperty("_EmissionColor"))
                {
                    originalEmissionColors[i] = originalMaterials[i].GetColor("_EmissionColor");
                    hadEmission[i] = originalMaterials[i].IsKeywordEnabled("_EMISSION");
                }
            }
        }
    }

    public void ShowOutline()
    {
        if (objectRenderer == null)
            return;

        foreach (Material mat in objectRenderer.materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", outlineColor * outlineIntensity);
            }
        }
    }

    public void HideOutline()
    {
        if (objectRenderer == null)
            return;

        for (int i = 0; i < originalMaterials.Length && i < objectRenderer.materials.Length; i++)
        {
            Material mat = objectRenderer.materials[i];
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.SetColor("_EmissionColor", originalEmissionColors[i]);
                if (!hadEmission[i])
                {
                    mat.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}
