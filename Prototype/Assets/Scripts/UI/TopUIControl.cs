using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TopUIControl : MonoBehaviour
{
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Transform waveContainer;
    [SerializeField] private GameObject waveUIPrefab;
    [SerializeField] private TextMeshProUGUI fromLevel;
    [SerializeField] private TextMeshProUGUI toLevel;

    [SerializeField] private Color activeWaveColor;
    [SerializeField] private Color unactiveWaveColor;

    private int healthTotal;

    private void Start()
    {
        sliderHealth.value = 0;
    }

    public void ResetWaveUI(int waveIndex)
    {
        healthTotal = 0;
        sliderHealth.value = 0;

        ActiveWave(waveIndex);
    }

    private void ActiveWave(int waveIndex)
    {
        var wave = waveContainer.GetChild(waveIndex).GetComponent<RawImage>();

        wave.DOColor(activeWaveColor, 0.85f);
    }

    public void SetupWaveAndLevel(int level, int waveNumber)
    {
        //Level
        fromLevel.text = level.ToString();
        toLevel.text = (level + 1).ToString();

        //waves
        int waveOld = waveContainer.childCount;
        if (waveOld == waveNumber)
            return;
        else if(waveOld < waveNumber)
        {
            int increase = waveNumber - waveOld;
            for (int i = 0; i < increase; i++)
            {
                var waveObj = Instantiate(waveUIPrefab, waveContainer) as GameObject;
                //waveObj.transform.SetParent(waveContainer);
            }
        }
        else if (waveOld > waveNumber)
        {
            int decrease = waveOld - waveNumber;
            for (int i = 0; i < decrease; i++)
            {
                Destroy(waveContainer.GetChild(i).gameObject);
            }
        }

        //Reset wave element
        ResetWaveElementUI();
    }

    private void ResetWaveElementUI()
    {
        sliderHealth.value = 0;

        for (int i = 0; i < waveContainer.childCount; i++)
        {
            waveContainer.GetChild(i).GetComponent<RawImage>().color = unactiveWaveColor;
        }
    }

    public void AddHealth(int health)
    {
        healthTotal += health;
    }

    public void InteractWithSlider(int damaged)
    {
        sliderHealth.value += Decrease(damaged);
    }


    private float Decrease(int damaged)
    {
        float ratio = damaged / (float)healthTotal;
        return ratio;
    }
}
