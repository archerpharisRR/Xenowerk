using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : Character
{
    NavMeshAgent navAgent;
    Animator animator;
    Rigidbody ZombieRigidbody;
    float speed;
    Vector3 previousLocation;
    [SerializeField] float staminaReward = 0.5f;
    [SerializeField] int creditReward;
    CreditsSystem creditsSystem;




    // Start is called before the first frame update
    public override void Start() 
    {
        navAgent = GetComponent<NavMeshAgent>();
        base.Start();
        animator = GetComponent<Animator>();
        ZombieRigidbody = GetComponent<Rigidbody>();
        previousLocation = transform.position;
        creditsSystem = FindObjectOfType<CreditsSystem>();
    }



    internal void Attack()
    {
        animator.SetLayerWeight(1, 1);
    }
    public virtual void AttackPoint()
    {

    }
    public void AttackFinished()
    {
        animator.SetLayerWeight(1, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float MoveDelta = Vector3.Distance(transform.position, previousLocation);
        speed = MoveDelta / Time.deltaTime;
        previousLocation = transform.position; 
        animator.SetFloat("Speed", speed);
    }




    public override void NoHealthLeft(GameObject killer)
    {
        base.NoHealthLeft();
        AIController AIC =  GetComponent<AIController>();
        if(AIC != null)
        {
            AIC.StopAIBehavior();
        }

        if(killer != null)
        {
           
            AbilityComponent abilityComp = killer.GetComponent<AbilityComponent>();
            if (abilityComp)
            {
                creditsSystem.ChangeCredits(creditReward);
                abilityComp.ChangeStamina(staminaReward);
            }
        }
    }

    public ZombieSaveData GenerateData()
    {
        

        return new ZombieSaveData(transform.position, GetComponent<HealthComponent>().GetHealth());
    }

    public void UpdateSaveData(ZombieSaveData zData)
    {   

        transform.position = zData.ZombieLocation;
        HealthComponent healthComp = GetComponent<HealthComponent>();
        healthComp.ChangeHealth(zData.ZombieHealth - healthComp.GetHealth());


  
    }

    public void DestroyZombie()
    {

         Destroy(gameObject);

    }

}
[Serializable]

public struct ZombieSaveData
{
    public ZombieSaveData(Vector3 zombieLoc, float zombieHealth)
    {
        ZombieLocation = zombieLoc;
        ZombieHealth = zombieHealth;
    }

    public Vector3 ZombieLocation;
    public float ZombieHealth;






}

