using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FusionFuryGame.RewardManager;

namespace FusionFuryGame
{
    public class ChallengeManager : Singlton<ChallengeManager>
    {
        // Define different types of challenges
        public enum ChallengeType
        {
            EnemyWaves,
            TimeTrials,
            LimitedResources,
            NoDamageRun,
            AccuracyChallenge
        }

        
        public GenericDictionary<ChallengeType, BaseChallenge> challengeDictionary = new GenericDictionary<ChallengeType, BaseChallenge>();


        public void StartChallenge(ChallengeType challengeType)
        {
            if (challengeDictionary.TryGetValue(challengeType, out BaseChallenge challengeScript))
            {
                JsonUtility.FromJsonOverwrite(SaveManager.LoadData(challengeScript.challengeSavedKey), challengeScript.commonData);
                if (!challengeScript.commonData.isCompleted)
                {
                    SetCurrentChallenge(challengeScript);
                    currentChallenge.StartChallenge();
                }
                else
                {
                    Debug.Log("Challenge already completed!");
                }
            }
            else
            {
                Debug.LogError($"No challenge script found for ChallengeType {challengeType}");
            }
        }
        

        private BaseChallenge currentChallenge;

        private void SetCurrentChallenge(BaseChallenge challengeScript)
        {
            if (currentChallenge != null)
            {
                currentChallenge.CompleteChallenge();
            }

            currentChallenge = challengeScript;
        }
    }

    
}