using GameManager;
using PlayerController.OnPrefabs;
using UnityEngine;

namespace InputScripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController playerMovementController;
   
        private void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                playerMovementController.MoveCommandReceived(new Vector3(0,1,0));   
            }
            
            if (Input.GetKey(KeyCode.DownArrow))
            {
                playerMovementController.MoveCommandReceived(new Vector3(0,-1,0));   
            }
            
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerMovementController.MoveCommandReceived(new Vector3(-1,0,0));   
            }
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerMovementController.MoveCommandReceived(new Vector3(1,0,0));   
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                GameManager.GameManager.inst.PlayerSkippedTurn();
            }
            
            if (Input.GetKeyDown(KeyCode.U))
            {
                UndoManager.inst.UndoMove();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.GameManager.inst.RestartGame();
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                GameManager.GameManager.inst.LoadNextLevel();
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameManager.GameManager.inst.LoadPreviousLevel();
            }
        }
    }
}
