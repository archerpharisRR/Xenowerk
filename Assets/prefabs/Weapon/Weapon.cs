using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponInfo
{
    public string name;
    public Sprite Icon;
    public float DamagePerBullet;
    public float ShootSpeed;
    public int cost;
}

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform FirePoint;
    [SerializeField] ParticleSystem BulletEmitter;
    [SerializeField] float DamagePerBullet = 1;
    [SerializeField] Sprite WeaponIcon;
    public float shootingSpeed = 1.0f;
    [SerializeField] string weaponName;
    [SerializeField] int cost;
    [SerializeField] AudioClip firingSound;
    public WeaponInfo GetWeaponInfo()
    {
        return new WeaponInfo()
        {
            Icon = WeaponIcon,
            name = weaponName,
            DamagePerBullet = this.DamagePerBullet,
            ShootSpeed = this.shootingSpeed,
            cost = this.cost
        };
    }

    public AudioClip FiringSound()
    {
        return firingSound;
    }

    public Sprite GetWeaponIcon() { return WeaponIcon; }
    public float GetDamagePerBullet() { return DamagePerBullet; }
    public GameObject Owner { set; get; }
    public Weapon originalPrefab { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Equip()
    {
        gameObject.SetActive(true);        
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public void Fire()
    {


        if(BulletEmitter)
        {
            BulletEmitter.Emit(BulletEmitter.emission.GetBurst(0).maxCount);
        }

    }
}
