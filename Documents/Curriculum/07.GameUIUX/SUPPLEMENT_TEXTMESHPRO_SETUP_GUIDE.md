# TextMeshPro 초기 설정과 한글 폰트 가이드

## 1. 이 문서에서 해결할 문제

UGUI에서 `Text - TextMeshPro`를 처음 만들거나 `TextMeshProUGUI`를 처음 사용할 때 **TMP Importer** 창이 나타날 수 있습니다. 이 창에서 필수 리소스를 가져오는 방법과, 기본 글꼴에 없는 한글을 표시하기 위한 Font Asset 생성·연결 방법을 정리합니다.

이 문서는 `07.GameUIUX`의 Canvas용 텍스트를 기준으로 합니다. 3D 월드에 놓는 `TextMeshPro`가 아니라 Canvas 자식의 `TextMeshProUGUI`를 사용합니다.

## 2. 처음 나타나는 TMP Importer 창

첨부한 창은 TextMeshPro가 처음 필요한 기본 리소스를 프로젝트에 추가하기 전에 표시하는 안내입니다. 두 선택지는 용도가 다릅니다.

| 버튼 | 선택 | 이유 |
| :--- | :--- | :--- |
| `Import TMP Essentials` | **반드시 선택** | 기본 Font Asset, Shader, Material, TMP Settings 등 TextMeshPro를 표시하는 데 필요한 리소스를 가져옵니다. |
| `Import TMP Examples & Extras` | 선택 사항 | 예제 씬과 추가 샘플을 가져옵니다. 이 과정의 실습에 필수는 아니며, 예제를 따로 살펴볼 때만 가져옵니다. |

### Essentials 가져오기

1. TMP Importer 창에서 `Import TMP Essentials`를 클릭합니다.
2. Unity가 에셋을 가져오고 스크립트를 다시 컴파일할 때까지 기다립니다.
3. Project 창에 `TextMesh Pro` 폴더와 기본 리소스가 생겼는지 확인합니다.
4. `GameObject > UI > Text - TextMeshPro`를 다시 만들어 봅니다. 새 오브젝트의 Inspector에 `TextMeshProUGUI` 컴포넌트가 있고, `Text Input`에 입력한 영문이 Game 뷰에 보이면 준비가 끝난 것입니다.

`Import TMP Examples & Extras`를 선택하지 않아도 Button, HUD, 메뉴, 점수, 토스트 UI를 만드는 데 지장이 없습니다. 수업용 프로젝트에 불필요한 예제 에셋을 늘리고 싶지 않다면 생략합니다.

### 창을 닫았거나 기본 리소스가 없을 때

메뉴에서 `Window > TextMeshPro > Import TMP Essential Resources`를 선택합니다. 가져온 뒤에도 분홍색 텍스트 또는 누락 오류가 남으면 Console의 첫 오류를 확인하고, `TextMesh Pro` 폴더를 임의로 삭제하거나 이동하지 않았는지 확인합니다.

## 3. 한글이 네모(□)로 나오는 이유

TextMeshPro는 글자 모양을 Font Asset의 **Glyph** 정보와 Atlas 텍스처에 저장해 그립니다. 기본 Font Asset에는 영문·숫자만 들어 있고 한글 Glyph가 없을 수 있습니다. 이 경우 `안녕하세요`를 입력해도 글자 모양을 찾지 못해 네모 또는 누락 문자로 보입니다.

따라서 한글을 표시하려면 한글 Glyph를 포함한 글꼴 파일(`.ttf` 또는 `.otf`)에서 TMP용 Font Asset을 만든 뒤, 텍스트 컴포넌트에 연결해야 합니다. 글꼴 파일은 프로젝트의 배포 목적에 맞는 라이선스를 확인한 뒤 사용합니다.

## 4. 한글 Font Asset 만들기

### 4.1 글꼴 파일 준비

