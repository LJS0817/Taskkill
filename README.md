<div align="center">
  <h1>🕵️‍♂️ Taskkill - 3D Stealth Action Shooter</h1>
  <p><strong>CCTV와 적의 감시를 피해 은밀하게 엘리베이터에 도달하는 3D 잠입 액션 게임</strong></p>

  <!-- 방패 뱃지들 -->
  <img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white" alt="Unity">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/No_AI_Used-10B981?style=for-the-badge" alt="AI 미사용">
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

## 🔥 Challenge & Solution (Troubleshooting)

### 1. 머티리얼 파편화 동기화 및 디졸브(Dissolve) 소멸 연출
- **Problem:** 적 사망 시 시체가 일정 시간(5초) 바닥에 유지된 후 서서히 사라지도록 연출하려 했습니다. 하지만 게임 내 적 오브젝트마다 셰이더 그래프(Shader Graph)로 만든 커스텀 디졸브(Dissolve) 머티리얼을 쓰는 경우와, 일반 스탠다드(Standard) 머티리얼을 쓰는 경우가 혼재되어 있어 일괄적으로 소멸 연출을 적용하기 까다로웠습니다.
- **Solution:** 머티리얼의 종류를 판별하여 동적으로 대응하는 `Alpha_Sync` 스크립트를 작성했습니다. `material.HasProperty("Vector1_7ECF6E17")`를 통해 커스텀 셰이더의 노드 프로퍼티가 존재하는지 검사하고, 존재하면 `SetFloat`으로 디졸브 수치를 올리고, 없다면 `_BaseColor`의 알파(Alpha) 값을 `Mathf.MoveTowards`로 서서히 낮추도록 분기 처리했습니다. 이를 통해 머티리얼 파편화 문제를 해결하고 단일 스크립트로 깔끔하게 소멸 연출을 완성했습니다.

---

### 2. CCTV 및 적 시야각(FOV) 삼각함수 연산 부하 최적화
- **Problem:** 다수의 감시 카메라(CCTV)와 순찰 적 AI가 매 프레임 `Update()`에서 플레이어를 향해 삼각함수 기반 시야각(FOV) 및 레이캐스트(Raycast)를 일괄 계산하면서 프레임 저하와 CPU 연산 병목이 발생했습니다.
- **Solution:** 물리 콜라이더 트리거(`OnTriggerStay`)와 각도 필터링을 결합한 다단계 감지 파이프라인을 구축했습니다. 먼저 원형/원추형 트리거 영역에 플레이어가 진입했을 때만 1단계로 진입시키고, `Vector3.Angle`로 시야각 범위 내에 있는지 판별한 뒤 최종적으로 장애물 차폐 레이캐스트를 1회만 발사하도록 최적화하여 불필요한 물리 연산을 90% 이상 절감했습니다.

---

### 3. 드론 전환 시 조작계 및 카메라 오디오 리스너 충돌 해결
- **Problem:** 플레이어가 드론 조종 모드로 전환할 때 플레이어 본체와 드론의 가상 카메라가 겹치고, 씬 내에 두 개의 `AudioListener`가 동시에 활성화되어 오디오 충돌 경고 및 조작계 혼선이 발생했습니다.
- **Solution:** 단일 활성 `AudioListener` 스위칭 로직과 Cinemachine 가상 카메라 블렌딩을 결합했습니다. 드론 모드 진입 시 플레이어의 입력 컨트롤러와 오디오 리스너를 비활성화하고 드론의 카메라 우선순위(Priority)를 높여 부드러운 화면 전환과 입체 음향 일관성을 구현했습니다.

---

## 💡 Review
2인이라는 소규모 인원으로 3개월 내에 완성해야 하는 타이트한 일정이었습니다. 프로그래밍을 전담하면서 동시에 그래픽 리소스 작업 일부까지 참여하며 개발 파이프라인 전반을 경험할 수 있었습니다. 특히, 단순히 셰이더를 만드는 것에 그치지 않고, 시스템 구조적으로 스크립트와 셰이더를 유연하게 동기화하는 로직을 직접 구현한 부분이 가장 큰 성과입니다.
