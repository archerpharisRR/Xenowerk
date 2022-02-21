using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource aS;

    Weapon weapon;
    // Start is called before the first frame update

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    public void PlayAudio()
    {
        aS.clip = clip;
        aS.Play();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
