using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnableShop : MonoBehaviour
{
    [SerializeField] GameObject shopMenu;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        //shopMenu.SetActive(true);

        if (shopMenu.activeSelf)
        {
            shopMenu.SetActive(false);
        }
        else
        {
            shopMenu.SetActive(true);
        }





    }

    public void OnClickExit()
    {
        shopMenu.SetActive(false);
    }
}
