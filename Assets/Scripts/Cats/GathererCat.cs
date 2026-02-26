using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GathererCat : MonoBehaviour
{
    public Spawner spawner;
    public float actionDelay;
    public float size;
    public float walkSpeed = 2f;

    public Crate crate;

    private float actionTimer;
    private bool hasReachedPosition = false;

    void Update()
    {
        if (!hasReachedPosition)
            return;

        actionTimer += Time.deltaTime;

        if (actionTimer >= actionDelay)
        {
            PerformAction();
            actionTimer = 0f;
        }
    }

    public void WalkToPosition(Vector3 targetPosition, Quaternion targetRotation, Action close)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / walkSpeed;

        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 0.3f);
        }

        transform
            .DOMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                hasReachedPosition = true;
                transform.rotation = targetRotation;
                TerrainUpgradeManager.Instance.ApplyUpgrade("Cat");
                close?.Invoke();
            });
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
