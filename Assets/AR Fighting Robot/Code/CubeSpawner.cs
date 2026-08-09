using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARFightingRobot
{
    public class CubeSpawner : MonoBehaviour
    {
        public GameObject cubePrefab; // Cube prefab mà bạn muốn tạo
        public Transform target; // Mục tiêu mà Cube sẽ tìm đến
        private Playerinf player;
        public float spawnInterval = 0.5f; // Khoảng thời gian giữa các lần tạo Cube
        public float moveSpeed = 1f; // Tốc độ di chuyển của Cube
        public float spawnRange = 5f; // Phạm vi tạo Cube ngẫu nhiên quanh GameObject
        public GameObject currentCube; // Biến lưu trữ Cube hiện tại

        private void Awake()
        {
            player = GetComponent<Playerinf>();
        }

        private void Start()
        {
            Transform weapon = target.Find("Mesh-Hammer-Weapon");
            // Bắt đầu việc tạo Cube mỗi khoảng thời gian, nhưng chỉ khi không có Cube nào đang tồn tại
            InvokeRepeating("TrySpawnCube", 0f, spawnInterval);
        }

        void TrySpawnCube()
        {
            // Nếu không có Cube nào tồn tại, mới tạo Cube mới
            if (currentCube == null)
            {
                SpawnCube();
            }
        }

        void SpawnCube()
        {
            // Tạo một vị trí ngẫu nhiên xung quanh GameObject (target)
            Vector3 randomPosition = new Vector3(
                Random.Range(target.position.x - spawnRange, target.position.x + spawnRange),
                target.position.y + 2f,  // Đảm bảo Cube có tọa độ Y giống với target (player)
                Random.Range(target.position.z - spawnRange, target.position.z + spawnRange)
            );

            // Tạo một Cube mới tại vị trí ngẫu nhiên
            currentCube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);

            // Điều chỉnh kích thước của Cube (sử dụng kích thước ban đầu thay vì Renderer)
            currentCube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);  // Kích thước ban đầu của Cube

            // Di chuyển Cube mới đến vị trí mục tiêu
            StartCoroutine(MoveCubeToTarget(currentCube));
        }

        IEnumerator MoveCubeToTarget(GameObject cube)
        {
            // Di chuyển Cube về phía mục tiêu cho đến khi đến gần
            while (cube != null && target != null)
            {
                cube.transform.position = Vector3.MoveTowards(cube.transform.position, target.position, moveSpeed / 600f);
                yield return null;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == currentCube)
            {
    

                player.CurrentHealth -= 0.1f; // Giảm máu khi Cube va chạm với Player

                Destroy(currentCube); // Hủy Cube khi va chạm
                currentCube = null;
            }
        }
    }

}
