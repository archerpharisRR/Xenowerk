using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopMenu : MonoBehaviour
{
    [SerializeField] ShopItem shopItemPrefab;
    [SerializeField] GameObject shopPanelObject;
    [SerializeField] ShopSystem shopSystem;

    // Start is called before the first frame update
    void Start()
    {
        PopulateItems(shopSystem.GetWeaponsOnSale());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopulateItems(Weapon[] weaponsOnSale)
    {
        foreach(Weapon weapon in weaponsOnSale)
        {
            ShopItem newItem = Instantiate(shopItemPrefab, shopPanelObject.transform);
            newItem.Init(weapon.GetWeaponInfo(), shopSystem);
        }
    }



}
