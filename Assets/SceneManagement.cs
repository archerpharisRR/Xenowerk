using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagement
{
    static int startLevelBuildIndex = 1;
public static void StartNewGame()
    {
        SceneManager.LoadScene(startLevelBuildIndex);
    }

    public static void LoadGame()
    {
        SaveGameManager.LoadGame();
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
