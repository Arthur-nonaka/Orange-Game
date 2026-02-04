using TMPro;
using UnityEngine;

public class UpgradePoint : MonoBehaviour
{
    public enum UpgradeType
    {
        Capacity,
    }

    public UpgradeType upgradeType;
    public Upgrades upgrades;
    public float baseCost = 2f;
    public float costMultiplier = 1.5f;
    public int baseValue = 1;
    public float valueIncrement = 1.5f;
    public int level = 0;
    public int maxLevel = 5;

    [Header("UI")]
    public TextMeshProUGUI costText;
    public TextMeshProUGUI incrementText;

    void Start()
    {
        UpdateUI();
    }

    public bool TryPurchase(PlayerMoney playerMoney)
    {
        if (IsMaxLevel())
            return false;

        if (playerMoney.GetMoney() >= GetCurrentCost())
        {
            playerMoney.SpendMoney(GetCurrentCost());
            ApplyUpgrade();
            level++;
            UpdateUI();
            return true;
        }

        return false;
    }

    private void ApplyUpgrade()
    {
        int increment = GetNextIncrement();

        switch (upgradeType)
        {
            case UpgradeType.Capacity:
                upgrades.UpgradeCapacity(increment);
                break;
            default:
                break;
        }
    }

    public float GetCurrentCost()
    {
        if (level >= maxLevel)
            return 0;

        return baseCost * Mathf.Pow(costMultiplier, level);
    }

    public bool IsMaxLevel()
    {
        return level >= maxLevel;
    }

    private int GetNextIncrement()
    {
        return Mathf.CeilToInt(baseValue * Mathf.Pow(valueIncrement, level + 1));
    }

    private int GetCurrentIncrement()
    {
        if (level == 0)
            return baseValue;

        return Mathf.CeilToInt(baseValue * Mathf.Pow(valueIncrement, level));
    }

    public void UpdateUI()
    {
        costText.text = $"{GetCurrentCost()} RS";
        incrementText.text = $"{GetCurrentIncrement()} -> {GetNextIncrement()}";
    }
}
