using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARFightingRobot
{
    public class WeaponAttack : MonoBehaviour
    {
        private Playerinf player;

        void Start()
        {
            player = FindObjectOfType<Playerinf>();
        }

        void OnCollisionEnter(Collision collision)
        {
            // Kiểm tra xem đối tượng va chạm có phải là Cube (clone) không
            if (collision.gameObject.CompareTag("Cube"))
            {
                // Hủy Cube khi va chạm
                player.Score++;
                Destroy(collision.gameObject); 
            }
        }
    }
}
