using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InGameUI igUI;
    public void OnPausePressed()
    {
        igUI.SWitchToPauseMenu();
    }
}
