using System;
using BoardScripts;
using BoardScripts.OnPrefabs;
using GameManager;
using StaticsAndEnums;
using UnityEngine;

namespace PlayerController.OnPrefabs
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Square currentSquare;

        private bool isMoving;
        private Square targetSquare;
        private Vector3 targetPosition;
        private float timeNeededToReachPosition;
        private DateTime lastMoveInput;
        private Square queuedSquare;

        public void UndoMovementToSquare(Square square)
        {
            currentSquare = square;
            targetSquare = null;
            queuedSquare = null;
            isMoving = false;
            transform.position = square.transform.position;
        }

        public bool IsPlayerMoving()
        {
            return isMoving;
        }

        public void MoveCommandReceived(Vector3 moveDirection)
        {
            if (GameManager.GameManager.inst.ReturnGameState() != Enums.GameState.TheseusTurn)
                return;

            if (isMoving && (DateTime.Now-lastMoveInput).TotalSeconds < 0.2f) // this will allow continues movement
                return;
            
            var desiredSquare = SquareManager.inst.ReturnSquareOnPosition(currentSquare.transform.position + moveDirection);
            
            if (desiredSquare == null)
                return;

            if (targetSquare == desiredSquare && isMoving)
            {
                QueueASquareForFutureMovement(moveDirection);
                return;
            }

            if (currentSquare.adjacentWalkableSquares.Contains(desiredSquare) && isMoving == false)
            {
                AssignNewSquareToTravelTo(desiredSquare);
            }
        }

        private void QueueASquareForFutureMovement(Vector3 moveDirection)
        {
            var desiredQueueSquare = SquareManager.inst.ReturnSquareOnPosition(targetSquare.transform.position + moveDirection);

            if (targetSquare.adjacentWalkableSquares.Contains(desiredQueueSquare))
            {
                queuedSquare = desiredQueueSquare;
            }
        }

        private void AssignNewSquareToTravelTo(Square desiredSquare)
        {
            targetSquare = desiredSquare;
            targetPosition = targetSquare.transform.position;
            timeNeededToReachPosition = 1 / speed;
            isMoving = true;
            lastMoveInput = DateTime.Now;
        }

        private void Update()
        {
            if (!isMoving) return;

            if (GameManager.GameManager.inst.ReturnGameState() != Enums.GameState.TheseusTurn)
                return;
            
            timeNeededToReachPosition -= Time.deltaTime;

            if (timeNeededToReachPosition <= 0)
            {
                UndoManager.inst.PlayerFinishedMovingToASquare(currentSquare);
                currentSquare = targetSquare;
                GameManager.GameManager.inst.PlayerFinishedTurn();

                if (queuedSquare == null)
                    StopMovement();
                else
                {
                    AssignNewSquareToTravelTo(queuedSquare);
                    queuedSquare = null;
                }

                return;
            }
                
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        private void StopMovement()
        {
            isMoving = false;
            targetSquare = null;
        }
    }
}
