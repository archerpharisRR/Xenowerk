using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    [SerializeField] RectTransform Background;
    [SerializeField] RectTransform Icon;

    [SerializeField] RectTransform CoolDown;
    [SerializeField] float expandSpeed = 1.5f;
    [SerializeField] float highlightedScale = 2.0f;
    [SerializeField] float ExpandedScale = 1.5f;
    [SerializeField] HealthRegen healthRegen;
    Vector3 GoalScale = new Vector3(1, 1, 1);
    float progress = 0;
    float changePerSecond = 1f;
    public bool abilityOnCooldown;


    AbilityBase ability;

    Material CooldownMat;
    Material StaminaMat;

    // Start is called before the first frame update
    void Start()
    {
        CooldownMat = Instantiate(CoolDown.GetComponent<Image>().material);
        CoolDown.GetComponent<Image>().material = CooldownMat;

        StaminaMat = Instantiate(Background.GetComponent<Image>().material);
        Background.GetComponent<Image>().material = StaminaMat;
    }


    float RechargeRate()
    {
        float rechargeTime = changePerSecond / healthRegen.CooldownTime;
        return rechargeTime;
    }


    // Update is called once per frame
    void Update()
    {
        

        Background.localScale = Vector3.Lerp(Background.localScale, GoalScale, Time.deltaTime * expandSpeed);

        if (abilityOnCooldown)
        {
            progress += RechargeRate() * Time.deltaTime;
            progress = Mathf.Clamp(progress, 0, 1);
            CooldownMat.SetFloat("_Progress", progress);


        }
        if (progress == 1)
        {

            abilityOnCooldown = false;
            progress = 0;
            SetCooldownProgress(1);
        }
    }

    public void SetCooldownProgress(float progress)
    {
        if (CooldownMat != null)
        {
            CooldownMat.SetFloat("_Progress", progress);
        }
    }

    public void SetStaminaProgress(float progress)
    {
        if (StaminaMat != null)
        {

            StaminaMat.SetFloat("_Progress", progress);
        }
    }

    internal void SetExpand(bool isExpanded)
    {
        if (isExpanded)
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;

           

        }
        else
        {

            if (IsHighlighted() && ability != null)
            {
                ability.ActivateAbility();
                abilityOnCooldown = true;
                SetCooldownProgress(progress);
            }

            GoalScale = new Vector3(1, 1, 1);

        }
    }

    private bool IsHighlighted()
    {
        return GoalScale == new Vector3(1, 1, 1) * highlightedScale;
    }

    internal void SetHighLighted(bool isHighlighted)
    {
        if (isHighlighted)
        {
            GoalScale = new Vector3(1, 1, 1) * highlightedScale;
        }
        else
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
    }

    public Vector2 GetCenter()
    {
        return Background.rect.center;
    }

    internal void AssignAbility(AbilityBase newAbility)
    {
        ability = newAbility;
        Icon.GetComponent<Image>().sprite = ability.GetIcon();
    }
}
