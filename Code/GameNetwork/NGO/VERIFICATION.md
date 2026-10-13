# NGO 실습 프로젝트 검증 기록

검증일: 2026-10-13. 대상은 Unity **6000.6.5f1** 과 NGO **2.13.3** 으로 구성한 DAY09 연결·캐릭터 이동 실습입니다.

## 결과 요약

| 확인 항목 | 결과 |
| --- | --- |
| Editor 컴파일 | 오류 없음, `recompile_status`의 `failed=false`, `compilationFailed=false` |
| Windows 개발용 Player 빌드 | Succeeded / Error 0 / Warning 1 |
| Editor Host + Windows Client 실제 접속 | 양쪽 `passed=true`, 각 피어에서 PlayerObject 2개 확인 |
| Windows Host + Windows Client 실제 접속 | 양쪽 `passed=true`, 재검증 스크립트 PASS |
| 소유권 | Host 소유자 0, Client 소유자 1; 양쪽 오브젝트 ID·소유자 일치 |
| 입력·물리 처리 | 각 피어에서 자기 CharacterController만 활성화 |
| 이동·복제본 동기화 | Host 전진, Client 오른쪽 이동이 상대에게 반영; 양쪽 각 좌표축 차이 0.05 이내 |
| 벽·바닥 충돌 | Host가 벽 앞에서 멈춤, 두 캐릭터가 바닥 위에 유지됨 |
| 실행 로그 | 검증 구간의 Error·Exception·Assert 0; Editor Console Warning 0 |
| 프로젝트 파일 검사 | `.meta` 누락·고아 파일, GUID 중복, 충돌 표시, manifest·Editor 버전 문제 없음 |
| 수업 코드 보존 | Launcher·CharacterPlayer 복사본이 상위 Day09 코드와 파일 해시까지 동일 |

마지막 빌드 ID는 `build_92cb6b0399d2`이며 빌드 시간은 약 9초였습니다. 개발용 Windows x64 Player에서 검증했으며 다른 운영체제나 IL2CPP 빌드의 검증 결과는 아닙니다.

## 실제 실행 근거

Editor는 Play Mode로 전환한 뒤 `Time.frameCount`가 변하는 것을 확인했습니다. 예를 들어 두 번째 실행의 프레임 값은 `7126 → 7144`로 증가했습니다. 단순히 Play 명령이 성공했다는 것과 실제 프레임 진행을 구분했습니다.

Editor Host와 Windows Client의 검증 결과:

| 피어 | 결과 | 결과 기록 시 프레임 | 가상 입력을 보낸 프레임 |
| --- | --- | ---: | ---: |
| Editor Host | PASS | 8011 | 224 |
| Windows Client | PASS | 21705 | 4172 |

Host에서 관측한 최종 위치는 대략 `(-2, 0.08, 2.92)`와 `(6.0053, 0.08, 0)`였습니다. Client에서도 같은 오브젝트가 약 0.005 이내의 좌표 차이로 관측됐습니다. `0.08` 높이는 CharacterController의 Skin Width에 따른 접촉 여유이며, 추락이 아닙니다.

아래는 참여자 2명과 이동 후 캐릭터를 확인한 실제 Editor Game 뷰입니다.

![Host 참여자 2명과 이동한 두 캐릭터](Assets/Screenshots/NGO-connected.png)

최종 실행 파일은 `Tools/RunSmoke.ps1`로 Windows Host·Client 두 개를 실행하여 다시 검사했습니다. 양쪽 모두 PASS였고 Host의 입력 프레임 6135, Client의 입력 프레임 4122, Error 0을 기록했습니다. 프레임 수 차이는 Editor·Player 실행 속도 차이이며 통신 횟수나 보장 성능을 뜻하지 않습니다.

원본 JSON·로그는 로컬 `Artifacts`에 보존했습니다. 이 폴더와 실행 파일은 Git에서 제외되므로 다른 컴퓨터에서는 README 안내대로 다시 빌드·검증합니다.

- Editor와 Player 검증: `Artifacts/host-smoke.json`, `client-smoke.json`, `Logs/ClientSmoke.log`.
- 최종 Player 두 개 검증: `Artifacts/Smoke-c19ff223bb184fbe896e4d1b6793f573/`.

## 발견하고 수정한 사항

