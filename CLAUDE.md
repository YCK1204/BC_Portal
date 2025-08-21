# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BC_Portal is a Korean Unity 3D puzzle-platformer game inspired by Portal, built in Unity 2022.3.17f1. This is a team project from a bootcamp ("내배캠" - Nae Bae Cam) featuring portal mechanics, physics-based puzzles, and obstacle systems.

## Language Preferences

- Developers prefer communication in Korean language
- Code comments and team discussions are typically conducted in Korean

## Build and Development Commands

### Unity Project Commands
- **Open in Unity**: Launch Unity Hub and open the project folder
- **Build**: Use Unity Editor → File → Build Settings → Build (no CLI build configured)
- **Play Mode**: Use Unity Editor Play button or Ctrl+P
- **Package Manager**: Window → Package Manager to manage dependencies

### No automated testing framework is currently set up in this project.

## High-Level Architecture

### Core Game Systems

**Portal System** (`Assets/Scripts/Portal/`):
- Portal placement and collision detection with surface validation
- Object warping and teleportation through linked portal pairs
- PortalableObject base class for objects that can travel through portals
- Custom rendering system using render textures

**Player System** (`Assets/Scripts/Player/`):
- Modular component-based architecture with Player.cs as main entity
- First-person movement controller using Unity's Input System
- Health/condition management and interaction system
- Animation controller integration

**Game Management** (`Assets/Scripts/Manager/`):
- Singleton pattern implementation for core managers
- StageManager handles level progression and spawn points
- SaveManager provides JSON-based save system for progress
- AudioManager controls sound effects and music

**Obstacle & Hazard System** (`Assets/Scripts/Obstacles/`):
- Inheritance-based architecture with ObstacleBase
- AI-controlled turrets with detection and firing mechanics
- Laser pointer obstacles and fall damage zones
- Projectile system for turret ammunition

**Interactive Elements** (`Assets/Scripts/Controller/Gimmick/`):
- Modular gimmick system for puzzle elements
- Buttons, switches, conveyor belts, jump pads
- Door controllers and pressure plate systems

### Key Design Patterns
- **Singleton**: Used for all manager classes (StageManager, SaveManager, PlayerManager)
- **Component-based**: Player and interactive objects use modular components
- **Inheritance**: ObstacleBase provides common interface for hazards
- **Observer**: Event-driven stage clearing system using ClearEvent

### Input System
- Modern Unity Input System (1.7.0) implementation
- PlayerInput.inputactions defines control scheme
- Supports movement, camera, portal shooting, jumping, and interaction

## Project Structure

```
Assets/
├── Scripts/           # Core game logic organized by system
│   ├── Manager/       # Singleton managers
│   ├── Player/        # Player-related functionality  
│   ├── Portal/        # Portal mechanics
│   ├── Obstacles/     # Hazards and enemy systems
│   └── Controller/    # Interactive elements
├── Prefabs/          # Organized by category (Player, Portal, Map, etc.)
├── Scenes/           # Multiple test/development scenes
├── Audio/            # Music, SFX, and TurretSound
├── Animations/       # UI and gameplay animation controllers
└── Resources/        # Runtime-loaded assets including portal materials
```

### Important Files
- `Assets/InputActions/PlayerInput.inputactions` - Input mappings
- `Assets/Scripts/Manager/StageManager.cs` - Level progression logic
- `Assets/Scripts/Portal/Portal.cs` - Core portal functionality
- `Assets/Scripts/Player/PlayerController.cs` - Movement and camera controls
- `ProjectSettings/ProjectSettings.asset` - Unity project configuration

## Development Guidelines

### Coding Conventions
- Korean comments are used throughout the codebase for team communication
- Singleton pattern is preferred for manager classes
- Component-based architecture for game objects
- Use existing gimmick controller system for new interactive elements

### Testing Setup
- Multiple test scenes available (YCK.unity, SYScene.unity, TestScene.unity)
- SaveDataScene_JJG.unity for save system testing
- No automated unit testing framework currently implemented

### Common Development Workflow
1. Use appropriate test scene for your feature area
2. Create prefabs in organized folders under Assets/Prefabs/
3. Follow existing architectural patterns (Singleton for managers, components for gameplay)
4. Test portal interactions for any new objects using PortalableObject base class
5. Use StageManager for level progression integration

### Package Dependencies
Key Unity packages used:
- Unity Input System (1.7.0) - Modern input handling
- TextMesh Pro (3.0.6) - UI text rendering  
- Post Processing (3.4.0) - Visual effects pipeline
- Cinemachine - Camera management
- Timeline - Cutscene/sequence system

## Architecture Notes

### Portal Rendering
- Uses render textures for portal views
- Custom shaders handle portal visual effects
- Materials stored in Assets/Resources/ for runtime access

### Save System
- JSON-based persistence in SaveManager
- Stage progression tracked through StageClearTrigger components
- Player progress saved to persistent data

### Audio System
- AudioMixer setup for sound management
- Organized audio assets (Music, SFX, TurertSound)
- EventClipAudio system for sound event handling

This is a well-structured Unity project with clear separation of concerns and modular design suitable for team development.

## Reference Game Design Notes

### 레퍼런스 게임: **"[Portal](https://namu.wiki/w/Portal%202)"**과 **"[The Witness](https://namu.wiki/w/The%20Witness)"**

