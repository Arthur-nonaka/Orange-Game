using UnityEngine;

public class DisableOutline : MonoBehaviour
{
    void Start()
    {
        Outline outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void Update() { }
}
