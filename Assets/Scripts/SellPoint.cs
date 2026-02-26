using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SellPoint : MonoBehaviour
{
    public PlayerMoney playerMoney;
    private bool isSelling = false;

    public bool SellItems(Inventory inventory)
    {
        if (isSelling)
        {
            return false;
        }

        if (inventory.GetAllItems().Count == 0)
        {
            return false;
        }

        isSelling = true;
        SoundManager.PlaySound(SoundType.SELL, 0.6f);
        StartCoroutine(ProcessSale(inventory));
        return true;
    }

    private IEnumerator ProcessSale(Inventory inventory)
    {
        float totalEarnings = 0f;

        foreach (var itemEntry in inventory.GetAllItems())
        {
            Item item = itemEntry.Key;
            float itemCount = itemEntry.Value;
            float finalPrice = item.price * (1 + item.priceMultiplier);
            totalEarnings += finalPrice * itemCount;
            for (int i = 0; i < itemCount; i++)
            {
                StartCoroutine(AnimationItemSell(item, inventory));
                yield return new WaitForSeconds(0.1f);
            }
        }

        playerMoney.AddMoney(totalEarnings);

        yield return new WaitForSeconds(0.6f);

        isSelling = false;
    }

    private IEnumerator AnimationItemSell(Item item, Inventory inventory)
    {
        GameObject itemObj = Instantiate(
            item.prefab,
            inventory.transform.position,
            Quaternion.identity
        );

        Rigidbody rb = itemObj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider col = itemObj.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        itemObj.transform.DOMove(transform.position, 0.5f).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(0.5f);

        Destroy(itemObj);

        inventory.RemoveItem(item);
    }
}
