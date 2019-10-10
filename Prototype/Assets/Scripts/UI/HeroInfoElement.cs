using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoElement : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private RawImage img;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI dpsText;

    [Header("Update Info")]
    [SerializeField] private TextMeshProUGUI hpIncreaseText;
    [SerializeField] private TextMeshProUGUI damageIncreaseText;
    [SerializeField] private TextMeshProUGUI dpsIncreaseText;


    public void SetupImage(Texture texture)
    {
        img.texture = texture;
    }

    public void SetupInfo(string name, string level, string hp, string dps)
    {
        nameText.text = "Name: " + name;
        levelText.text = "Level: " + level;
        hpText.text = "Hp: " + hp;
        dpsText.text = "Dps: " + dps;
    }

    public void SetupUpdateInfo(string hp, string damage, string dps)
    {
        hpIncreaseText.text = "+" + hp + "Hp";
        damageIncreaseText.text = "+" + damage + "Damage";
        dpsIncreaseText.text = "+" + dps + "Dps";
    }
}
