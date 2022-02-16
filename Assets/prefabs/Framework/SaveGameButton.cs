using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveGameButton : MonoBehaviour
{
    [SerializeField] InGameUI igUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        igUI.SwichToInGameMenu();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SaveGame()
    {
        SaveGameManager.SaveGame();
    }

    public void LoadGame()
    {
        SaveGameManager.LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