1. 씬 생성 도구가 VolumeProfile의 효과를 하위 에셋으로 저장하지 않아 재컴파일 후 참조가 비어졌습니다. `AssetDatabase.AddObjectToAsset`로 Tonemapping·Bloom·Vignette를 저장하도록 수정하고 기존 실습 프로필도 Unity API로 복구했습니다. 재컴파일 이후에도 세 효과가 유효한 저장 에셋임을 확인했습니다. Unity 패키지 소스는 수정하지 않았습니다.
2. 첫 자동 검증에서 Client 입력은 작동했지만 Editor 입력이 Game 뷰 초점 설정에 막혔습니다. 검증 중에만 Input System의 Editor 입력 전달 정책을 바꾸고, 종료 시 기존 정책을 복구하도록 보완했습니다. 학생용 이동 코드를 바꿔 검사를 통과시킨 것이 아닙니다. 첫 실패 결과도 `Artifacts/*-smoke-first.json`에 남겼습니다.
3. 검증 도구의 객체 검색을 Unity 6.6의 권장 `FindObjectsByType<T>()`로 바꿨습니다. 최종 빌드 전 명시적으로 컴파일 상태를 확인했습니다.

## 남은 경고와 검증하지 않은 범위

마지막 빌드의 경고 1건은 `enableInBuilds=false`이므로 Unity Pipeline이 Player에서 실행되지 않는다는 안내입니다. 의도한 설정이며 NGO 통신 오류가 아닙니다. Editor의 Pipeline 서버는 사용하지만 Player에 원격 제어 서버를 활성화하지 않았습니다. 초기 빌드에 있었던 URP 디버그 셰이더 스트리핑 경고는 최종 증분 빌드에서는 기록되지 않았으며, 향후 전체 재빌드에서 다시 표시될 수 있습니다.

자동 검증은 씬에 저장된 Button의 `onClick` 콜백을 호출하고 가상 Keyboard 입력을 전달합니다. 실제 마우스로 버튼을 클릭하는 과정이나 실제 키보드 장치 자체까지 자동 검증한 것은 아닙니다. 한국어 상태 문구와 버튼은 Game 뷰 캡처로 확인했고, 직접 조작하는 절차는 README에 정리했습니다.

실제 접속은 같은 PC의 `127.0.0.1:7777`로 이루어졌습니다. 다른 PC, 공유기·방화벽, 인터넷, Relay, WebSocket, 악성 입력 검증, 장시간 연결, 다수 참여자 부하와 연결 해제 후 재접속은 이번 검증에 포함하지 않았습니다. DAY10~12의 추가 기능도 아직 포함하지 않습니다.

프로젝트 생성 검증 후 Player 프로세스를 종료하고 Editor를 `NetworkPlay` 씬의 편집 상태로 돌려놓았습니다. 생성 검증 당시에는 커밋·푸시하지 않았으며, 이후 별도 요청에 따라 관련 문서·프로젝트를 버전 관리 대상으로 준비했습니다. 다른 과정의 수정은 포함하지 않습니다.

## Git 버전 관리 점검

- 프로젝트 전용 `.gitattributes`에서 바이너리 폰트·이미지와 대형 폰트 아틀라스를 LFS로 관리합니다. 현재 대상은 6개이며 Git 인덱스에 정상 LFS 참조로 저장되는 것을 확인했습니다.
- `.gitignore`에 Unity CLI의 현재 생성 파일 무시 규칙을 보완했습니다. Library·Temp·Logs·Builds·UserSettings·Artifacts가 커밋 대상에 없음을 확인했습니다.
- 씬·프리팹·일반 설정은 Force Text, `.meta`와 패키지 manifest·lock은 보존합니다. `unity vcs doctor`는 Error 0, Warning 0입니다. 상위 C# 저장소의 `.meta` 제외를 이 프로젝트에서 의도적으로 해제했다는 Notice 1건은 정상입니다.
- Unity가 생성한 YAML의 빈 값 뒤 공백과 원본 TMP 리소스의 서식은 그대로 보존했습니다. 이를 지우기 위해 검증된 에셋이나 제3자 소스를 재작성하지 않았습니다.
- 다른 컴퓨터에서 LFS 본체를 내려받는 방법은 README의 파일 관리 절차를 따릅니다. 런타임 Pipeline 서버는 계속 비활성 상태입니다.
