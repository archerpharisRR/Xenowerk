using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityBase : ScriptableObject
{

    public float CooldownTime = 5.0f;
    [SerializeField] Sprite Icon;
    [SerializeField] int AbilityLevel = 1;

    public AbilityComponent OwnerComp
    {
        get;
        set;
    }
    public bool isOnCooldown
    {
        private set;
        get;
    }

    public int GetLevel()
    {
        return AbilityLevel;
    }

    public Sprite GetIcon()
    {
        return Icon;
    }

    public virtual void Init(AbilityComponent ownerAbilityComp)
    {
        OwnerComp = ownerAbilityComp;
        isOnCooldown = false;
    }

    bool CanCast()
    {
        return (!isOnCooldown) && OwnerComp.GetStaminaLevel() >= AbilityLevel;
    }

    public abstract void ActivateAbility();

    protected bool CommitAbility()
    {
        if (CanCast())
        {
            StartCooldown();
            return true;
        }
        return false;
    }

    private void StartCooldown()
    {
        OwnerComp.StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(CooldownTime);

        isOnCooldown = false;
        yield return null;
    }
}
