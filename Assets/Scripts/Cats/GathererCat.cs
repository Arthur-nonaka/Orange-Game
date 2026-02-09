using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GathererCat : MonoBehaviour
{
    public Spawner spawner;
    public float actionDelay;
    public float size;

    public Crate crate;

    private float actionTimer;

    void Update()
    {
        actionTimer += Time.deltaTime;

        if (actionTimer >= actionDelay)
        {
            PerformAction();
            actionTimer = 0f;
        }
    }

    private void PerformAction()
    {
        GameObject item = spawner.GetNextItem();
        if (item != null && !crate.IsMax())
        {
            AnimateItemToCrate(item);
        }
    }

    private void AnimateItemToCrate(GameObject item)
    {
        Collider itemCollider = item.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        item.transform.DOMove(crate.transform.position, 0.5f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                crate.InsertItem(item.GetComponent<Grabbable>(), size);
                item.GetComponent<Grabbable>().NotifySpawner();
                item.gameObject.SetActive(false);
                Destroy(item.gameObject);
            });
    }
}
