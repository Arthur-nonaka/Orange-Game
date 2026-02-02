using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SellPoint : MonoBehaviour
{
    public PlayerMoney playerMoney;

    public void SellItems(Inventory inventory)
    {
        float totalEarnings = 0f;

        foreach (var itemEntry in inventory.GetAllItems())
        {
            Item item = itemEntry.Key;
            float itemCount = itemEntry.Value;
            totalEarnings += item.price * itemCount;

            for (int i = 0; i < itemCount; i++)
            {
                StartCoroutine(AnimationItemSell(item, inventory));
            }
        }

        playerMoney.AddMoney(totalEarnings);
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
