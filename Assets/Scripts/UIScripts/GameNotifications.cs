using UnityEngine;

namespace UIScripts
{
    public class GameNotifications : MonoBehaviour
    {
        [SerializeField] private GameObject lostOrWonTextParent;
        [SerializeField] private GameObject lostText;
        [SerializeField] private GameObject wontText;
        public void PlayerWon()
        {
            lostOrWonTextParent.SetActive(true);
            wontText.SetActive(true);
        }

        public void PlayerLost()
        {
            lostOrWonTextParent.SetActive(true);
            lostText.SetActive(true);
        }
        
    }
}
