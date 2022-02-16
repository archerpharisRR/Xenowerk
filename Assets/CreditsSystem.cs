using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CreditsSystem : MonoBehaviour
{
    public int playerCredits = 0;
    [SerializeField] TMP_Text rewardText;
    
    public int GetCredits()
    {
        return playerCredits;
    }

    public void ChangeCredits(int currentCredits)
    {
        rewardText.text = playerCredits.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        rewardText.text = playerCredits.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        rewardText.text = playerCredits.ToString();
    }
}
