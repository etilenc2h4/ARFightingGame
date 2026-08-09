using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARFightingRobot
{
    public class GUIControls : MonoBehaviour
    {
        public Texture2D swordIcon; // Biểu tượng kiếm
        private WarriorController warriorController;
		private Playerinf player;
		
        private Canvas uiCanvas; // Canvas để chứa slider
        private Slider healthSlider; // Thành phần slider máu

        private void Awake()
        {
            warriorController = GetComponent<WarriorController>();
			player = GetComponent<Playerinf>();

            // Tạo một Canvas nếu chưa có
            GameObject canvasObj = new GameObject("UICanvas");
            uiCanvas = canvasObj.AddComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Tạo slider
            CreateHealthSlider();
        }



        private void CreateHealthSlider()
        {
            // Tạo một GameObject mới cho slider
            GameObject sliderObj = new GameObject("HealthSlider");
            sliderObj.transform.SetParent(uiCanvas.transform);

            // Thêm component Slider
            healthSlider = sliderObj.AddComponent<Slider>();

            // Cấu hình RectTransform của Slider
            RectTransform rectTransform = sliderObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(800, 70); // Kích thước
            rectTransform.anchoredPosition = new Vector2(-100, 900); // Vị trí trên màn hình
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Tạo các thành phần cho Slider (Background, Fill Area)
            CreateSliderUIElements(sliderObj);

            // Đặt giá trị ban đầu
            healthSlider.minValue = 0;
            healthSlider.maxValue = player.MaxHealth;
            healthSlider.value = player.CurrentHealth;
        }

        private void CreateSliderUIElements(GameObject sliderObj)
        {
            // Tạo Background
            GameObject background = new GameObject("Background");
            background.transform.SetParent(sliderObj.transform);
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = Color.gray;

            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Tạo Fill Area
            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObj.transform);
            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0f, 0f);
            fillAreaRect.anchorMax = new Vector2(1f, 1f);
            fillAreaRect.offsetMin = new Vector2(5, 5);
            fillAreaRect.offsetMax = new Vector2(-5, -5);

            // Tạo Fill
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform);
            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = Color.green;

            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            // Liên kết Fill vào Slider
            healthSlider.fillRect = fillRect;
        }

        private void OnGUI()
        {
            Attacking();

            // Cập nhật giá trị slider máu
            if (healthSlider != null && warriorController != null)
            {
                healthSlider.value = player.CurrentHealth;
            }
        }

        private void Attacking()
        {
            if (GUI.Button(new Rect(725, 1750, 300, 300), swordIcon))
            {
                warriorController.Attacking();
            }
        }
    }
}
