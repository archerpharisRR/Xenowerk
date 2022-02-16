using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void StartGameBtnClicked()
    {
        SceneManagement.StartNewGame();
    }

    public void LoadBtnClicked()
    {
        SceneManagement.LoadGame();
    }

    public void QuitBtnClicked()
    {
        SceneManagement.QuitGame();
    }
}
