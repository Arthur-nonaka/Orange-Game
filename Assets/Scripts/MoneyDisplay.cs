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
    }

    void OnDestroy()
    {
        playerMoney.OnMoneyChanged -= UpdateMoneyDisplay;
    }
}
