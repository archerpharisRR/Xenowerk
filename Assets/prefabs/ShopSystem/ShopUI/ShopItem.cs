using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    ShopSystem _shopSystem;

    [SerializeField]TextMeshProUGUI text;
    [SerializeField]Image Icon;
    [SerializeField] Button button;
    [SerializeField] CreditsSystem cs;
    bool alreadyBought;
    
    public WeaponInfo WeaponInfo
    {
        get;
        private set;
    }
    // Start is called before the first frame update
    void Start()
    {
        cs = FindObjectOfType<CreditsSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        if(cs.playerCredits < WeaponInfo.cost)
        {
            button.interactable = false;
        }
        else if(cs.playerCredits > WeaponInfo.cost && !alreadyBought)
        {
            button.interactable = true;
        }


        
    }

    internal void Init(WeaponInfo weaponInfo, ShopSystem shopSystem)
    {
        _shopSystem = shopSystem;
        WeaponInfo = weaponInfo;
        Icon.sprite = weaponInfo.Icon;
        text.text = $"{weaponInfo.name}\n" +
            $"Rate: { weaponInfo.ShootSpeed}\n" +
            $"Damage: { weaponInfo.DamagePerBullet}\n" +
            $"Price: { weaponInfo.cost}";

    }

    public void Purchase()
    {
        _shopSystem.PurchaseWeapon(WeaponInfo.name);
        alreadyBought = true;
        
    }

    public void TurnOffButton()
    {
        button.interactable = false;
    }
}
