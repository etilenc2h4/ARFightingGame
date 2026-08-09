# AR Fighting Game

Đây là dự án game đối kháng tích hợp công nghệ Thực tế Tăng cường (Augmented Reality - AR) được phát triển trên nền tảng Unity.

## 🎮 Tổng quan (Overview)
AR Fighting Game mang đến trải nghiệm chiến đấu độc đáo khi đưa các nhân vật ảo ra ngoài đời thực thông qua camera của thiết bị. Người chơi có thể điều khiển nhân vật, sử dụng các kỹ năng và chiến đấu trong môi trường không gian thực.

## ✨ Tính năng nổi bật (Features)
- **Tích hợp AR**: Sử dụng Unity XR/AR Foundation để đưa môi trường game ra ngoài đời thực.
- **Hệ thống Combat cơ bản**: Các bộ điều khiển chiến đấu, vũ khí và animation được thiết lập sẵn (Warrior Controller, Weapon Attack).
- **Inverse Kinematics (IK)**: Áp dụng IK cho chuyển động tay của nhân vật (IKHands).
- **Giao diện & Điểm số**: Hệ thống tính điểm và điều khiển UI (GUIControls, GuiScoreControl).

## 📂 Cấu trúc thư mục chính (Project Structure)
Dự án được xây dựng trên cấu trúc Unity tiêu chuẩn. Một số thành phần nổi bật bao gồm:

* `Assets/`: Chứa toàn bộ tài nguyên (models, textures, animations, prefabs, materials).
* `Assignment/`: Chứa các tài liệu báo cáo của dự án, video demo và slide thuyết trình.
  * `Code/`: Source code chính (WarriorController, IKHands, AnimatorParentMove,...).
  * `Report.docx`: Báo cáo chi tiết dự án.
  * `Slide.pptx`: Slide thuyết trình.
  * `Video.mp4`: Video demo gameplay.
* `ProjectSettings/`: Các thiết lập cấu hình của dự án Unity.

## 🚀 Hướng dẫn cài đặt và chạy dự án (How to run)
1. **Yêu cầu hệ thống**: 
   - [Unity Hub](https://unity.com/download) và Unity Editor (tương thích với phiên bản sử dụng trong dự án).
   - SDK cho Android/iOS (tùy thuộc vào thiết bị build).
2. **Clone dự án**:
   ```bash
   git clone https://github.com/etilenc2h4/ARFightingGame.git
   ```
3. Mở **Unity Hub** -> Chọn **Add project from disk** -> Trỏ tới thư mục `ARFightingGame`.
4. Mở Scene chính của game trong thư mục `Assets/Scenes`.
5. Kết nối thiết bị hỗ trợ AR, hoặc build file `.apk` (với Android) để trải nghiệm trực tiếp trên điện thoại.

## 📝 Giấy phép (License)
Dự án được tạo ra nhằm mục đích học tập và báo cáo bài tập.