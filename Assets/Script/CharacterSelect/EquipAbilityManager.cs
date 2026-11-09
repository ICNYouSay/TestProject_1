using UnityEngine;
using UnityEngine.UI;

public class EquipAbilityManager : MonoBehaviour
{
    [Header("ÉAÉròg")]
    public Button[] EquipAbilityButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEquipAbility(int index, Sprite abilityIcon)
    {
        if (index < 0 || index >= EquipAbilityButtons.Length)
        {
            Debug.LogError("ë∂ç›ÇµÇ»Ç¢î‘çÜìnÇ≥ÇÍÇΩÇ…ÇÂÅ`ÇÒÇóÇóÇó: " + index);
            return;
        }
        EquipAbilityButtons[index].image.sprite = abilityIcon;
    }
}
