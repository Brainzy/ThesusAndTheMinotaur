using System.Collections.Generic;
using BoardScripts.OnPrefabs;
using MinotaurController.OnPrefabs;
using PlayerController.OnPrefabs;
using StaticsAndEnums;
using UnityEngine;

namespace GameManager
{
    public class UndoManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private MinotaurMovementController minotaurMovementController;
        
        public static UndoManager inst;

        private readonly Dictionary<int, Square> playerSquareHistory = new Dictionary<int, Square>();
        private readonly Dictionary<int, Square> minotaurSquareHistory = new Dictionary<int, Square>();

        private void Awake()
        {
            inst = this;
        }

        public void PlayerFinishedMovingToASquare(Square square)
        {
          playerSquareHistory.Add(GameManager.inst.ReturnCurrentTurnCount(), square);
        }

        public void MinotaurFinishedMovingToASquare(Square square)
        {
            if (minotaurSquareHistory.ContainsKey(GameManager.inst.ReturnCurrentTurnCount()))
            {
                return;
            }

            minotaurSquareHistory.Add(GameManager.inst.ReturnCurrentTurnCount(), square);
        }

        public void UndoMove()
        {
            if (GameManager.inst.ReturnGameState() == Enums.GameState.TheseusTurn &&
                playerMovementController.IsPlayerMoving() == false)
            {
                ExecuteUndoMovement();
            }
        }

        private void ExecuteUndoMovement()
        {
            if (GameManager.inst.ReturnCurrentTurnCount() == 0)
                return;
            
            if (playerSquareHistory.ContainsKey(GameManager.inst.ReturnCurrentTurnCount()-1))
            {
                playerMovementController.UndoMovementToSquare(playerSquareHistory[GameManager.inst.ReturnCurrentTurnCount()-1]);
                playerSquareHistory.Remove(GameManager.inst.ReturnCurrentTurnCount() - 1);
            }
            
            if (minotaurSquareHistory.ContainsKey(GameManager.inst.ReturnCurrentTurnCount()))
            {
                minotaurMovementController.UndoMovementToSquare(minotaurSquareHistory[GameManager.inst.ReturnCurrentTurnCount()]);
                minotaurSquareHistory.Remove(GameManager.inst.ReturnCurrentTurnCount());
            }
            
            GameManager.inst.MoveWasUndoed();
          
        }

    }
}
