# NGO 실습 검증 프로젝트

Unity **6000.6.5f1 (Unity 6.6.5)** 에서 DAY09의 Host·Client 연결과 CharacterController 이동을 확인하는 프로젝트입니다. 두 창은 같은 컴퓨터의 `127.0.0.1:7777`로 연결합니다. 별도 서비스 계정이나 중계 서버는 사용하지 않습니다.

## 기준 환경

| 구성 | 버전·설정 |
| --- | --- |
| Unity Editor | 6000.6.5f1 |
| NGO (Netcode for GameObjects) | 2.13.3 |
| Unity Transport | 6.6.0 — 사용 Editor의 패키지 해석 결과 |
| Input System | 1.20.0 / Active Input Handling = Input System Package (New) |
| UI | uGUI 2.6.0 / TextMesh Pro |
| 렌더링 | URP (Universal Render Pipeline) 17.6.0 |
| Unity Pipeline | 0.8.0-exp.1 — Editor 조작·검증용 |

**Unity Pipeline은 NGO 실행에 필요한 전송 패키지가 아닙니다.** 이 프로젝트에는 Editor 명령으로 씬·참조·실행을 검증하기 위해 설치했습니다. URP라는 렌더 파이프라인과도 별개의 패키지입니다.

## 바로 실행하기

1. Unity Hub에서 이 폴더를 기존 프로젝트로 추가하고 `6000.6.5f1`로 엽니다. 다른 버전으로 자동 업그레이드하지 않습니다.
2. `Assets/Scenes/NetworkPlay.unity`를 엽니다. Console에 컴파일 Error가 없는지 확인합니다.
3. Windows 실행 파일이 없다면 `File > Build Profiles`에서 Windows를 선택합니다. Scene List에 `NetworkPlay`만 포함하고 `Builds/Windows`에 빌드합니다.
4. Editor에서 Play Mode를 시작하고 Game 뷰의 `Host 시작`을 누릅니다. `Host 실행 / 참여자 1명`이 표시됩니다.
5. `Builds/Windows/NGO.exe`를 실행하고 해당 창의 `Client 시작`을 누릅니다. Host는 `참여자 2명`, Client는 `Client 연결 완료`로 바뀝니다.
6. Editor Game 뷰를 클릭하고 W/A/S/D 또는 방향키로 왼쪽 캐릭터를 움직입니다. Client 창에서도 같은 캐릭터가 움직여야 합니다.
7. Client 창을 클릭하고 같은 키로 오른쪽 캐릭터를 움직입니다. Editor에서도 그 이동이 보여야 합니다.
8. 벽을 향해 이동해 통과하지 않는지 확인합니다. 확인 후 Client 실행 파일을 닫고 Editor의 Play Mode를 종료합니다.

Host와 Client 버튼을 같은 실행 창에서 연달아 누르는 실습이 아닙니다. 코드·프리팹·씬을 바꾸면 실행 파일도 다시 빌드합니다. 카메라와 바닥은 두 참여자 실습용이며, 세 번째 Client의 시작 위치는 별도로 배치하지 않았습니다.

## 무엇이 준비되어 있나요?

- `Assets/Scenes/NetworkPlay.unity`: 바닥·벽·카메라·NetworkManager·Canvas·EventSystem이 구성된 씬.
- `Assets/Prefabs/PlayerNetworkPrefab.prefab`: NetworkObject, CharacterController, Owner 권한 NetworkTransform, NetworkCharacterPlayer가 있는 캡슐 플레이어.
- `Assets/Scripts/NetworkLauncher.cs`, `NetworkCharacterPlayer.cs`: 상위 `Day09` 폴더의 수업 코드와 동일한 복사본.
- `Assets/Settings/PracticeNetworkPrefabs.asset`: 플레이어 프리팹을 등록한 네트워크 프리팹 목록.
- `Assets/Fonts/NgoKoreanUI.asset`: 상태 메시지에 필요한 한국어와 영문 글자를 포함한 TMP 폰트.
- `Assets/Editor/ProjectBootstrap/NgoLabBuilder.cs`: 실습 씬을 구성한 Editor 도구. 기존 실습 씬·프리팹이 있으면 덮어쓰지 않습니다.
- `Assets/Scripts/Verification/NetworkSmokeRunner.cs`: 명시적으로 요청한 자동 검증에서만 생성되는 보조 코드. 일반 버튼 실습에서는 실행하지 않습니다.

