using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    MovementComponent movementComp;
    InputActions inputActions;
    Animator animator;
    int speedHash = Animator.StringToHash("speed");
    Coroutine BackToIdleCoroutine;
    InGameUI inGameUI;

    [SerializeField] Weapon[] StartWeaponPrefabs;
    [SerializeField] Transform GunSocket;
    [SerializeField] JoyStickZ moveStick;
    [SerializeField] JoyStickZ aimStick;
    [SerializeField] List<Weapon> Weapons = new List<Weapon>();
    [SerializeField] AudioSource audioSource;
    public Weapon CurrentWeapon;
    CameraManager cameraManager;
    int currentWeaponIndex = 0;
    bool PlayerDead = false;


    AbilityComponent abilityComp;
    AbilityWheel abilityWheel;

    internal void AquireNewWeapon(Weapon weapon, bool Equip = false)
    {

        Weapon newWeapon = Instantiate(weapon, GunSocket);
        newWeapon.Owner = gameObject;
        newWeapon.UnEquip();
        Weapons.Add(newWeapon);
        if (Equip)
        {
            EquipWeapon(Weapons.Count - 1);
        }
    }

    private void Awake()
    {
        inputActions = new InputActions();
        abilityComp = GetComponent<AbilityComponent>();
        abilityWheel = FindObjectOfType<AbilityWheel>();
        if (abilityComp != null)
        {
            abilityComp.onNewAbilityInit += NewAbilityAdded;
            abilityComp.onStamianUpdated += StaminaUpdated;

        }
    }

    private void StaminaUpdated(float newValue)
    {
        abilityWheel.UpdatedStamina(newValue);
    }


    private void NewAbilityAdded(AbilityBase newAbility)
    {
        AbilityWheel abilityWheel = FindObjectOfType<AbilityWheel>();
        abilityWheel.AddNewAbility(newAbility);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void InitializeWeapons()
    {
        foreach (Weapon weapon in StartWeaponPrefabs)
        {
            AquireNewWeapon(weapon);
        }
        EquipWeapon(0);
    }

    void EquipWeapon(int weaponIndex)
    {
        if (Weapons.Count > weaponIndex)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.UnEquip();
            }

            currentWeaponIndex = weaponIndex;
            Weapons[weaponIndex].Equip();
            CurrentWeapon = Weapons[weaponIndex];
            animator.SetFloat("FiringSpeed", CurrentWeapon.shootingSpeed);
            if (inGameUI != null)
            {
                inGameUI.SwichedWeaponTo(CurrentWeapon);
            }

        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        inGameUI = FindObjectOfType<InGameUI>();
        movementComp = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        inputActions.Gameplay.CursorPos.performed += CursorPosUpdated;
        inputActions.Gameplay.move.performed += MoveInputUpdated;
        inputActions.Gameplay.move.canceled += MoveInputUpdated;
        inputActions.Gameplay.MainAction.performed += MainActionButtonDown;
        inputActions.Gameplay.MainAction.canceled += MainActionReleased;
        inputActions.Gameplay.Space.performed += BigAction;
        inputActions.Gameplay.NextWeapon.performed += NextWeapon;
        animator.SetTrigger("BackToIdle");
        InitializeWeapons();
        cameraManager = FindObjectOfType<CameraManager>();

        abilityWheel.UpdatedStamina(abilityComp.GetStaminaLevel());
    }

    public void NextWeapon(InputAction.CallbackContext obj)
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
        EquipWeapon(currentWeaponIndex);
    }

    private void BigAction(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("BigAction");
    }

    private void MainActionReleased(InputAction.CallbackContext obj)
    {
        StopFire();
    }

    private void MainActionButtonDown(InputAction.CallbackContext obj)
    {
        Fire();
    }

    private void CursorPosUpdated(InputAction.CallbackContext obj)
    {
        movementComp.SetCursorPos(obj.ReadValue<Vector2>());
    }

    private void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>().normalized;
        UpdateMovement(input);
    }

    private void UpdateMovement(Vector2 input)
    {
        movementComp.SetMovementInput(input);
        if (input.magnitude == 0)
        {
            BackToIdleCoroutine = StartCoroutine(DelayedBackToIdle());
        }
        else
        {
            if (BackToIdleCoroutine != null)
            {
                StopCoroutine(BackToIdleCoroutine);
                BackToIdleCoroutine = null;
            }
        }
    }

    IEnumerator DelayedBackToIdle()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("BackToIdle");
    }

    void UpdateAnimation()
    {
        animator.SetFloat(speedHash, GetComponent<CharacterController>().velocity.magnitude);
        Vector3 PlayerForward = movementComp.GetPlayerDesiredLookDir();
        Vector3 PlayerMoveDir = movementComp.GetPlayerDesiredMoveDir();
        Vector3 PlayerLeft = Vector3.Cross(PlayerForward, Vector3.up);
        float forwardAmt = Vector3.Dot(PlayerForward, PlayerMoveDir);
        float leftAmt = Vector3.Dot(PlayerLeft, PlayerMoveDir);
        animator.SetFloat("forwardSpeed", forwardAmt);
        animator.SetFloat("leftSpeed", leftAmt);
    }

    // Update is called once per frame
    new void Update()
    {

        base.Update();

        if (!PlayerDead)
        {
            UpdateAnimation();
            UpdateMoveStickInput();

            UpdateAimStickInput();
        }

        UpdateCamera();

    }

    private void UpdateAimStickInput()
    {
        movementComp.SetAimInput(aimStick.Input);
        if (aimStick.Input.magnitude > 0)
        {
            Fire();
        }
        else
        {
            StopFire();
        }
    }

    private void Fire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
    }

    private void StopFire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
    }

    private void UpdateCamera()
    {
        cameraManager.UpdateCamera(transform.position, moveStick.Input, aimStick.Input.magnitude > 0);
    }



    public void FireTimePoint()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.Fire();
            audioSource.clip = CurrentWeapon.FiringSound();
            audioSource.Play();


        }
        
    }

    public void UpdateMoveStickInput()
    {
        if (moveStick != null)
        {
            UpdateMovement(moveStick.Input);
        }
    }

    public override void NoHealthLeft(GameObject killer)
    {
        base.NoHealthLeft();
        PlayerDead = true;
    }

    public PlayerSaveData GenerateSaveData()
    {
        List<string> weaponNames = new List<string>();
        foreach (Weapon weapon in Weapons)
        {
            weaponNames.Add(weapon.GetWeaponInfo().name);
        }


        return new PlayerSaveData(transform.position,
            GetComponent<HealthComponent>().GetHealth(),
            GetComponent<AbilityComponent>().GetStaminaLevel(),
            weaponNames.ToArray(),
            FindObjectOfType<CreditsSystem>().GetCredits()
            );

    }

    public void UpdateFromSaveData(PlayerSaveData data)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = data.PlayerLocation;
        GetComponent<CharacterController>().enabled = true;

        HealthComponent healthComp = GetComponent<HealthComponent>();
        healthComp.ChangeHealth(data.PlayerHealth - healthComp.GetHealth());

        abilityComp.ChangeStamina(data.PlayerStamina - abilityComp.GetStaminaLevel());

        CreditsSystem credSystem = FindObjectOfType<CreditsSystem>();
        credSystem.ChangeCredits(data.PlayerCredits - credSystem.GetCredits());

        var shops = Resources.FindObjectsOfTypeAll<ShopSystem>();
        if (shops.Length > 0)
        {
            ShopSystem shop = shops[0];
            Weapon[] allWeapons = shop.GetWeaponsOnSale();
            List<string> weaponInData = new List<string>(data.Weapons);
            foreach (Weapon weapon in allWeapons)
            {
                bool hadWeapon = weaponInData.Contains(weapon.GetWeaponInfo().name);
                bool alreadyHave = false;
                foreach (Weapon weaponAlreadyHave in StartWeaponPrefabs)
                {
                    if (weaponAlreadyHave.GetWeaponInfo().name == weapon.GetWeaponInfo().name)
                    {
                        alreadyHave = true;
                    }
                }

                if (hadWeapon && !alreadyHave)
                {
                    AquireNewWeapon(weapon);
                }
            }
        }
    }
}


    [Serializable]

    public struct PlayerSaveData
    {
        public PlayerSaveData(Vector3 playerLoc, float playerHealth, float playerStamina, string[] weapons, int playerCredits)
        {
            PlayerLocation = playerLoc;
            PlayerHealth = playerHealth;
            PlayerStamina = playerStamina;
            Weapons = weapons;
            PlayerCredits = playerCredits;
        }

        public Vector3 PlayerLocation;
        public float PlayerHealth;
        public float PlayerStamina;
         public int PlayerCredits;
        public string[] Weapons;



    

    }
