using UnityEngine;

namespace ARFightingRobot
{
    public class AnimatorParentMove : MonoBehaviour
    {
        public Animator animator;
        public WarriorController warriorController; // Không khởi tạo tại đây

        void Awake()
        {
            warriorController = GetComponentInParent<WarriorController>(); // Khởi tạo trong Awake
        }

        void OnAnimatorMove()
        {
            if (warriorController.useRootMotion) {
                transform.parent.rotation = animator.rootRotation;
                transform.parent.position += animator.deltaPosition;
            }
        }
    }

}
