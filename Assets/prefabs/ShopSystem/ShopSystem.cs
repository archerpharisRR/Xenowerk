using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Shop/ShopSystem")]
public class ShopSystem : ScriptableObject
{
    CreditsSystem creditSystem;
    [SerializeField] Weapon[] weaponsOnSale;
    

    void Start()
    {
    
    }

    internal Weapon[] GetWeaponsOnSale()
    {
        return weaponsOnSale;
    }

    public void PurchaseWeapon(string weaponName)
    {
        if (!HasCreditSystem())
        {
            return;
        }

        foreach(Weapon weapon in weaponsOnSale)
        {
            if(weapon.GetWeaponInfo().name == weaponName)
            {
                Player player = FindObjectOfType<Player>();
                if (player != null && CanPurchase(weapon.GetWeaponInfo().cost))
                {
                    player.AquireNewWeapon(weapon, true);
                    creditSystem.playerCredits -= weapon.GetWeaponInfo().cost;
                }
            }
        }
    }

    public bool CanPurchase(float cost)
    {
        if (!HasCreditSystem())
        {
            return false;
        }
        if(cost > creditSystem.playerCredits)
        {
            return false;
        }
        return true;
    }

    bool HasCreditSystem()
    {
        if (creditSystem == null)
        {
            creditSystem = FindObjectOfType<CreditsSystem>();
        }

        return creditSystem != null;
    }
}