한글 Font Asset의 원본으로는 Unity가 가져올 수 있는 `.ttf` 또는 `.otf` 파일을 사용합니다. 웹에서 쓰는 `.woff`, `.woff2`와 여러 글꼴을 하나로 묶은 `.ttc`, `.otc` 파일은 이 과정의 TMP 원본으로 사용하지 않습니다. Variable Font도 확장자가 `.ttf`일 수 있지만, 처음 만드는 Font Asset에서는 굵기가 고정된 `Regular` 또는 `Bold` 정적 파일이 결과를 비교하고 다시 만들기 쉽습니다.

| 글꼴 | 용도와 한글 지원 | 내려받기 | TMP에 선택할 파일 |
| :--- | :--- | :--- | :--- |
| Pretendard | 기본 UI 예시로 권장하는 한글 고딕체입니다. 일반 `Pretendard`와 `Pretendard GOV`에는 한글이 포함되지만, `Pretendard Std`는 라틴·그리스·키릴 문자 전용이므로 한글 UI에 사용하지 않습니다. | [Pretendard 공식 Releases](https://github.com/orioncactus/pretendard/releases) | 정적 `Pretendard-Regular.otf` 또는 `.ttf` |
| Noto Sans KR | 한글·영문·숫자·기호를 함께 쓰는 일반 UI의 대안입니다. `Noto Sans` 일반판이 아니라 `Noto Sans KR` 또는 `NotoSansCJKkr` 파일을 선택합니다. | [Noto Sans KR](https://fonts.google.com/noto/specimen/Noto+Sans+KR) | 정적 `NotoSansCJKkr-Regular.otf` 또는 `.ttf` |
| IBM Plex Sans KR | 숫자와 영문을 또렷하게 보여 주고 싶은 HUD·디버그 정보 UI의 대안입니다. | [IBM Plex 공식 Releases](https://github.com/IBM/plex/releases) | 정적 `IBMPlexSansKR-Regular.otf` 또는 `.ttf` |

세 글꼴은 모두 한글 Glyph를 포함한 계열이며, OFL (SIL Open Font License 1.1)로 제공됩니다. OFL은 상업적 사용·수정·재배포를 허용하지만, 글꼴 파일만 따로 판매하거나 수정본에 원래의 예약 이름을 사용하는 등의 조건은 지켜야 합니다. 프로젝트에 원본 글꼴 파일을 포함해 배포한다면 해당 글꼴과 함께 제공된 라이선스 파일도 보관합니다.

1. 위 표에서 고른 한글 Glyph 포함 `.ttf` 또는 `.otf` 파일을 준비합니다.
2. Project 창의 예를 들어 `Assets/Fonts` 폴더에 넣습니다.
3. 아래의 원본 Font Inspector 설정을 확인합니다.

글꼴 원본 파일은 Font Asset을 다시 만들 때 필요합니다. 이미 만든 `.asset`만 남기지 말고 원본 글꼴 파일과 라이선스 파일도 프로젝트에서 함께 관리합니다.

### 4.2 원본 글꼴 파일의 Inspector 설정

Project 창에서 원본 `.ttf` 또는 `.otf` 파일을 선택하면 Inspector에 Font Import Settings가 표시됩니다. 이 Inspector는 원본 글꼴을 Unity에 가져오는 설정이며, **한글 Glyph 전체를 목록으로 보여 주거나 한글 지원을 판정하는 화면은 아닙니다.** 미리보기에서 한글이 보이지 않는다고 해서 한글이 없는 글꼴이라고 결론 내리지 않습니다.

| 항목 | 설정 | 이유 |
| :--- | :--- | :--- |
| `Character` | `Dynamic` | TMP Font Asset Creator가 원본 글꼴 데이터를 읽어 SDF Atlas를 만들 수 있게 합니다. |
| `Include Font Data` | 체크 | Dynamic TMP Font Asset은 실행 중 Glyph를 추가할 때 원본 글꼴 데이터를 사용합니다. 체크가 꺼져 있으면 Font Asset Creator 또는 실행 중 글자 생성에서 경고가 날 수 있습니다. |
| `Font Size` | 기본값 유지 | TMP의 글자 크기와 Atlas 품질은 Font Asset Creator와 `TextMeshProUGUI`에서 정합니다. 원본 Font Inspector의 크기를 조절할 필요가 없습니다. |
| `Rendering Mode` | 기본값 유지 | TMP는 Font Asset Creator에서 SDF Atlas를 생성하므로 원본 Font Inspector의 렌더링 방식을 실습용으로 바꾸지 않습니다. |

`Character`가 `Dynamic`이면 `Include Font Data`가 표시됩니다. 이 항목이 이미 체크되어 있으면 다른 원본 Import Settings는 건드리지 않고 `Apply`를 누릅니다. `Include Font Data`가 보이지 않거나 체크할 수 없다면 `Character`를 먼저 `Dynamic`으로 바꿉니다.

> 이 설정은 **원본 글꼴 파일**에 적용합니다. 생성 뒤의 `KoreanUI SDF.asset` Font Asset Inspector에서 `Atlas Population Mode`를 `Dynamic` 또는 `Static`으로 정하는 작업과는 별개입니다.

### 4.3 Font Asset Creator에서 생성

`Font Asset Creator`에서 화면에 보이는 항목을 아래처럼 설정합니다.

| Font Asset Creator 항목 | 첫 한글 UI Font Asset 설정 | 이유 |
| :--- | :--- | :--- |
| `Source Font` | 가져온 `Pretendard-Regular` 등 원본 글꼴 | Font Asset의 원본입니다. |
| `Font Face` | 자동 선택된 `Regular` 유지 | 원본의 굵기입니다. Bold가 필요하면 Bold 원본으로 별도 Font Asset을 만듭니다. |
| `Sampling Point Size` | `Auto Sizing` 유지 | 처음에는 Atlas에 맞는 크기를 자동으로 계산하게 둡니다. |
| `Padding` | 기본값 `8 px` 유지 | SDF 글자의 Outline과 Softness에 쓸 여백입니다. 글자가 서로 닿을 때만 늘립니다. |
| `Packing Method` | `Fast` 유지 | 수업용 UI Font Asset에 충분합니다. |
| `Atlas Resolution` | 짧은 테스트는 `512 × 512`부터, 문구가 많으면 `1024 × 1024` | 한글 Glyph가 Atlas에 들어갈 공간입니다. 생성이 실패하거나 빈 공간이 부족하면 해상도를 키웁니다. |
| `Character Set` | `Custom Characters` | 실제 UI에서 쓸 한글·영문·숫자·기호만 Atlas에 넣습니다. |
| `Render Mode` | `SDFAA` 유지 | TMP의 일반적인 SDF 텍스트 표시 방식입니다. |
| `Get Font Features` | 처음에는 해제 | 합자·커닝 같은 고급 OpenType 기능이 필요할 때만 별도로 확인합니다. |

1. Project 창에서 원본 `.ttf` 또는 `.otf` 파일을 선택합니다.
2. 마우스 오른쪽 버튼 메뉴에서 `Create > TextMeshPro > Font Asset > SDF`를 선택합니다. 그러면 `Font Asset Creator` 창이 열립니다.
3. `Source Font`에 선택한 원본이 들어갔는지, `Font Face`가 `Regular`인지 확인합니다.
4. `Character Set`을 `Custom Characters`로 바꿉니다. 새로 나타나는 입력 칸에 먼저 `게임 시작 점수: 100% 일시 정지`를 입력합니다. 이 문자열에 포함된 공백, 숫자, `:`, `%`도 함께 Atlas에 들어갑니다.
5. `Generate Font Atlas`를 누릅니다. 아래 미리보기 영역에 글자가 생성되고 경고가 없으면 `Save as...`를 눌러 `Assets/Fonts/KoreanUI_Static SDF.asset`처럼 저장합니다.
6. `Atlas is full` 또는 Glyph 누락 경고가 나오면 넣을 문자를 줄이거나 `Atlas Resolution`을 `1024 × 1024`로 키운 뒤 다시 생성합니다.

이 절차로 만든 Font Asset은 메뉴·HUD처럼 문구가 미리 정해진 UI에 적합한 **Static Font Asset**입니다. 한글 전체를 무조건 넣지 않고 실제 문구부터 넣는 이유는 Atlas 크기와 메모리 사용량을 통제하기 위해서입니다.

### 4.4 Dynamic Font Asset이 필요한 경우

채팅, 이름 입력, 서버에서 내려오는 문구처럼 실행 전에는 어떤 한글이 나올지 알 수 없는 UI에는 Dynamic Font Asset을 사용합니다. 이는 위 `Font Asset Creator` 창의 설정이 아니라 다음 흐름으로 만듭니다.

1. Project 창에서 원본 `.ttf` 또는 `.otf` 파일을 선택하고, 마우스 오른쪽 버튼 메뉴에서 `Create > TextMeshPro > Font Asset > SDF`를 선택해 별도의 Font Asset을 만듭니다.
2. 생성된 Font Asset을 선택합니다. Inspector의 `Generation Settings > Atlas Population Mode`가 `Dynamic`인지 확인합니다.
3. `Source Font File`에 원본 글꼴 파일이 연결되어 있는지 확인하고, 필요하면 `Atlas Width`, `Atlas Height`, `Multi Atlas Textures`를 설정합니다.
4. Canvas의 `TextMeshProUGUI`에 이 Font Asset을 지정하고 실제 문구를 입력해 Glyph가 추가되는지 확인합니다.

Dynamic Font Asset은 실행 중 Glyph를 Atlas에 추가하는 대신 원본 글꼴 파일이 빌드에 포함됩니다. 정해진 메뉴 문구에는 Static, 입력·변동 문구에는 Dynamic을 사용합니다.

### 4.5 한글 Glyph가 실제로 있는지 확인

한글 지원 여부는 다음 두 단계로 확인합니다.

1. 생성한 `KoreanUI SDF.asset`을 선택합니다. Inspector의 `Character Table`에서 `Character Search`에 `가`, `힣`, `점`, `시`처럼 실제 UI에 쓸 문자를 입력합니다.
2. Canvas의 `TextMeshProUGUI`에 이 Font Asset을 지정하고 `게임 시작\n점수: 100%\n일시 정지`를 입력한 뒤 Game 뷰에서 네모 없이 보이는지 확인합니다.

`Static` Font Asset은 Atlas 생성 시 넣은 문자만 `Character Table`에 있으므로, 이 표에서 한글을 검색해 포함 여부를 확인할 수 있습니다. `Dynamic` Font Asset은 실제 텍스트에서 사용한 Glyph가 Atlas와 Character Table에 추가됩니다. 따라서 Dynamic 설정에서는 **TextMeshProUGUI에 실제 문구를 입력해 보는 확인 단계가 필수**입니다.

### 4.6 모든 한글을 한 번에 넣으려 하지 않기

완성형 한글 음절은 매우 많습니다. 전체 범위를 한 장의 Atlas에 모두 넣으면 Atlas가 커지고 메모리와 생성 시간이 늘어납니다. HUD·메뉴에서 정해진 문구만 쓰는 경우에는 `Static` Font Asset을 만들 때 실제 문구의 글자만 넣고, 문구가 자주 바뀌는 학습 단계에서는 `Dynamic`을 사용합니다.

## 5. Canvas 텍스트에 한글 Font Asset 연결

1. Hierarchy에서 한글을 보여 줄 `Text (TMP)` 오브젝트를 선택합니다.
2. Inspector의 `TextMeshProUGUI` 컴포넌트에서 `Font Asset` 슬롯을 찾습니다.
3. 방금 만든 `KoreanUI SDF` Font Asset을 끌어 놓습니다.
4. `Text Input`에 `게임 시작`, `점수: 100`, `일시 정지`처럼 한글·숫자·기호가 섞인 문구를 입력합니다.
5. Game 뷰에서 네모 없이 보이는지, 줄 높이와 글자 굵기가 의도한 UI와 맞는지 확인합니다.

영문과 한글의 기준선·두께가 어색하면 Font Asset의 Face Info 값을 임의로 바꾸기보다, 먼저 UI에서 사용할 글꼴 조합을 정하고 실제 화면 크기에서 비교합니다. 한글 UI 전체를 같은 글꼴로 표현해야 한다면 한글 Font Asset을 해당 텍스트의 기본 `Font Asset`으로 지정하는 편이 예측하기 쉽습니다.

## 6. 기존 영문 글꼴에 한글을 보조 글꼴로 연결하기

영문 전용 Font Asset을 기본으로 유지하면서, 없는 한글만 다른 글꼴에서 찾게 할 수 있습니다. 이것이 **Fallback Font Asset**입니다.

### 특정 기본 글꼴에만 적용

1. 기본으로 사용하는 Font Asset을 Project 창에서 선택합니다.
2. Inspector의 `Fallback Font Asset Table`에 한글 Font Asset을 추가합니다.
3. 그 Font Asset을 쓰는 텍스트에 한글·영문·숫자를 함께 입력해 확인합니다.

### 프로젝트의 TMP 텍스트에 공통 적용

여러 Font Asset에서 같은 한글 보조 글꼴을 사용해야 하면 `Edit > Project Settings > TextMeshPro`의 `Fallback Font Assets` 목록에 한글 Font Asset을 추가합니다. 이 방법은 프로젝트 전체에 영향을 주므로, 특정 화면만 다른 글꼴을 써야 한다면 개별 Font Asset의 Fallback Table을 우선 사용합니다.

Fallback은 기본 글꼴에 없는 Glyph를 찾을 때만 사용합니다. 따라서 영문은 기본 글꼴, 한글은 보조 글꼴로 그려질 수 있습니다. 두 글꼴의 높이·굵기·자간이 다르면 한 문장 안에서도 어색해질 수 있으므로, UI가 통일되어야 하는 화면에서는 한글 지원 글꼴 하나를 기본 Font Asset으로 사용하는 방법도 비교합니다.

## 7. 확인표와 문제 해결

| 확인 항목 | 정상 상태 | 문제가 있을 때 |
| :--- | :--- | :--- |
| TMP Essentials | `TextMesh Pro` 기본 리소스가 프로젝트에 있음 | `Window > TextMeshPro > Import TMP Essential Resources`를 다시 실행합니다. |
| 텍스트 타입 | Canvas 오브젝트에 `TextMeshProUGUI`가 있음 | 3D용 `TextMeshPro`를 만든 것은 아닌지 확인합니다. |
| Font Asset | Text 컴포넌트의 `Font Asset`에 한글 Font Asset이 지정됨 | Font Asset을 Text 컴포넌트에 직접 다시 연결합니다. |
| Glyph | Game 뷰에서 한글, 숫자, `:`, `%`, `!`가 보임 | 해당 문자를 포함해 Font Asset을 다시 생성하거나 Dynamic 설정을 확인합니다. |
| Fallback | 기본 글꼴에 없는 한글이 보조 글꼴로 보임 | 기본 Font Asset의 Fallback Table 또는 Project Settings의 Fallback Font Assets를 확인합니다. |
| 배포 전 점검 | 실제 UI 문구로 Play Mode를 확인함 | 언어별 문구, 버튼 상태, 줄바꿈, Atlas 크기를 함께 점검합니다. |

## 오늘의 정리

- TMP Importer가 처음 나타나면 `Import TMP Essentials`를 선택합니다. `Examples & Extras`는 선택 사항입니다.
- 한글이 네모로 보이는 문제는 텍스트 내용이 아니라 Font Asset에 한글 Glyph가 없어서 생깁니다.
- 한글 글꼴 파일에서 TMP Font Asset을 만들고 `TextMeshProUGUI`의 `Font Asset`에 연결합니다.
- 기본 영문 글꼴을 유지해야 할 때만 Fallback Font Asset을 사용하고, 글꼴 조합의 기준선과 굵기를 실제 Game 뷰에서 확인합니다.
