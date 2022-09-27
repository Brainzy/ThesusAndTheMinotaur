using System;
using MinotaurController.OnPrefabs;
using StaticsAndEnums;
using UIScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MinotaurMovementController minotaurMovementController;
        [SerializeField] private GameNotifications gameNotifications;
        
        private Enums.GameState gameState; 
        public static GameManager inst;
        private int currentTurnCount;

        public int ReturnCurrentTurnCount()
        {
            return currentTurnCount;
        }

        public Enums.GameState ReturnGameState()
        {
            return gameState;
        }
        
        private void Awake()
        {
            inst = this;
        }

        private void Start()
        {
            UpdateGameState(Enums.GameState.TheseusTurn);
        }

        private void UpdateGameState(Enums.GameState newGameState)
        {
            gameState = newGameState;
        }

        public void PlayerFinishedTurn()
        {
            UpdateGameState(Enums.GameState.MinotaurTurn);
            currentTurnCount++;
            minotaurMovementController.MinotaurTurnFindTargetSquareToMove();
        }

        public void MinotaurFinishedTurn()
        {
            UpdateGameState(Enums.GameState.TheseusTurn);
        }

        public void PlayerTouchedMinotaur()
        {
            UpdateGameState(Enums.GameState.GameOver);
            gameNotifications.PlayerLost();
        }

        public void PlayerReachedExit()
        {
            UpdateGameState(Enums.GameState.GameOver);
            gameNotifications.PlayerWon();
        }

        public void MoveWasUndoed()
        {
            currentTurnCount--;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void PlayerSkippedTurn()
        {
            if (ReturnGameState() != Enums.GameState.TheseusTurn)
                return;

            PlayerFinishedTurn();
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadPreviousLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        
        
    }
}
