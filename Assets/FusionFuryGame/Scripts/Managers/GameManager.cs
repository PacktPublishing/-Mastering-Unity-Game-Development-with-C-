using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FusionFuryGame
{
    public class GameManager : MonoBehaviour
    {
        public PlayerData playerData;
        public GameSettings gameSettings;
        public AudioClip gameMusic;
        #region Unity CallBacks

        private void OnEnable()
        {
            PlayerHealth.onPlayerDied += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            PlayerHealth.onPlayerDied -= HandlePlayerDeath;
        }
        private void Start()
        {
            AudioManager.Instance.PlayMusic(gameMusic, 0.3f);
            LoadGameData();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(0);
            }
        }
        private void OnApplicationQuit()
        {
            SaveGameData();
        } 
        #endregion


        private void LoadGameData()
        {
            
            if (playerData == null)
            {
               
                Debug.Log("No player data found, creating new instance.");
            }
            else
            {
                JsonUtility.FromJsonOverwrite(SaveManager.LoadData("playerData"), playerData);
                Debug.Log("Player data loaded successfully.");
            }

            
            if (gameSettings == null)
            {
                gameSettings = ScriptableObject.CreateInstance<GameSettings>();
                Debug.Log("No game settings found, creating new instance.");
            }
            else
            {
                JsonUtility.FromJsonOverwrite(SaveManager.LoadData("gameSettings"), gameSettings);
                Debug.Log("Game settings loaded successfully.");
            }
        }

        private void SaveGameData()
        {
            SaveManager.SaveData("playerData", JsonUtility.ToJson(playerData));
            SaveManager.SaveData("gameSettings", JsonUtility.ToJson(gameSettings));
            Debug.Log("Game data saved successfully.");
        }

        private void HandlePlayerDeath()
        {
            StartCoroutine(StopTheGame());
        }

        IEnumerator StopTheGame()
        {
            yield return new WaitForSecondsRealtime(0.15f);
            Time.timeScale = 0;
        }
    }
}