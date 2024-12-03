using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;

    public void SetMaxHP(float maxHP)
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = maxHP;
        }
    }

    public void UpdateHP(float currentHP)
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }
}