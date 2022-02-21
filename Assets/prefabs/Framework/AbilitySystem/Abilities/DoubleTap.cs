using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DoubleTap")]
public class DoubleTap : AbilityBase
{

    [SerializeField] Player player;
    [SerializeField] Animator playerAnimator;


    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        player = OwnerComp.GetComponent<Player>();
        playerAnimator = OwnerComp.GetComponent<Animator>();
    }

    public override void ActivateAbility()
    {
        if (CommitAbility())
        {
            player.CurrentWeapon.shootingSpeed = player.CurrentWeapon.shootingSpeed * 1.5f;
            playerAnimator.SetFloat("FiringSpeed", player.CurrentWeapon.shootingSpeed);
            Debug.Log("Ability should activate");
            OwnerComp.StartCoroutine(HealthRegenCoroutine());
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        yield return new WaitForSeconds(5);
        player.CurrentWeapon.shootingSpeed = player.CurrentWeapon.shootingSpeed / 1.5f;
        playerAnimator.SetFloat("FiringSpeed", player.CurrentWeapon.shootingSpeed);
    }


}
