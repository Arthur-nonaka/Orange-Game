using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoney : MonoBehaviour
{
    private float amount = 0;

    public delegate void MoneyChangedDelegate(float newAmount);
    public event MoneyChangedDelegate OnMoneyChanged;

    public void SetHack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AddMoney(1000f);
        }
    }

    public void AddMoney(float value)
    {
        amount += value;
        OnMoneyChanged?.Invoke(amount);
    }

    public bool SpendMoney(float value)
    {
        if (amount >= value)
        {
            amount -= value;
            OnMoneyChanged?.Invoke(amount);
            return true;
        }
        return false;
    }

    public float GetMoney()
    {
        return amount;
    }
}
