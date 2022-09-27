using System.Collections.Generic;
using BoardScripts.OnPrefabs;
using GameManager;
using PlayerController.OnPrefabs;
using StaticsAndEnums;
using UnityEngine;

namespace MinotaurController.OnPrefabs
{
    public class MinotaurMovementController : MonoBehaviour
    {
        [SerializeField] private float minotaurSpeed = 2;
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private Square currentSquare;
        [SerializeField] private int minotaurMoveAmount = 2;

        private readonly List<Square> minotaurMovementQueue = new List<Square>();
        private bool minotaurMoving;
        private Square targetSquare;
        private Vector3 targetPosition;
        private float timeNeededToReachPosition;

        public void UndoMovementToSquare(Square square)
        {  
            currentSquare = square;
            targetSquare = null;
            transform.position = square.transform.position;
            minotaurMovementQueue.Clear();
            minotaurMoving = false;
        }
        
        public void MinotaurTurnFindTargetSquareToMove()
        {
            var checkingSquare = currentSquare;
            for (int i = 0; i < 2; i++)
            {
                if (AddToMinotaurMovementQueue(checkingSquare.horizontalWalkableSquares))
                {
                    checkingSquare = minotaurMovementQueue[^1]; // in this case we found a closer square and we try for another square
                    continue;
                }

                if (AddToMinotaurMovementQueue(checkingSquare.verticallWalkableSquares))
                    checkingSquare = minotaurMovementQueue[^1];
            }
          
            if (minotaurMovementQueue.Count == 0)
            {
                GameManager.GameManager.inst.MinotaurFinishedTurn();
            }
            else
            {
                minotaurMoving = true;
                AssignNewSquareToTravelTo(minotaurMovementQueue[0]);
            }
        }
        
        private void AssignNewSquareToTravelTo(Square desiredSquare)
        {
            targetSquare = desiredSquare;
            targetPosition = targetSquare.transform.position;
            timeNeededToReachPosition = 1 / minotaurSpeed;
            minotaurMovementQueue.RemoveAt(0);
        }

        private bool AddToMinotaurMovementQueue(List<Square> squares)
        {
            for (int i = 0; i < squares.Count; i++)
            {
                if (minotaurMoveAmount == minotaurMovementQueue.Count)
                    break;

                var square = squares[i];
                
                if (DoesMovingGetMinotaurCloserToPlayer(square.transform.position))
                {
                    minotaurMovementQueue.Add(square);
                    return true;
                }
            }

            return false;
        }

        private bool DoesMovingGetMinotaurCloserToPlayer(Vector3 newPotentialPos)
        {
            var currentPosition = minotaurMovementQueue.Count == 0 ? transform.position : minotaurMovementQueue[0].transform.position; // this takes into account if already 1 field is scheduled

            var playerPos = playerMovementController.transform.position;
            
            var currentDistance =  Vector3.Distance( currentPosition,playerPos);
            var distanceFromNewPos = Vector3.Distance(newPotentialPos, playerPos);
            
            return distanceFromNewPos < currentDistance;
        }
        
        private void Update()
        {
            if (minotaurMoving == false)
                return;

            if (GameManager.GameManager.inst.ReturnGameState() != Enums.GameState.MinotaurTurn)
                return;
            
            timeNeededToReachPosition -= Time.deltaTime;

            if (timeNeededToReachPosition <= 0)
            {
                UndoManager.inst.MinotaurFinishedMovingToASquare(currentSquare);
                currentSquare = targetSquare;
                
                if (minotaurMovementQueue.Count==0)
                    GameManager.GameManager.inst.MinotaurFinishedTurn();
                else
                {
                    AssignNewSquareToTravelTo(minotaurMovementQueue[0]);
                }
                return;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, minotaurSpeed * Time.deltaTime);
        }
    }
}
