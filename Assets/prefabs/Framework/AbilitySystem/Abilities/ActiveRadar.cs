using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Active Radar")]
public class ActiveRadar : AbilityBase
{

    [SerializeField] Radar radar;
    GameObject[] blips;



    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        radar = FindObjectOfType<Radar>();
        blips = GameObject.FindGameObjectsWithTag("Blip");
        
    }
    public override void ActivateAbility()
    {
        if (CommitAbility())
        {
            radar.enabled = true;
            OwnerComp.StartCoroutine(HealthRegenCoroutine());
      
        }
    }



    private IEnumerator HealthRegenCoroutine()
    {
          
        yield return new WaitForSeconds(5);
        radar.RemoveAllBlips();
        radar.enabled = false;

     

        


    }
}
