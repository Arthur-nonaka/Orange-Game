using TMPro;
using UnityEngine;

public class UpgradePoint : MonoBehaviour
{
    public enum UpgradeType
    {
        Capacity,
        OrangeMax,
        OrangeRate,
        NewTree,
        IncreaseValue,
        Discovery,
    }

    public UpgradeType upgradeType;
    public Upgrades upgrades;
    public Discoveries discoveries;
    public DiscoveryType discoveryType;
    public float baseCost = 2f;
    public float costMultiplier = 1.5f;
    public float baseValue = 1f;
    public float valueIncrement = 1.5f;
    public int level = 0;
    public int maxLevel = 5;
    public float percentStep = 0.2f;

    [Header("UI")]
    public TextMeshProUGUI costText;
    public TextMeshProUGUI incrementText;

    void Start()
    {
        UpdateUI();
        ResetUpgrade();
    }

    private void ResetUpgrade()
    {
        level = 0;

        if (upgradeType == UpgradeType.Discovery)
            return;

        switch (upgradeType)
        {
            case UpgradeType.Capacity:
                upgrades.UpgradeCapacity(GetCurrentIncrementInt());
                break;
            case UpgradeType.OrangeMax:
                upgrades.UpgradeOrangeMax(GetCurrentIncrementInt());
                break;
            case UpgradeType.OrangeRate:
                upgrades.UpgradeOrangeRate(GetCurrentIncrementFloat());
                break;
            case UpgradeType.IncreaseValue:
                upgrades.UpgradeOrangeValue(GetCurrentPercent());
                break;
        }
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
        switch (upgradeType)
        {
            case UpgradeType.Capacity:
                upgrades.UpgradeCapacity(GetNextIncrementInt());
                break;
            case UpgradeType.OrangeMax:
                upgrades.UpgradeOrangeMax(GetNextIncrementInt());
                break;
            case UpgradeType.OrangeRate:
                upgrades.UpgradeOrangeRate(GetNextIncrementFloat());
                break;
            case UpgradeType.IncreaseValue:
                upgrades.UpgradeOrangeValue(GetNextPercent());
                break;
            case UpgradeType.Discovery:
                discoveries.UnlockDiscovery(discoveryType);
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

    private float GetCurrentPercent()
    {
        return Mathf.Clamp01(level * percentStep);
    }

    private float GetNextPercent()
    {
        return Mathf.Clamp01((level + 1) * percentStep);
    }

    private int GetNextIncrementInt()
    {
        return Mathf.CeilToInt(baseValue * Mathf.Pow(valueIncrement, level + 1));
    }

    private int GetCurrentIncrementInt()
    {
        if (level == 0)
            return Mathf.CeilToInt(baseValue);
        return Mathf.CeilToInt(baseValue * Mathf.Pow(valueIncrement, level));
    }

    private float GetNextIncrementFloat()
    {
        return baseValue * Mathf.Pow(valueIncrement, level + 1);
    }

    private float GetCurrentIncrementFloat()
    {
        if (level == 0)
            return baseValue;
        return baseValue * Mathf.Pow(valueIncrement, level);
    }

    public void UpdateUI()
    {
        if (IsMaxLevel())
        {
            costText.text = "MAX";
            return;
        }

        costText.text = $"{GetCurrentCost():F2} RS";

        if (upgradeType == UpgradeType.Discovery)
        {
            incrementText.text = discoveries.IsDiscoveryUnlocked(discoveryType)
                ? "UNLOCKED"
                : "LOCKED";
        }
        else if (upgradeType == UpgradeType.OrangeRate)
        {
            incrementText.text =
                $"{GetCurrentIncrementFloat():F2}s -> {GetNextIncrementFloat():F2}s";
        }
        else if (upgradeType == UpgradeType.IncreaseValue)
        {
            incrementText.text = $"{GetCurrentPercent():P0} -> {GetNextPercent():P0}";
        }
        else
        {
            incrementText.text = $"{GetCurrentIncrementInt()} -> {GetNextIncrementInt()}";
        }
    }
}
