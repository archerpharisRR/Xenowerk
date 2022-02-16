using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnNeWAbilityInitialized(AbilityBase newAbility);
public delegate void OnStaminaUpdated(float newValue);

public class AbilityComponent : MonoBehaviour
{

    [SerializeField] float staminaLevel;
    [SerializeField] float maxStaminaLevel;
    [SerializeField] float staminaDropSpeed = 0.5f;
    [SerializeField] float staminaDrainingStartDelay = 2.0f;

    [SerializeField] AbilityBase[] abilities;

    public event OnNeWAbilityInitialized onNewAbilityInit;
    public event OnStaminaUpdated onStamianUpdated;
    Coroutine staminaDrainingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
            onNewAbilityInit.Invoke(abilities[i]);

            
        }
        staminaDrainingCoroutine = StartCoroutine(StaminaDrainingCoroutine());
    }

    public void ChangeStamina(float changeAmount)
    {
        if(changeAmount > 0)
        {
            if(staminaDrainingCoroutine != null)
            {
                StopCoroutine(staminaDrainingCoroutine);
                staminaDrainingCoroutine = StartCoroutine(StaminaDrainingCoroutine());
            }
        }

        staminaLevel = Mathf.Clamp(staminaLevel + changeAmount, 0, maxStaminaLevel);
        onStamianUpdated?.Invoke(staminaLevel);
    }

    IEnumerator StaminaDrainingCoroutine()
    {
        yield return new WaitForSeconds(staminaDrainingStartDelay);
        while (staminaLevel > 0)
        {

            staminaLevel -= staminaDropSpeed * Time.deltaTime;
            onStamianUpdated.Invoke(staminaLevel);
            yield return new WaitForEndOfFrame();

        }
        staminaLevel = Mathf.Clamp(staminaLevel,0, maxStaminaLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal float GetStaminaLevel()
    {
        return (int)staminaLevel;
    }

}