설명: 3D 퍼즐 플랫폼 게임은 플레이어가 퍼즐을 풀며 장애물을 피해 목표 지점에 도달하는 게임입니다.

- **참고 이미지**
    - 포탈
        
        ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/d29ce2e5-012d-4869-a11c-f48d410a3f43/Untitled.png)
        
        ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/f51c9ca8-f3fe-4d0b-aae8-b58dfb339bf9/Untitled.png)
        
        ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/2b229854-b31f-41e8-9b21-8dacaf64003c/Untitled.png)
        
    - 더 위트니스
        
        ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/a2ee454b-ea80-4696-a6bc-40d0d20d4212/Untitled.png)
        
        ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/007e7414-52f2-4dba-bfc9-1ce66e9f66f7/Untitled.png)
        
        ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/3af0e94a-2580-4492-b9c7-1b08131d67df/Untitled.png)
        
- **필수 구현 사항**
    1. **퍼즐 디자인** (난이도: ★★★☆☆)
        - 다양한 퍼즐을 게임에 디자인하고 구현하여 게임의 핵심 플레이를 제공합니다.
        - 퍼즐의 난이도와 다양성을 고려하여 설계합니다.
    2. **플레이어 캐릭터 및 컨트롤** (난이도: ★★★☆☆)
        - 플레이어 캐릭터를 제작하고, 캐릭터를 조작할 수 있는 컨트롤러를 구현합니다.
        - 필요한 도구나 능력을 제공하여 퍼즐을 해결할 수 있도록 합니다.
    3. **퍼즐 해결 시스템** (난이도: ★★★☆☆)
        - 퍼즐 해결에 필요한 시스템을 구현하고, 퍼즐의 상호작용 및 해결 방법을 설계합니다.
        - 퍼즐 요소의 동작 메커니즘과 규칙을 구현합니다.
    4. **장애물 및 트랩** (난이도: ★★★☆☆)
        - 장애물과 트랩을 게임에 추가하여 플레이어의 도전을 높이고 퍼즐과 조화롭게 결합시킵니다.
    5. **목표 지점** (난이도: ★★☆☆☆)
        - 퍼즐을 풀고 목표 지점에 도달할 수 있는 목표 지점을 제공합니다.
        - 목표 지점 도달로 레벨 완료를 처리합니다.
    6. **게임 진행 상태 및 저장** (난이도: ★★★☆☆)
        - 퍼즐 해결 상태와 게임 진행 상태를 저장하고 관리하는 시스템을 구현합니다.
        - 플레이어의 진척 상황을 추적하고 레벨 별로 관리합니다.
    7. **사운드 및 음악** (난이도: ★☆☆☆☆)
        - 게임에 사운드 효과와 음악을 추가하여 게임의 분위기를 개선합니다.
    8. **UI 애니메이션 추가** (난이도: ★★★☆☆)
        - UI 노출, 전환 시 자연스럽게 이동, 페이드, 크기 변화 등 애니메이션을 추가합니다.
        - UI 애니메이션 (Unity 기본 Animator, 외부 라이브러리 Dotween)
    
- **선택 구현 사항**
    1. **고급 퍼즐 요소** (난이도: ★★★★☆)
        - 퍼즐의 난이도를 높이기 위해 고급 퍼즐 요소를 도입합니다.
        - 복합적인 논리나 물리학 요소를 활용한 퍼즐을 추가합니다.
    2. **포탈 시스템** (난이도: ★★★★☆)
        - 퍼즐 해결에 포탈 기술과 같은 고유한 시스템을 추가합니다.
        - 포탈을 이용한 공간 이동이나 시간 조작과 같은 퍼즐을 제공합니다.
    3. **스토리와 미스테리** (난이도: ★★★★☆)
        - 게임에 깊은 스토리나 미스테리 요소를 포함하여 게임의 내러티브를 강화합니다.
        - 퍼즐 해결과 스토리 진행을 연계합니다.
    4. **퍼즐 생성기** (난이도: ★★★★☆)
        - 플레이어들이 자신만의 퍼즐을 생성하고 공유할 수 있는 퍼즐 생성기를 제공합니다.
        - 사용자 맞춤형 콘텐츠 생성을 지원합니다.
    5. **업적 시스템** (난이도: ★★★☆☆)
        - 업적 시스템을 도입하여 플레이어의 동기부여를 높입니다.
        - 퍼즐 관련 업적을 추가하여 플레이어의 도전욕구를 자극합니다.
    6. **다양한 환경과 테마** (난이도: ★★☆☆☆)
        - 다양한 환경과 테마의 퍼즐을 추가하여 게임의 다양성을 확보합니다.
        - 환경에 따라 퍼즐의 특성을 다르게 디자인합니다.
    7. **퍼즐 편집기** (난이도: ★★★★☆)
        - 게임 내에서 퍼즐을 편집하고 수정할 수 있는 퍼즐 편집기를 구현합니다.
        - 플레이어들이 자신만의 퍼즐을 만들 수 있도록 합니다.

### Project Implementation Status
- 프로젝트에서 구현된 항목들을 추적하고 있습니다.
- 이 섹션은 개발 팀의 진행 상황을 기록하고 앞으로의 발전 방향을 제시합니다.
- 아직 전체 기능이 완전히 구현되지 않았으며, 지속적인 개발 중입니다.