using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI playerClass;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerHp;
    [SerializeField] private TextMeshProUGUI playerAttackDamage;
    [SerializeField] private TextMeshProUGUI playerHitRate;
    [SerializeField] private TextMeshProUGUI playerEvasion;

    public void TextSetting(CardGamePlayer player)
    {
        slider.maxValue = player.hp;
        slider.value = player.hp;
        playerClass.text = "Class     :   " + player.className;
        playerHp.text = "HP         :   " + player.hp.ToString();
        playerAttackDamage.text = "Damage:   " + player.attackDamage.ToString();
        playerHitRate.text = "HitRate  :   " + player.status.hitRate.ToString();
        playerEvasion.text = "Evasion :   " + player.status.evasion.ToString();
    }
}
