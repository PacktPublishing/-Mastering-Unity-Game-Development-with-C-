﻿using UnityEngine;
using System.Collections;


namespace Chapter4
{
    public class EnemyWavesChallenge : BaseChallenge
    {
        public int totalWaves = 5;  // Adjust as needed 
        private int currentWave = 0;
        public override void StartChallenge()
        {
            if (!commonData.isCompleted)
            {
                StartCoroutine(StartEnemyWavesChallenge());
            }
            else
            {
                Debug.Log("Challenge already completed!");
            }
        }

        IEnumerator StartEnemyWavesChallenge()
        {
            while (currentWave < totalWaves)
            {
                yield return StartCoroutine(SpawnEnemyWave());
               currentWave++;
            }
            CompleteChallenge();
        }
        public override void CompleteChallenge()
        {
            if (!commonData.isCompleted)
            {
                RewardManager.Instance.GrantReward(commonData);
                commonData.isCompleted = true;
            }
            else
            {
                Debug.Log("Challenge already completed!");
            }
        }
        IEnumerator SpawnEnemyWave()
        {
            // Adjust spawn positions, enemy types, and other parameters based on your game 
            Debug.Log($"Spawning Wave {currentWave + 1}");
            yield return new WaitForSeconds(2f);

        }

    }

}