using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Abilities/SpeedBoost")]
public class SpeedUp : AbilityBase
{
    [SerializeField] float SpeedBoostAmount;
    [SerializeField] float BuffTime;

    MovementComponent movementComp;

    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        movementComp = OwnerComp.GetComponent<MovementComponent>();
    }


    public override void ActivateAbility()
    {
        if (CommitAbility())
        {
            movementComp.WalkingSpeed = SpeedBoostAmount;
            OwnerComp.StartCoroutine(HealthRegenCoroutine());
            
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        yield return new WaitForSeconds(5);
        movementComp.WalkingSpeed = 5;
    }


}
