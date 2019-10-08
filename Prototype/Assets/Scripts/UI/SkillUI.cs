using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillUI;
    [SerializeField] private GameObject lockUI;
    [SerializeField] RectTransform cooldownImg;

    [HideInInspector]
    public Hero heroTarget;

    private bool activedSkill;
    private float timeCooldownSkill;
    private bool cooldown = false;

    //Temp
    public bool isLock = true;


    private void Start()
    {
        if (isLock)
            LockSkill();
        else
            ActiveSkill();
    }

    private void Update()
    {
        if (timeCooldownSkill > 0)
            timeCooldownSkill -= Time.deltaTime;
    }
    
    public void ActiveSkill()
    {
        activedSkill = true;
        skillUI.SetActive(true);
        lockUI.SetActive(false);
    }

    public void LockSkill()
    {
        activedSkill = false;
        skillUI.SetActive(false);
        lockUI.SetActive(true);
    }

    public void SetImgSkill(Sprite img)
    {
        skillUI.GetComponent<Image>().sprite = img;
    }

    public void UsingSkill(float timeCooldown)
    {
        if (cooldown || !activedSkill || heroTarget == null)
            return;

        cooldown = true;
        timeCooldownSkill = timeCooldown;

        var colorUI = skillUI.GetComponent<Image>().color;
        skillUI.GetComponent<Image>().color = new Color(colorUI.r, colorUI.g, colorUI.b, 0.5f);

        cooldownImg.gameObject.SetActive(true);
        cooldownImg.anchoredPosition = Vector3.zero;
        cooldownImg.DOAnchorPosY(-cooldownImg.rect.height, heroTarget.TimeSkill).OnComplete(() => 
        {
            cooldown = false;
            cooldownImg.gameObject.SetActive(false);
            skillUI.GetComponent<Image>().color = new Color(colorUI.r, colorUI.g, colorUI.b, 0.95f);
        });


        //Hero using Skill
        heroTarget.UsingSkillActive();
    }
}
