using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARFightingRobot
{
    public class GameControl : MonoBehaviour
    {
        // Start is called before the first frame update

        private Playerinf player;
        public GameObject gameOverUI;

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            player = FindObjectOfType<Playerinf>();
            if (player != null)
            {
                if (player.CurrentHealth <= 0)
                {
                    gameOverUI.SetActive(true);
                }
            }
        }

        public void RestartGame()
        {
            // Tải lại Scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}