Player Prefab의 **Authority Mode = Owner** 입니다. 입력과 중력은 IsOwner가 true인 캐릭터만 계산하며, 상대 복제본은 전달받은 위치를 표시합니다. 체력·전투 검증이나 서버 권한 이동은 아직 추가하지 않았습니다.

## 자동 검증의 범위

자동 검증은 Editor Host와 Windows Player Client를 실제 연결하고 저장된 버튼 콜백을 호출합니다. 가상 키보드 입력을 원본 이동 코드에 전달하여 다음 항목을 확인합니다.

- 두 PlayerObject 생성과 서로 다른 소유자.
- 각 창에서 자신의 CharacterController만 활성화.
- Host의 전진과 Client의 오른쪽 이동이 양쪽 복제본에 반영.
- Host 캐릭터의 벽 충돌과 바닥 유지.
- 실행 중 Error·Exception 여부.

자동 검증의 버튼 콜백 호출은 실제 마우스 클릭 검증과 구분합니다. 화면·버튼 조작은 위의 안내형 실행 절차로도 확인합니다. 구체적인 실행 결과와 제한은 [검증 기록](VERIFICATION.md)에 정리합니다.

Windows Player 두 개로 자동 검증을 다시 실행하려면, 먼저 Editor의 Play Mode와 기존 Host를 종료한 뒤 프로젝트 폴더의 PowerShell에서 실행합니다. `7777` 포트를 다른 프로그램이 사용하면 실패할 수 있습니다.

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Tools\RunSmoke.ps1
```

이 명령의 실행 정책 변경은 해당 PowerShell 프로세스에만 적용됩니다. 결과와 로그는 `Artifacts/Smoke-...`에 저장되고, 스크립트가 실행한 두 Player는 검증 후 종료됩니다.

현재 준비한 것은 **DAY09 연결·소유자 이동의 기준 프로젝트** 입니다. DAY10의 NetworkVariable·RPC, DAY11의 서버 권한 상호작용, DAY12의 UniTask 예제까지 모두 구현하거나 검증한 프로젝트는 아닙니다. 다음 실습은 이 기준 씬을 복사하여 확장할 수 있습니다.

## 파일 관리와 폰트 라이선스

이 프로젝트는 상위 C# 교육 저장소에 포함됩니다. 별도 Git 저장소나 원격 저장소는 만들지 않았습니다. `Assets`의 `.meta` 파일과 `Packages/manifest.json`, `packages-lock.json`은 보존하고, Library·Temp·UserSettings·Logs·Builds·Artifacts는 프로젝트별 `.gitignore`로 제외합니다.

**Git LFS (Git Large File Storage, 대용량 파일 저장 확장)** 는 폰트·이미지 같은 파일 본체를 별도로 저장하고 Git에는 참조를 기록합니다. 프로젝트의 `.gitattributes`는 바이너리 파일과 텍스처가 포함된 두 폰트 아틀라스를 LFS로 관리합니다. 씬·프리팹·일반 설정과 `.meta`는 텍스트로 유지합니다.

다른 컴퓨터에서 이 저장소를 내려받을 때는 Git LFS를 먼저 설치한 뒤 저장소 안에서 다음 명령을 실행합니다. ZIP 다운로드 대신 Git clone 사용을 권장합니다.

```powershell
git lfs install --local
git lfs pull
git lfs fsck
```

폰트·이미지 파일 내용이 `version https://git-lfs.github.com/spec/v1`로 시작하는 짧은 텍스트라면 LFS 참조만 받은 상태입니다. 본체를 받기 전에는 Unity를 열지 말고, `git lfs pull`의 다운로드 오류부터 해결합니다. 빌드 결과는 내려받는 대상이 아니므로 해당 컴퓨터에서 다시 빌드합니다.

한국어 폰트는 [Google Fonts의 Noto Sans KR](https://github.com/google/fonts/tree/main/ofl/notosanskr)을 사용합니다. SIL Open Font License 1.1과 저작권 안내는 `Assets/Fonts/OFL.txt`에 함께 포함했습니다.

## 학습 문서

- [DAY09: 연결과 캐릭터 이동](../../../Documents/Curriculum/10.GameNetwork/DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)
- [NGO 기본 구조와 컨셉](../../../Documents/Curriculum/10.GameNetwork/Supplement/NGO_ARCHITECTURE_AND_CONCEPTS.md)
- [핵심 컴포넌트 Inspector 설정](../../../Documents/Curriculum/10.GameNetwork/Supplement/NGO_COMPONENT_INSPECTOR_GUIDE.md)
