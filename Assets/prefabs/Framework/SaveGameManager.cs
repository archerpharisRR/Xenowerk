using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public static class SaveGameManager
{
    static string SaveDir = Application.persistentDataPath + "/zombieGameSave.json";
    static SaveGameData currentLoadedData;
    static bool isDead = false;

    static public void SaveGame()
    {
        Player player = GameObject.FindObjectOfType<Player>();
        Zombie zombie = GameObject.FindObjectOfType<Zombie>();
        if(player == null)
        {
            return;
        }

 

        SaveGameData data = new SaveGameData();
        data.LevelIndex = SceneManager.GetActiveScene().buildIndex;
        data.SaveTime = System.DateTime.Now.ToString();
        data.PlayerData = player.GenerateSaveData();

        if (zombie == null)
        {
            isDead = true;
            return;
        }


        data.ZombieData = zombie.GenerateData();

    
        string playerData = JsonUtility.ToJson(data, true);

        Debug.Log(playerData);
        File.WriteAllText(SaveDir, playerData);

        
    }

    
    
    static public void LoadGame()
    {
  

        string saveDataString = File.ReadAllText(SaveDir);
        currentLoadedData = JsonUtility.FromJson<SaveGameData>(saveDataString);
      
        SceneManager.LoadScene(currentLoadedData.LevelIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Time.timeScale = 1;






        //int playerCurrentCredits = GameObject.FindObjectOfType<CreditsSystem>().GetCredits();
        //add credit functionality later

    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player player = GameObject.FindObjectOfType<Player>();
        Zombie zombie = GameObject.FindObjectOfType<Zombie>();
        if (player == null)
        {
            return;
        }
        player.UpdateFromSaveData(currentLoadedData.PlayerData);
        zombie.UpdateSaveData(currentLoadedData.ZombieData);
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (isDead)
        {
            zombie.DestroyZombie();
        }
    }
}
[Serializable]
public struct SaveGameData
{

    public SaveGameData(int levelIndex, PlayerSaveData playerData, ZombieSaveData zombieData, string saveTime)
    {
        LevelIndex = levelIndex;
        PlayerData = playerData;
        ZombieData = zombieData;
        SaveTime = saveTime;
    }

    public int LevelIndex;
    public PlayerSaveData PlayerData;
    public ZombieSaveData ZombieData;
    public string SaveTime;
}
