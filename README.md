# BC_Portal
<div align="center">
<!-- 프로젝트 대표 이미지 자리 -->
<img width="480" height="320" alt="스크린샷 2025-08-21 192844" src="https://github.com/user-attachments/assets/2658dd41-57f0-4a71-a37e-8afb9353e1e9" />
  
![Unity](https://img.shields.io/badge/Unity-2022.3.17f1-blue)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-PC-lightgrey)
</div>

<br>

## 📖 프로젝트 개요

**프로젝트명**: BC_Portal  
**개발 기간**: 2025.08.14 - 2025.08.22  
**주관**: 내일배움캠프 Unity 11기

<br>

## 🎮 게임 소개

**BC_Portal**은 Portal과 The Witness에서 영감을 받은 **3D 퍼즐 플랫폼 게임**입니다.

- **장르**: 3D 퍼즐 플랫폼
- **플레이 방식**: 1인칭 시점 퍼즐 해결
- **핵심 메커니즘**: 포털을 활용한 공간 이동과 물리 기반 퍼즐
- **아트 스타일**: 미래지향적 산업 시설

플레이어는 포털을 생성하여 공간을 이동하며, 다양한 장애물과 퍼즐을 해결해 목표 지점에 도달해야 합니다.

## 조작법
- 기본이동 WASD
- 점프 spacebar
- 상호작용 E
- 일시정지 및 UI메뉴 : ESC
- 포탈 생성 : 마우스 좌클릭 - 포탈1  / 마우스 우클릭 - 포탈2

<br>

## ⚡ 주요 기능

### 🌀 포탈 시스템
두 개의 연결된 포탈을 생성하여 공간 간 순간이동이 가능합니다.

![4](https://github.com/user-attachments/assets/af0504af-e3aa-4c9e-9c3d-c2df973337c2)


**핵심 기술**:
- Render Texture를 활용한 포털 뷰 렌더링
- 물리 기반 오브젝트 워핑 시스템
- 포털 간 연결 관리 및 충돌 처리

### 🎯 퍼즐 해결 시스템
다양한 상호작용 요소를 활용한 물리 기반 퍼즐을 제공합니다.

![3](https://github.com/user-attachments/assets/e83908d4-d50f-4019-b5ae-4894458fa9a3)


**구현 요소**:
- 버튼, 스위치, 압력판 시스템
- 컨베이어 벨트 및 점프 패드
- 문 제어 및 트리거 시스템

### 🚨 장애물 & 위험 요소
플레이어의 도전을 높이는 다양한 장애물이 구현되어 있습니다.

![5](https://github.com/user-attachments/assets/ca6f5ca1-27e0-4bf4-8e14-fe34f7c0a981)


**장애물 종류**:
- AI 기반 자동 추적 터렛
- 레이저 포인터 및 감지 시스템
- 낙하 데미지 구역

<br>

## 🛠️ 기술 스택

### 개발 환경
- **Engine**: Unity 2022.3.17f1
- **Language**: C#
- **Platform**: PC (Windows)

### 주요 Unity 패키지
- **Input System** (1.7.0): 현대적 입력 처리
- **TextMesh Pro** (3.0.6): UI 텍스트 렌더링
- **Post Processing** (3.4.0): 시각 효과 파이프라인
- **Timeline**: 시퀀스 및 컷신 제작

### 아키텍처 패턴
- **Singleton Pattern**: 매니저 클래스 관리
- **Component-Based Architecture**: 모듈화된 게임 오브젝트 설계
- **Observer Pattern**: 이벤트 기반 시스템
- **Inheritance**: 장애물 시스템 구조화

<br>

## 📁 프로젝트 구조

```
Assets/
├── Scripts/           # 게임 로직 (시스템별 분류)
│   ├── Manager/       # 싱글톤 매니저들
│   ├── Player/        # 플레이어 관련 기능
│   ├── Portal/        # 포털 메커니즘
│   ├── Obstacles/     # 장애물 및 적 시스템
│   └── Controller/    # 상호작용 요소들
├── Prefabs/          # 재사용 가능한 게임 오브젝트
├── Scenes/           # 게임 씬 및 테스트 씬
├── Audio/            # 음향 효과 및 배경음악
├── Animations/       # UI 및 게임플레이 애니메이션
└── Resources/        # 런타임 로드 에셋
```

<br>

## 💾 [게임 다운로드]
(배포_링크)

<br>

## 🏆 주요 성과

- ✅ 포털 기반 3D 퍼즐 시스템 구현 완료
- ✅ 물리 기반 오브젝트 상호작용 시스템 구축
- ✅ AI 기반 장애물 시스템 개발
- ✅ 모듈화된 아키텍처로 확장성 확보
- ✅ 완성도 높은 게임플레이 경험 제공

## 👥 개발팀 소개

<table align="center">
  <tr>
    <td align="center" width="200px">
      <br/>
      <b>팀장 [정진규]</b>
      <br/>
      <sub>포탈 시스템 & 저장 시스템</sub>
      <br/>
      <a href="https://github.com/Hira7388">
        <img src="https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white"/>
      </a>
    </td>
    <td align="center" width="200px">
      <br/>
      <b>팀원 [오승엽]</b>
      <br/>
      <sub>장애물 시스템 & 사운드</sub>
      <br/>
      <a href="https://github.com/Cae1umBlue">
        <img src="https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white"/>
      </a>
    </td>
    <td align="center" width="200px">
      <br/>
      <b>팀원 [최우영]</b>
      <br/>
      <sub>게임 디자인 & UI</sub>
      <br/>
      <a href="https://github.com/wooyoung-1">
        <img src="https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white"/>
      </a>
    </td>
    <td align="center" width="200px">
      <br/>
      <b>팀원 [심재환]</b>
      <br/>
      <sub>플레이어</sub>
      <br/>
      <a href="https://github.com/smjawhn">
        <img src="https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white"/>
      </a>
    </td>
    <td align="center" width="200px">
      <br/>
      <b>팀원 [김예찬]</b>
      <br/>
      <sub>퍼즐 시스템</sub>
      <br/>
      <a href="https://github.com/YCK1204">
        <img src="https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white"/>
      </a>
    </td>
  </tr>
</table>
