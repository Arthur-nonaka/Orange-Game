using DG.Tweening;
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
            )
            {
                handIcon.enabled = true;
                return;
            }
        }

        handIcon.enabled = false;
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
                upgradePoint.TryPurchase(playerMoney);
                return;
            }
        }
    }

    private void PickupItem(Grabbable grabbable)
    {
        if (inventory.GetTotalItemCount() >= inventory.maxSize)
        {
            ShakeHand();
            return;
        }

        grabbable.GetComponent<Collider>().enabled = false;
        grabbable.NotifySpawner();
        grabbable
            .transform.DOMove(inventory.transform.position, 0.5f)
            .OnComplete(() =>
            {
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
                strength: 5f,
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
