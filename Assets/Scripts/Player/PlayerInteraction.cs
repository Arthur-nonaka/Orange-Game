using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Transform cameraTransform;
    public float grabDistance = 3f;
    public Inventory inventory;
    public Image handIcon;
    public SellPoint sellPoint;
    public PlayerMoney playerMoney;

    private int pendingItems = 0;
    private GameObject currentHighlightedObject;
    private Outline currentOutline;

    void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            if (
                hit.collider.GetComponent<Grabbable>() != null
                || hit.collider.GetComponent<SellPoint>() != null
                || hit.collider.GetComponent<UpgradePoint>() != null
                || hit.collider.GetComponent<Crate>() != null
            )
            {
                handIcon.enabled = true;
                if (currentHighlightedObject != hit.collider.gameObject)
                {
                    if (currentOutline != null)
                    {
                        currentOutline.enabled = false;
                    }

                    currentHighlightedObject = hit.collider.gameObject;
                    currentOutline = currentHighlightedObject.GetComponent<Outline>();

                    if (currentOutline != null)
                    {
                        currentOutline.enabled = true;
                    }
                }
                return;
            }
        }

        handIcon.enabled = false;
        DisableCurrentOutline();
    }

    private void DisableCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
        currentHighlightedObject = null;
        currentOutline = null;
    }

    public void SetInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                PickupItem(grabbable);
                return;
            }

            SellPoint sellPoint = hit.collider.GetComponent<SellPoint>();
            if (sellPoint != null)
            {
                if (!sellPoint.SellItems(inventory))
                {
                    ShakeHand();
                }
                return;
            }

            UpgradePoint upgradePoint = hit.collider.GetComponent<UpgradePoint>();
            if (upgradePoint != null)
            {
                if (!upgradePoint.TryPurchase(playerMoney))
                {
                    ShakeHand();
                }
                return;
            }

            Crate crate = hit.collider.GetComponent<Crate>();
            if (crate != null)
            {
                if (inventory.GetTotalItemCount() + pendingItems >= inventory.maxSize)
                {
                    ShakeHand();
                    return;
                }

                Grabbable item = crate.GetItem();
                if (item != null)
                {
                    PickupItemFromCrate(item);
                }
                else
                {
                    ShakeHand();
                }
                return;
            }
        }
    }

    private void PickupItem(Grabbable grabbable)
    {
        if (inventory.GetTotalItemCount() + pendingItems >= inventory.maxSize)
        {
            ShakeHand();
            return;
        }
        pendingItems++;
        grabbable.GetComponent<Collider>().enabled = false;
        grabbable
            .transform.DOMove(inventory.transform.position, 0.5f)
            .OnComplete(() =>
            {
                pendingItems--;
                inventory.AddItem(grabbable.GetItem());
                grabbable.gameObject.SetActive(false);
                grabbable.NotifySpawner();
            });
    }

    private void PickupItemFromCrate(Grabbable grabbable)
    {
        pendingItems++;
        grabbable.GetComponent<Collider>().enabled = false;
        grabbable
            .transform.DOMove(inventory.transform.position, 0.5f)
            .OnComplete(() =>
            {
                pendingItems--;
                inventory.AddItem(grabbable.GetItem());
                grabbable.gameObject.SetActive(false);
                Destroy(grabbable.gameObject);
            });
    }

    private void ShakeHand()
    {
        handIcon
            .transform.DOShakePosition(
                0.3f,
                strength: 10f,
                vibrato: 10,
                randomness: 1f,
                snapping: false,
                fadeOut: true
            )
            .OnComplete(() =>
            {
                handIcon.transform.localPosition = Vector3.zero;
            });
    }
}
