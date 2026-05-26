# GitHub Desktop 사용법 가이드

이 문서는 Git 명령어가 아직 익숙하지 않은 학생이 **GitHub Desktop**으로 프로젝트를 저장하고, GitHub에 올리고, 다시 내려받는 기본 흐름을 익히기 위한 참고 문서입니다.

GitHub Desktop은 터미널 명령어 대신 버튼과 화면으로 Git을 다루는 도구입니다. 게임으로 비유하면, Git은 "**세이브 시스템**", GitHub는 "**클라우드 저장소**", GitHub Desktop은 "**세이브 파일 관리 화면**"에 가깝습니다.

공식 참고 문서:

- [GitHub Desktop 공식 문서](https://docs.github.com/desktop)
- [GitHub Desktop 시작하기](https://docs.github.com/desktop/overview/getting-started-with-github-desktop)
- [GitHub Desktop 다운로드](https://desktop.github.com/)

---

## 1. GitHub Desktop으로 무엇을 하나요?

| 상황 | Git 용어 | GitHub Desktop에서 하는 일 | 쉬운 비유 |
| :--- | :--- | :--- | :--- |
| 프로젝트 상태를 저장 | Commit | 변경 파일을 고르고 커밋 작성 | 게임 세이브 |
| 내 저장 내용을 GitHub에 올림 | Push | `Push origin` 클릭 | 클라우드 업로드 |
| GitHub의 최신 내용을 받음 | Pull | `Pull origin` 클릭 | 클라우드 다운로드 |
| GitHub에 새 내용이 있는지 확인 | Fetch | `Fetch origin` 클릭 | 업데이트 확인 |
| GitHub 프로젝트를 내 PC로 복사 | Clone | 저장소를 선택해 내려받기 | 게임 설치 |
| 새 작업 줄기 만들기 | Branch | 새 브랜치 생성 | 평행 세계에서 실험 |

가장 중요한 흐름은 다음 3단계입니다.

```text
파일 수정 -> Commit -> Push
```

수업 중에는 이 흐름을 "**수정하고, 세이브하고, 클라우드에 올린다**"라고 기억하면 됩니다.

---

## 2. 설치와 로그인

### 1) 설치하기

1. 브라우저에서 [desktop.github.com](https://desktop.github.com/)에 접속합니다.
2. Windows용 설치 파일을 다운로드합니다.
3. 설치 파일을 실행하고 안내에 따라 설치합니다.

### 2) GitHub 계정으로 로그인하기

1. GitHub Desktop을 실행합니다.
2. `Sign in to GitHub.com`을 클릭합니다.
3. 브라우저가 열리면 GitHub 계정으로 로그인합니다.
4. 권한 승인 화면이 나오면 승인합니다.

로그인이 끝나면 GitHub Desktop이 내 GitHub 계정과 연결됩니다.

### 3) Git도 따로 설치해야 하나요?

GitHub Desktop은 Git을 모르는 프로그램이 아니라, 내부에서 Git을 사용해 커밋, 푸시, 풀을 처리하는 프로그램입니다. 그래서 GitHub Desktop만 사용한다면 보통 별도로 Git 명령어를 설치하지 않아도 기본 작업을 할 수 있습니다.

다만 수업이나 실무에서는 다음 상황 때문에 **Git for Windows**를 따로 설치해두는 것을 추천합니다.

- PowerShell, 명령 프롬프트, Git Bash에서 `git` 명령어를 직접 사용할 때
- VS Code, Rider, Unity 외부 도구가 시스템의 `git` 명령어를 찾을 때
- `git lfs track "*.fbx"`처럼 Git LFS 설정 명령을 직접 실행할 때
- GitHub Desktop이 아닌 다른 Git 도구와 함께 사용할 때

확인 방법:

```bash
git --version
```

위 명령을 터미널에서 실행했을 때 버전이 나오면 명령줄 Git을 사용할 수 있는 상태입니다. `git`을 찾을 수 없다는 메시지가 나오면 [Git for Windows](https://git-scm.com/download/win)를 설치합니다.

정리하면 다음과 같습니다.

| 질문 | 답 |
| :--- | :--- |
| GitHub Desktop만 쓸 건데 Git을 따로 설치해야 하나요? | 보통은 필요 없습니다. |
| 터미널에서 `git` 명령어를 쓸 건가요? | Git for Windows 설치를 추천합니다. |
| Unity LFS 설정을 할 건가요? | 명령줄에서 `git lfs track`을 써야 하므로 설치 여부를 확인합니다. |

---

## 3. 새 저장소 만들기

새 C# 콘솔 프로젝트나 개인 실습 폴더를 Git으로 관리하고 싶을 때 사용합니다.

1. GitHub Desktop 상단 메뉴에서 `File` -> `New repository...`를 선택합니다.
2. `Name`에 저장소 이름을 입력합니다.
   - 예: `CSharpStudy`, `ConsoleRPG`, `InventoryPractice`
3. `Local path`에서 프로젝트를 저장할 위치를 선택합니다.
4. 필요하면 `Initialize this repository with a README`를 체크합니다.
5. `Create repository`를 클릭합니다.

이 단계가 끝나면 내 컴퓨터에 Git이 관리하는 프로젝트 폴더가 생깁니다.

### README는 무엇인가요?

`README.md`는 프로젝트의 첫 안내판입니다. 게임 패키지 뒷면 설명처럼, 이 프로젝트가 무엇인지 적어두는 파일입니다.

---

## 4. 기존 폴더를 GitHub Desktop에 추가하기

이미 만들어둔 C# 프로젝트 폴더가 있다면 새로 만들 필요 없이 추가할 수 있습니다.

1. `File` -> `Add local repository...`를 선택합니다.
2. `Choose...`를 눌러 기존 프로젝트 폴더를 선택합니다.
3. `Add repository`를 클릭합니다.

만약 Git 저장소가 아니라는 안내가 나오면, GitHub Desktop이 해당 폴더를 저장소로 만들 수 있도록 안내합니다.

주의할 점:

- 프로젝트 폴더 전체를 선택합니다.
- `.csproj` 파일만 선택하는 것이 아니라, 그 파일이 들어있는 폴더를 선택합니다.
- `bin`, `obj`, `.vs` 같은 자동 생성 폴더는 보통 Git에 올리지 않습니다.

---

## 5. GitHub에서 프로젝트 내려받기(Clone)

GitHub에 이미 올라간 저장소를 내 컴퓨터로 가져올 때 사용합니다.

1. `File` -> `Clone repository...`를 선택합니다.
2. `GitHub.com` 탭에서 가져올 저장소를 선택합니다.
3. `Local path`에서 저장할 위치를 고릅니다.
4. `Clone`을 클릭합니다.

Clone은 "**GitHub 클라우드에 있던 프로젝트를 내 PC에 설치하는 것**"이라고 이해하면 됩니다.

---

## 6. 커밋하기: 세이브 포인트 만들기

파일을 수정한 뒤에는 반드시 커밋을 남겨야 합니다.

1. Visual Studio, VS Code, Rider 등에서 파일을 수정하고 저장합니다.
2. GitHub Desktop으로 돌아옵니다.
3. 왼쪽 `Changes` 영역에 바뀐 파일 목록이 보이는지 확인합니다.
4. 하단 `Summary`에 변경 내용을 짧게 적습니다.
   - 좋은 예: `Add player movement`
   - 좋은 예: `Fix inventory item count`
   - 아쉬운 예: `수정`
5. 필요한 경우 `Description`에 자세한 설명을 적습니다.
6. `Commit to main` 또는 `Commit to 현재브랜치명` 버튼을 클릭합니다.

커밋 메시지는 "**나중의 나에게 남기는 메모**"입니다. 한 달 뒤에 봐도 무슨 작업이었는지 알 수 있게 적는 것이 좋습니다.

---

## 7. Push하기: GitHub에 올리기

커밋은 먼저 내 컴퓨터에 저장됩니다. GitHub에도 올리려면 Push가 필요합니다.

1. 커밋을 만든 뒤 상단의 `Push origin` 버튼을 확인합니다.
2. `Push origin`을 클릭합니다.
3. GitHub 저장소 페이지에서 파일이 올라갔는지 확인합니다.

처음 만든 저장소라면 `Publish repository` 버튼이 보일 수 있습니다. 이 버튼은 "**내 컴퓨터에만 있던 저장소를 GitHub에 처음 공개하거나 업로드하는 버튼**"입니다.

수업 과제 제출용 저장소라면 공개 범위가 맞는지 확인합니다.

- `Public`: 누구나 볼 수 있음
- `Private`: 초대한 사람만 볼 수 있음

---

## 8. Pull과 Fetch: 최신 상태 맞추기

다른 컴퓨터에서 작업했거나, 팀원이 GitHub에 새 커밋을 올렸다면 내 컴퓨터도 최신 상태로 맞춰야 합니다.

### Fetch origin

`Fetch origin`은 GitHub에 새 변경 사항이 있는지 확인합니다.

비유하면 "**클라우드에 새 세이브 파일이 있는지 확인만 하는 버튼**"입니다.

### Pull origin

`Pull origin`은 GitHub에 있는 새 변경 사항을 실제로 내려받습니다.

비유하면 "**클라우드의 최신 세이브 파일을 내 컴퓨터에 적용하는 버튼**"입니다.

수업 시작 전 추천 습관:

```text
GitHub Desktop 열기 -> Fetch origin -> Pull origin이 보이면 클릭
```

---

## 9. Branch 사용하기

Branch는 원래 코드에서 갈라진 작업 줄기입니다. 게임의 평행 세계처럼, 원본을 망가뜨리지 않고 실험할 수 있습니다.

예를 들어 `main` 브랜치는 안정된 기본 세계이고, `feature/inventory` 브랜치는 인벤토리 기능을 실험하는 세계입니다.

### 새 브랜치 만들기

1. 상단의 `Current Branch`를 클릭합니다.
2. `New Branch`를 선택합니다.
3. 브랜치 이름을 입력합니다.
   - 예: `feature/player-move`
   - 예: `practice/day03-class`
4. `Create Branch`를 클릭합니다.

브랜치 이름은 띄어쓰기 없이 영어 소문자와 하이픈을 쓰면 관리하기 쉽습니다.

### 브랜치 합치기

작업이 끝난 브랜치는 보통 Pull Request 또는 Merge를 통해 기본 브랜치에 합칩니다. 입문 단계에서는 혼자 작업하더라도 `main`에 바로 실험하기보다 브랜치를 만들어보는 연습이 좋습니다.

---

## 10. 충돌(Conflict)이 났을 때

충돌은 같은 파일의 같은 부분을 서로 다르게 고쳤을 때 발생합니다.

비유하면 한 사람은 대사 파일에 "안녕하세요"를 적고, 다른 사람은 같은 줄에 "반갑습니다"를 적은 상태입니다. Git은 둘 중 무엇을 살릴지 스스로 결정하지 못하므로 사람에게 물어봅니다.

충돌이 났을 때 기본 대응:

1. 당황하지 말고 충돌 파일 이름을 확인합니다.
2. GitHub Desktop에서 안내하는 파일을 에디터로 엽니다.
3. 남길 코드와 지울 코드를 직접 정리합니다.
4. 파일을 저장합니다.
5. GitHub Desktop에서 충돌이 해결되었는지 확인합니다.
6. 다시 커밋합니다.

충돌 표시 예시:

```text
<<<<<<< HEAD
내 컴퓨터에서 수정한 코드
=======
GitHub에서 내려온 코드
>>>>>>> origin/main
```

이 표시는 최종 코드에 남아 있으면 안 됩니다. 필요한 코드만 남기고 충돌 표시 줄은 삭제해야 합니다.

---

## 11. C# 프로젝트에서 조심할 파일

C# 프로젝트는 빌드할 때 자동 생성되는 파일이 많습니다. 모든 파일을 Git에 올리면 저장소가 지저분해질 수 있습니다.

보통 Git에 올리는 파일:

- `.cs`
- `.csproj`
- `.sln`
- `README.md`
- 직접 만든 이미지, 문서, 데이터 파일

보통 Git에 올리지 않는 폴더:

- `bin/`
- `obj/`
- `.vs/`
- `.idea/`

GitHub Desktop의 변경 목록에 `bin`, `obj` 파일이 너무 많이 보이면 `.gitignore` 설정을 확인합니다. 새 저장소를 만들 때 `.gitignore` 템플릿에서 `VisualStudio`를 선택하면 C# 프로젝트에 맞는 제외 규칙을 쉽게 만들 수 있습니다.

---

## 12. Unity 프로젝트를 저장소에 추가할 때 주의할 점

Unity 프로젝트는 C# 콘솔 프로젝트보다 자동 생성 파일이 훨씬 많습니다. 프로젝트 전체를 아무 생각 없이 GitHub에 올리면 용량이 커지고, 다른 컴퓨터에서 열 때 문제가 생길 수 있습니다.

Unity 프로젝트를 Git에 올릴 때는 "**게임 제작에 필요한 원본 재료만 올리고, Unity가 다시 만들 수 있는 임시 파일은 빼기**"라고 생각하면 됩니다.

### Git에 올려야 하는 폴더와 파일

보통 저장소에 포함해야 하는 항목:

- `Assets/`
- `Packages/`
- `ProjectSettings/`
- `UserSettings/` 일부 설정 파일
- `.gitignore`
- `README.md`

특히 `Assets/`는 게임의 실제 재료 창고입니다. 스크립트, 씬, 프리팹, 머티리얼, 이미지, 사운드 등이 들어 있으므로 대부분 Git에 올려야 합니다.

`Packages/manifest.json`과 `Packages/packages-lock.json`도 중요합니다. 이 파일들은 프로젝트가 어떤 Unity 패키지를 쓰는지 적어둔 목록입니다. 다른 컴퓨터에서 프로젝트를 열 때 같은 패키지를 맞추는 데 필요합니다.

### Git에 올리지 않는 폴더

보통 저장소에서 제외해야 하는 항목:

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `MemoryCaptures/`
- `.vs/`
- `.idea/`
- 자동 생성된 IDE 파일

`Library/` 폴더는 특히 조심해야 합니다. Unity가 프로젝트를 열면서 다시 만드는 캐시 폴더라서 용량이 매우 큽니다. 이 폴더를 GitHub에 올리는 것은 게임의 원본 재료가 아니라 작업장 먼지와 임시 상자를 같이 택배로 보내는 것과 비슷합니다.

### Unity용 .gitignore 사용하기

Unity 프로젝트 저장소에는 Unity 전용 `.gitignore`가 필요합니다.

GitHub Desktop에서 새 저장소를 만들 때 `.gitignore` 템플릿을 고를 수 있다면 `Unity`를 선택합니다.

이미 저장소를 만든 뒤라면 GitHub의 Unity `.gitignore` 예시를 참고해 직접 추가할 수 있습니다.

- [Unity .gitignore 보기](https://github.com/github/gitignore/blob/main/Unity.gitignore)
- [Unity .gitignore Raw 다운로드](https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore)

주의할 점:

- `.gitignore`를 만들기 전에 이미 `Library/`를 커밋했다면, `.gitignore`만 추가해도 자동으로 사라지지 않습니다.
- 실수로 큰 폴더를 커밋했다면 혼자 강제로 되돌리기보다 강사에게 먼저 보여줍니다.
- GitHub Desktop의 `Changes` 목록에 `Library/` 파일이 많이 보이면 커밋하기 전에 멈추고 `.gitignore`를 확인합니다.

### Unity 에디터 설정 확인하기

협업하거나 다른 컴퓨터에서 프로젝트를 열 계획이 있다면 Unity 설정도 확인합니다.

1. Unity에서 `Edit` -> `Project Settings...`를 엽니다.
2. `Editor` 항목으로 이동합니다.
3. `Asset Serialization`을 `Force Text`로 설정합니다.
4. 가능하면 `Version Control` 관련 설정에서 숨김 메타 파일이 아닌 일반 메타 파일을 사용하는지 확인합니다.

버전에 따라 메뉴 이름이 조금 다를 수 있지만, 핵심은 Unity 파일을 사람이 읽을 수 있는 텍스트 형태로 저장하는 것입니다. 이렇게 해야 Git에서 변경 내용을 비교하기 쉽습니다.

### .meta 파일은 지우면 안 됩니다

Unity의 `.meta` 파일은 각 에셋의 신분증입니다.

예를 들어 `Player.png` 옆에 `Player.png.meta`가 있다면, Unity는 이 `.meta` 파일을 보고 "이 이미지는 어떤 고유 ID를 가진 에셋인가?"를 기억합니다.

주의할 점:

- `Assets/` 안의 `.meta` 파일은 함께 커밋합니다.
- 이미지나 스크립트를 옮길 때는 가능하면 Unity 에디터 안에서 옮깁니다.
- 파일 탐색기에서 `.meta`만 따로 삭제하지 않습니다.

`.meta` 파일이 사라지면 프리팹, 씬, 머티리얼 연결이 끊길 수 있습니다. 비유하면 물건은 남아 있는데 이름표와 바코드를 떼어버린 상태입니다.

### 씬과 프리팹 충돌 조심하기

Unity의 `.unity` 씬 파일과 `.prefab` 파일은 여러 사람이 동시에 수정하면 충돌이 나기 쉽습니다.

협업할 때 추천 습관:

- 같은 씬을 여러 명이 동시에 수정하지 않습니다.
- 담당 씬이나 담당 프리팹을 나눕니다.
- 작업 시작 전 `Pull origin`으로 최신 상태를 받습니다.
- 작업 종료 후 바로 `Commit`과 `Push`를 합니다.
- 큰 씬 수정 전에는 팀원에게 먼저 공유합니다.

혼자 작업하더라도 노트북과 학원 PC를 번갈아 쓴다면 같은 문제가 생길 수 있습니다. 한 컴퓨터에서 작업을 끝냈다면 반드시 Push하고, 다른 컴퓨터에서는 먼저 Pull한 뒤 작업합니다.

### 대용량 파일 주의하기

Unity 프로젝트에는 이미지, 사운드, 영상, 3D 모델처럼 큰 파일이 들어갈 수 있습니다.

GitHub 일반 저장소에 너무 큰 파일을 계속 올리면 저장소가 무거워집니다. 프로젝트 규모가 커지면 Git LFS를 사용하기도 합니다.

GitHub Desktop을 설치하면 Git LFS도 함께 설치됩니다. 하지만 이것은 "**LFS 도구가 준비되어 있다**"는 뜻이지, Unity 프로젝트의 큰 파일들이 자동으로 LFS에 들어간다는 뜻은 아닙니다. GitHub 공식 문서에서도 GitHub Desktop과 함께 LFS를 사용하려면 명령줄에서 Git LFS 추적 설정을 해야 한다고 안내합니다.

- [GitHub Desktop과 Git LFS 공식 문서](https://docs.github.com/ko/desktop/configuring-and-customizing-github-desktop/about-git-large-file-storage-and-github-desktop)
- [Unity .gitattributes 템플릿 보기](https://github.com/gitattributes/gitattributes/blob/master/Unity.gitattributes)
- [Unity .gitattributes Raw 다운로드](https://raw.githubusercontent.com/gitattributes/gitattributes/master/Unity.gitattributes)

즉, 큰 에셋을 LFS로 관리하려면 보통 다음 두 가지가 필요합니다.

1. `git lfs track` 명령으로 추적할 파일 종류를 등록합니다.
2. 생성되거나 수정된 `.gitattributes` 파일을 함께 커밋합니다.

예시:

```bash
git lfs track "*.psd"
git lfs track "*.fbx"
git lfs track "*.wav"
git lfs track "*.mp4"
git lfs track "*.zip"
```

위 명령을 실행하면 저장소 루트에 `.gitattributes` 파일이 생성되거나 수정됩니다. 이 파일은 "어떤 확장자를 LFS로 관리할지 적어둔 규칙표"입니다.

예시 `.gitattributes`:

```gitattributes
*.psd filter=lfs diff=lfs merge=lfs -text
*.fbx filter=lfs diff=lfs merge=lfs -text
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp4 filter=lfs diff=lfs merge=lfs -text
*.zip filter=lfs diff=lfs merge=lfs -text
```

중요한 점:

- `.gitattributes`는 반드시 Git에 커밋해야 합니다.
- `.gitattributes`를 커밋하지 않으면 다른 컴퓨터에서 어떤 파일을 LFS로 받아야 하는지 알 수 없습니다.
- 이미 일반 Git 파일로 커밋된 대용량 파일은 나중에 LFS 규칙을 추가해도 자동으로 과거 기록이 바뀌지 않습니다.
- `git lfs track`은 보통 명령 프롬프트, PowerShell, Git Bash, 또는 GitHub Desktop의 `Repository` -> `Open in Command Prompt`에서 실행합니다.
- Git LFS는 저장 용량과 트래픽 제한이 있으므로, 수업용 작은 프로젝트에서는 꼭 필요한 큰 원본 파일에만 사용합니다.

입문 단계에서는 다음 원칙을 지키면 좋습니다.

- 사용하지 않는 에셋은 `Assets/`에 넣지 않습니다.
- 영상 파일이나 고해상도 원본 파일은 꼭 필요한 경우에만 저장소에 넣습니다.
- 에셋 스토어에서 받은 대형 샘플 전체를 무작정 커밋하지 않습니다.
- GitHub Desktop에서 변경 파일 수와 용량이 갑자기 많아지면 커밋 전에 확인합니다.

### Unity 프로젝트 추가 전 체크리스트

```text
1. Unity 프로젝트 루트 폴더를 선택했는가?
   예: MyUnityGame/
   그 안에 Assets, Packages, ProjectSettings가 보여야 함

2. Unity용 .gitignore가 있는가?
   Library, Temp, Obj, Build, Logs 등이 제외되어야 함

3. 대용량 에셋이 있다면 .gitattributes를 확인했는가?
   Git LFS 추적 규칙은 자동으로 정해지지 않음

4. Assets 안의 .meta 파일을 함께 올리는가?
   .meta 파일은 Unity 에셋의 신분증

5. Library 폴더가 Changes에 보이지 않는가?
   보이면 커밋 전에 멈추고 .gitignore 확인

6. 작업 전 Pull, 작업 후 Commit과 Push를 했는가?
   컴퓨터를 바꿔 작업할 때 특히 중요
```

Unity 저장소 관리는 "무엇을 올릴까?"보다 "무엇을 올리지 말까?"가 더 중요합니다. `Assets`, `Packages`, `ProjectSettings`는 원본 재료이고, `Library`, `Temp`, `Build`는 Unity가 다시 만들 수 있는 작업 결과물이라고 구분하면 됩니다.

---

## 13. 수업 중 추천 작업 습관

### 수업 시작 전

1. GitHub Desktop을 엽니다.
2. 오늘 작업할 저장소를 선택합니다.
3. `Fetch origin`을 누릅니다.
4. `Pull origin`이 보이면 먼저 누릅니다.

### 실습 중

1. 작은 기능 하나를 완성합니다.
2. 실행해서 확인합니다.
3. 커밋 메시지를 적고 커밋합니다.

좋은 커밋 단위:

- 변수와 자료형 예제 추가
- 몬스터 공격 메서드 구현
- 인벤토리 출력 버그 수정
- 퀴즈 풀이 파일 추가

너무 큰 커밋:

- 하루치 모든 작업을 한 번에 커밋
- 여러 기능과 버그 수정을 한 커밋에 섞기

### 수업 종료 전

1. 마지막 커밋을 만듭니다.
2. `Push origin`을 클릭합니다.
3. GitHub 웹사이트에서 파일이 올라갔는지 확인합니다.

---

## 14. 자주 생기는 문제

### Changes에 파일이 안 보여요

- 파일을 저장했는지 확인합니다.
- GitHub Desktop에서 올바른 저장소를 열었는지 확인합니다.
- 프로젝트 폴더 바깥에 파일을 만든 것은 아닌지 확인합니다.

### Push가 안 돼요

- 인터넷 연결을 확인합니다.
- GitHub 로그인이 풀렸는지 확인합니다.
- 먼저 `Fetch origin` 또는 `Pull origin`이 필요한 상황인지 확인합니다.

### 실수로 이상한 파일을 커밋했어요

- 아직 Push 전이라면 커밋을 되돌릴 수 있습니다.
- Push 후라면 혼자 판단하지 말고 강사에게 먼저 보여줍니다.
- 비밀번호, 토큰, 개인 정보가 올라간 경우에는 파일 삭제만으로 끝나지 않을 수 있습니다.

### main에 바로 작업해도 되나요?

개인 연습 저장소라면 가능하지만, 협업이나 제출 프로젝트에서는 브랜치를 나누는 습관이 좋습니다. `main`은 제출 가능한 안정 버전, 브랜치는 실험 공간으로 생각하면 됩니다.

---

## 15. 한 장 요약

```text
처음 가져오기: Clone
기존 폴더 추가: Add local repository
새 저장소 생성: New repository

작업 기본 흐름:
1. 파일 수정
2. GitHub Desktop에서 Changes 확인
3. Summary 작성
4. Commit
5. Push origin

수업 시작 전:
Fetch origin -> Pull origin

수업 종료 전:
Commit -> Push origin -> GitHub 웹사이트 확인

Unity 프로젝트:
Assets, Packages, ProjectSettings는 올림
Library, Temp, Obj, Build, Logs는 제외
.meta 파일은 Assets와 함께 커밋
대용량 에셋은 git lfs track 후 .gitattributes도 커밋
```

GitHub Desktop은 Git을 대신 이해해주는 도구가 아니라, Git의 동작을 눈으로 보기 쉽게 도와주는 도구입니다. 버튼을 누를 때마다 "지금 내가 세이브하는가, 업로드하는가, 다운로드하는가?"를 떠올리면 Git 흐름이 훨씬 빨리 익숙해집니다.
