using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace FusionFuryGame
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        public static UnityAction onPlayerDied = delegate { };
        public static UnityAction<float> onPlayerHealthChanged = delegate { };
        public float startingMaxHealth = 100;  // Set a default starting maximum health for the player
        private float currentHealth;

        public float healInterval = 2f;  // Time interval for healing
        public float healAmount = 5f;    // Amount of healing per interval

        private WaitForSeconds healIntervalWait;  // Reusable WaitForSeconds instance
        private Coroutine healOverTimeCoroutine;
        public float MaxHealth { get; set; }
       
        public float CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                if (currentHealth <= 0)
                {
                    onPlayerDied.Invoke();
                }
            }
        }
        void OnDestroy()
        {
            // Ensure to stop the healing coroutine when the object is destroyed
            if (healOverTimeCoroutine != null)
                StopCoroutine(healOverTimeCoroutine);
        }

        void Start()
        {
            SetMaxHealth();  // Set initial max health
            healIntervalWait = new WaitForSeconds(healInterval);
            StartHealingOverTime();
        }

        public void TakeDamage(float damage)
        {
            // Implement logic to handle taking damage
            CurrentHealth -= damage *0.2f;
            onPlayerHealthChanged.Invoke(CurrentHealth);
        }

        public void SetMaxHealth()
        {
            MaxHealth = startingMaxHealth;
            currentHealth = MaxHealth;
            // Implement logic to calculate and set the actual MaxHealth based on game progress and levels
        }

        public void Heal()
        {
            CurrentHealth += healAmount * 0.4f;
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            onPlayerHealthChanged.Invoke(CurrentHealth);
        }

        private void StartHealingOverTime()
        {
            healOverTimeCoroutine = StartCoroutine(HealOverTime());
        }

        private IEnumerator HealOverTime()
        {
            while (true)
            {
                yield return healIntervalWait;
                Heal();
            }
        }
    }
}