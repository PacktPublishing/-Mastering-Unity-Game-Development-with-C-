using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FusionFuryGame
{
    public class PlayerShoot : MonoBehaviour
    {
        public static UnityAction onFire = delegate { };
        [SerializeField] BaseWeapon currentWeapon;
        [SerializeField] private float fireDamage;
        [SerializeField] private float shootingInterval = 0.5f;  // Set the shooting interval in seconds
        private float timeSinceLastShot = 0f;
        private PlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            //PoolManager.Instance.AddNewPoolItem(currentWeapon.weaponData.projectileData.attachedProjectile.gameObject, 50);
        }

        private void Update()
        {
            timeSinceLastShot += Time.deltaTime;
        }

        private void OnEnable()
        {
            PlayerInput.onShoot += OnShootFire;
            PlayerInput.onReload += OnReloadWeapon;
        }

        private void OnDisable()
        {
            PlayerInput.onShoot -= OnShootFire;
            PlayerInput.onReload -= OnReloadWeapon;
        }



        public void OnShootFire()
        {
            // Check if enough time has passed since the last shot
            if (timeSinceLastShot >= shootingInterval)
            {
                Vector3 aimDirection = playerMovement.GetMouseAimDirection();
                // Shoot in the forward vector of the weapon and pass player power stat
                currentWeapon.Shoot(fireDamage, aimDirection, true);

                // Reset the timer
                timeSinceLastShot = 0f;

                // Invoke the onFire event
                onFire.Invoke();
            }
        }

        private void OnReloadWeapon()
        {
            currentWeapon.ReloadAction();
        }


        public void ShootForAbility()
        {
            // Special shooting logic for abilities
            currentWeapon.ShootAbility(fireDamage * 2f, transform.forward * 3f);
        }
    }
}
