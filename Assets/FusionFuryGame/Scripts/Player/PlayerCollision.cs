using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FusionFuryGame
{
    public class PlayerCollision : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        private IDamage enemyDamage;

        public static UnityAction onPlayerGetHit = delegate { };
        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyProjectile"))
            {
                if (collision.gameObject.TryGetComponent(out enemyDamage))
                {
                    playerHealth.TakeDamage(enemyDamage.GetDamageValue());
                    onPlayerGetHit.Invoke();
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BossArea"))
            {
                CameraManager.Instance.SwitchCamera(CameraType.BossCamera);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("BossArea"))
            {
                CameraManager.Instance.SwitchCamera(CameraType.PlayerCamera);
            }
        }


        private void OnCollisionExit(Collision collision)
        {
            //Start To heal the player 
        }
    }
}