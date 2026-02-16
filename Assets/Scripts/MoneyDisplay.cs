using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public PlayerMoney playerMoney;

    void Start()
    {
        playerMoney.OnMoneyChanged += UpdateMoneyDisplay;
        UpdateMoneyDisplay(playerMoney.GetMoney());
    }

    void UpdateMoneyDisplay(float newAmount)
    {
        moneyText.text = $"{newAmount:F2}";
        StartCoroutine(PlaySoundAfterDelay());
    }

    System.Collections.IEnumerator PlaySoundAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.PlaySound(SoundType.BUY, 0.04f);
    }

    void OnDestroy()
    {
        playerMoney.OnMoneyChanged -= UpdateMoneyDisplay;
    }
}
