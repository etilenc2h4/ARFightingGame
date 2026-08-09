using TMPro;
using UnityEngine;

namespace ARFightingRobot
{
    public class GuiScoreControl : MonoBehaviour
    {
        private Playerinf player;
        public TMP_Text scoreText; // Biến lưu trữ TMP_Text
        void Start()
        {

        }
        private void Update()
        {
            player = FindObjectOfType<Playerinf>();
            if (scoreText != null && player != null)
            {
                scoreText.text = "Điểm: " + player.Score; // Hiển thị điểm trên màn hình
            }
            else 
            {
                scoreText.text = "";
            }
        }
    }
}