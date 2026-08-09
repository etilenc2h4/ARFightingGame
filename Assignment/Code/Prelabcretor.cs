using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARFightingRobot
{
    public class PrelabCreator : MonoBehaviour
    {
        public GameObject RobotPrelab;
        public Vector3 prelaboffset;
        private GameObject Robot;
        private ARTrackedImageManager aRTrackedImageManager;
        
        // Danh sách lưu trữ các hình ảnh đã được xử lý
        private HashSet<string> processedImages = new HashSet<string>();

        private void OnEnable()
        {
            aRTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();
            aRTrackedImageManager.trackedImagesChanged += OnImageChanged;
        }

        private void OnDisable()
        {
            aRTrackedImageManager.trackedImagesChanged -= OnImageChanged;
        }

        private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
        {
            foreach (ARTrackedImage image in obj.added)
            {
                Robot = Instantiate(RobotPrelab, image.transform);
                Robot.transform.position += prelaboffset;

                // Gán thành phần Rigidbody
                Rigidbody rb = Robot.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                // Gán Animator nếu cần
                Animator animator = Robot.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.applyRootMotion = false; // Nếu bạn sử dụng script để di chuyển
                }
            }
        }
    }
}