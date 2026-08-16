<div align="center">
  <h1>🕵️‍♂️ Taskkill - 3D Stealth Action Shooter</h1>
  <p><strong>CCTV와 적의 감시를 피해 은밀하게 엘리베이터에 도달하는 3D 잠입 액션 게임</strong></p>

  <!-- 방패 뱃지들 -->
  <img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white" alt="Unity">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
  <br><br>
</div>

## 📌 Project Overview
- **개발 기간:** 2019.12 ~ 2020.02 (3개월)
- **개발 인원:** 2인 팀 프로젝트 (프로그래밍 1명, 그래픽 1명)
- **담당 역할:** 메인 프로그래머 (클라이언트 로직 전체 구현 및 일부 그래픽 작업 참여)
- **엔진 버전:** Unity 2019.4.0f1
- **장르:** 3D 잠입 액션, 슈팅

## 🎮 Game Concept
제한된 작은 맵 안에서 **CCTV와 적의 시야에 들키지 않고 다음 구역으로 가는 엘리베이터에 도달**하는 것이 목표인 3D 잠입 액션 게임입니다.
무작정 돌진하는 것이 아니라, 드론과 다양한 무기(총, 칼)를 활용하여 전략적으로 은밀하게 적을 암살하고 퍼즐(해킹)을 풀어야 합니다.

### 💡 주요 플레이 요소
1. **은밀한 암살 (Assassination):** 총과 칼을 상황에 맞게 사용하여 소음이나 흔적 없이 적을 제거할 수 있습니다.
2. **드론 해킹 시스템 (Drone Hacking):** 드론을 조종하여 잠긴 문의 보안을 해킹해 열거나, 벽 너머의 적 위치를 정찰할 수 있습니다.
3. **은폐와 회피 (Stealth & Evasion):** 감시 카메라(CCTV)의 사각지대를 파악하고 적의 순찰 경로를 예측해 움직여야 합니다.

---

## 🛠 Tech Stack & Implementation

### **엔진 & 프로그래밍**
- Unity 3D (2019.4.0f1) / C#

### **핵심 기술 (Key Technologies)**

- **Shader Graph (셰이더 그래프)**
  - **Dissolve Effect (디졸브 이펙트):** 적이 사망했을 때 시체가 자연스럽게 타들어 가며 사라지는 연출을 노드 기반 셰이더로 구현했습니다.
  - **Hologram Effect (홀로그램 이펙트):** 드론 정찰 모드나 특정 UI 요소에 SF 느낌의 홀로그램 효과를 적용하기 위해 커스텀 셰이더를 제작했습니다.

- **Particle System (파티클 시스템)**
  - 감전(전기가 튀는 효과) 및 총구 화염(Muzzle Flash), 피격 효과 등 역동적인 슈팅 액션의 타격감을 살리기 위해 다양한 파티클 이펙트를 직접 제작 및 배치했습니다.

- **Unity NavMesh (AI 경로 탐색)**
  - 적 NPC들이 맵 내부의 장애물을 피해 자연스럽게 순찰(Patrol)하고, 플레이어 발견 시 추적(Chase)할 수 있도록 Unity의 내장 AI Navigation 시스템을 적용했습니다.

---

## 🔥 Review & Challenge
2인이라는 소규모 인원으로 3개월 내에 완성해야 하는 타이트한 일정이었습니다. 프로그래밍을 전담하면서 동시에 그래픽 리소스 작업 일부까지 참여하며 개발 파이프라인의 전반을 경험할 수 있었습니다. 특히, Shader Graph와 Particle System을 적극 활용하여 에셋에만 의존하지 않고 게임의 컨셉에 맞는 시각적 효과를 직접 구현한 부분이 가장 큰 성과입니다.
