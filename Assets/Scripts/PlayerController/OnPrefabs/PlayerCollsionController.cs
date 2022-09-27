using UnityEngine;

namespace PlayerController.OnPrefabs
{
    public class PlayerCollsionController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Minotaur"))
            {
                GameManager.GameManager.inst.PlayerTouchedMinotaur();
            }

            if (other.CompareTag("ExitMarker"))
            {
                GameManager.GameManager.inst.PlayerReachedExit();
            }
        }
    }
}
