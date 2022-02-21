using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InGameUI igUI;

    public void OnPausePressed()
    {
        
        //Time.timeScale = 0;
        igUI.SWitchToPauseMenu();

        
    }
}
