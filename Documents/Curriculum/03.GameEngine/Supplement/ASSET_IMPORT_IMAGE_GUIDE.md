# 보충: 이미지 에셋 임포트 가이드

이 문서의 목표는 이미지 파일을 "**게임 화면에 붙이는 종이와 스티커**"처럼 이해하고, 파일 포맷의 차이, 사용 용도, Unity Import Settings에서 자주 조정하는 옵션을 정리하는 것입니다.

이미지는 단순히 예쁜 그림 파일이 아닙니다. UI 아이콘, 캐릭터 스프라이트, 머티리얼 텍스처, 노멀맵, 파티클 모양처럼 용도에 따라 Unity가 읽는 방식이 달라집니다. 같은 그림이라도 설정을 잘못하면 흐릿해지거나, 용량이 커지거나, 색이 이상하게 보일 수 있습니다.

## 1. 핵심 개념: "그림 파일은 용도에 맞게 포장해야 한다"

요리 재료를 냉장, 냉동, 실온 보관으로 나누듯이 이미지도 용도에 따라 포맷과 임포트 설정을 나눕니다.

UI 아이콘은 선명한 테두리와 투명 배경이 중요합니다. 배경 원화는 큰 화면에 맞는 해상도가 중요합니다. 노멀맵은 사람 눈에 보이는 색 그림이 아니라 표면 방향 정보이므로 일반 이미지처럼 다루면 안 됩니다.

### 이 단어는 무슨 뜻인가요?

- **Texture**: Unity에서 이미지 파일을 읽어 만든 그래픽 데이터입니다.
- **Sprite**: 2D 오브젝트나 UI Image에 쓰기 좋게 설정한 이미지입니다.
- **Alpha**: 투명도 정보입니다. 아이콘, 캐릭터, 파티클 가장자리에 자주 쓰입니다.
- **Compression**: 이미지 용량을 줄이기 위해 데이터를 압축하는 방식입니다.
- **Max Size**: Unity가 사용할 이미지의 최대 해상도입니다.
- **Mipmap**: 멀리 있는 3D 물체의 텍스처를 더 안정적으로 보이게 하는 축소본 묶음입니다.
- **Normal Map**: 색 그림이 아니라 표면의 작은 굴곡 방향을 저장한 특수 텍스처입니다.

## 2. 자주 쓰는 이미지 파일 포맷

| 포맷 | 특징 | 주 사용 용도 | 주의할 점 |
| :--- | :--- | :--- | :--- |
| `PNG` | 투명도 지원, 비교적 선명함 | UI 아이콘, 2D Sprite, 투명 배경 이미지 | 사진처럼 큰 이미지는 용량이 커질 수 있음 |
| `JPG` 또는 `JPEG` | 용량이 작고 사진에 강함 | 배경 이미지, 컨셉 이미지, 큰 사진 텍스처 | 투명도 없음, 반복 저장하면 품질 손상 |
| `PSD` | 레이어를 가진 작업 원본 | 아트 작업 원본, UI 시안 | 프로젝트 용량이 커질 수 있어 최종 배포용은 주의 |
| `TGA` | 알파 포함 가능, 게임 개발에서 오래 쓰임 | 텍스처, 노멀맵, 마스크 이미지 | 파일 크기가 클 수 있음 |
| `EXR` 또는 `HDR` | 높은 밝기 범위 저장 | HDRI 환경 조명, 고급 라이팅 자료 | 입문 수업에서는 필요한 경우에만 사용 |

수업 프로젝트에서는 보통 `PNG`와 `JPG`만 알아도 충분합니다. 투명 배경이 필요하면 `PNG`, 사진처럼 넓은 배경이면 `JPG`를 먼저 생각합니다.

## 3. 용도별 추천 포맷과 설정 방향

| 용도 | 추천 포맷 | Unity 설정 방향 |
| :--- | :--- | :--- |
| UI 아이콘 | `PNG` | `Texture Type`을 `Sprite (2D and UI)`로 설정 |
| 버튼 배경 | `PNG` | 투명도가 필요하면 Alpha 유지, UI 크기에 맞게 Max Size 조정 |
| 2D 캐릭터 | `PNG` | `Sprite Mode`를 Single 또는 Multiple로 선택 |
| 3D 머티리얼 Base Map | `PNG`, `JPG`, `TGA` | `Texture Type`은 Default, sRGB 켬 |
| Normal Map | `PNG`, `TGA` | `Texture Type`을 Normal map으로 설정 |
| 파티클 모양 | `PNG` | Alpha 포함, 가장자리 번짐 확인 |
| 큰 배경 이미지 | `JPG`, `PNG` | Max Size와 Compression으로 용량 관리 |

## 4. Unity Import Settings에서 자주 보는 옵션

이미지 파일을 Project 창에서 선택하면 Inspector에 Import Settings가 표시됩니다. 처음에는 아래 항목만 차근차근 확인해도 대부분의 문제를 줄일 수 있습니다.

| 옵션 | 의미 | 실무에서 보는 기준 |
| :--- | :--- | :--- |
| `Texture Type` | 이미지를 어떤 용도로 읽을지 정함 | UI/2D는 Sprite, 3D 표면은 Default, 노멀맵은 Normal map |
| `Sprite Mode` | Sprite를 하나로 쓸지 여러 조각으로 나눌지 정함 | 아이콘 1개는 Single, 스프라이트 시트는 Multiple |
| `sRGB (Color Texture)` | 색상 이미지로 처리할지 정함 | Base Color와 UI는 켜고, 마스크/데이터 텍스처는 끄는 경우가 있음 |
| `Alpha Source` | 투명도를 어디서 가져올지 정함 | 투명 PNG는 Input Texture Alpha 사용 |
| `Max Size` | Unity가 사용할 최대 해상도 | 실제 화면 크기보다 지나치게 크지 않게 조정 |
| `Compression` | 압축 품질과 용량의 균형 | UI는 품질 우선, 배경/텍스처는 용량과 품질 균형 |
| `Generate Mip Maps` | 멀리 있는 3D 텍스처용 축소본 생성 | 3D 표면 텍스처는 켜고, UI/Sprite는 보통 끔 |
| `Filter Mode` | 확대/축소 시 보간 방식 | 픽셀아트는 Point, 일반 이미지는 Bilinear |
| `Wrap Mode` | UV 밖으로 나갔을 때 반복할지 정함 | 타일 텍스처는 Repeat, UI는 Clamp |

설정을 바꾼 뒤에는 Inspector 아래쪽의 `Apply`를 눌러야 변경이 적용됩니다. `Apply`를 잊으면 눈으로 본 설정과 실제 사용 설정이 달라질 수 있습니다.

## 5. 이미지 종류별 설정 예시

### UI 아이콘

```text
Texture Type: Sprite (2D and UI)
Sprite Mode: Single
Alpha Source: Input Texture Alpha
Generate Mip Maps: Off
Compression: None 또는 Low Quality가 아닌 선명한 설정
Max Size: 실제 UI 크기에 맞게 256, 512 등으로 조정
```

UI 아이콘은 작아 보여도 가장자리가 흐릿하면 품질이 낮아 보입니다. 너무 큰 원본을 그대로 쓰기보다 실제 표시 크기보다 조금 큰 정도로 맞춥니다.

### 2D 스프라이트 시트

```text
Texture Type: Sprite (2D and UI)
Sprite Mode: Multiple
Filter Mode: Point 또는 Bilinear
Compression: 품질 확인 후 선택
```

여러 캐릭터 프레임이 한 이미지에 들어 있다면 `Sprite Mode`를 `Multiple`로 두고 Sprite Editor에서 잘라 씁니다. 픽셀아트라면 `Filter Mode`를 `Point`로 두어 흐릿해지는 것을 막습니다.

### 3D 머티리얼 Base Map

```text
Texture Type: Default
sRGB (Color Texture): On
Generate Mip Maps: On
Wrap Mode: Repeat 또는 Clamp
Compression: Normal Quality 중심으로 확인
```

3D 물체 표면에 붙는 색 텍스처는 거리에 따라 축소되어 보이므로 Mipmap이 도움이 됩니다. 바닥, 벽처럼 반복되는 텍스처는 `Wrap Mode`를 `Repeat`로 두는 경우가 많습니다.

### Normal Map

```text
Texture Type: Normal map
sRGB (Color Texture): Off 또는 Unity가 Normal map에 맞게 처리
Generate Mip Maps: On
```

노멀맵은 눈으로 보는 색 그림이 아닙니다. `Texture Type`을 Normal map으로 설정하지 않으면 표면 굴곡이 의도대로 나오지 않을 수 있습니다.

## 6. 실무에서 자주 생기는 문제

### 1. UI가 흐릿하게 보임

원인은 여러 가지입니다.

- 원본 이미지 해상도가 너무 낮음
- `Max Size`가 작게 줄어 있음
- 압축 품질이 낮음
- Canvas에서 너무 크게 확대해서 사용함

UI는 플레이어가 오래 보는 요소라서 선명도가 중요합니다. 아이콘이 흐릿하면 먼저 원본 크기와 Import Settings의 `Max Size`, `Compression`을 확인합니다.

### 2. 투명 배경이 검은색이나 흰색으로 보임

이미지에 Alpha가 없거나, Alpha 설정이 잘못되었거나, 사용하는 머티리얼/셰이더가 투명을 지원하지 않을 수 있습니다. UI Image에 쓰는 PNG라면 먼저 원본 PNG가 투명도를 실제로 가지고 있는지 확인합니다.

### 3. 3D 텍스처가 멀리서 반짝거리거나 지저분함

3D 표면 텍스처에서 Mipmap이 꺼져 있으면 멀리 있는 물체가 지저분하게 깜빡여 보일 수 있습니다. UI에는 Mipmap이 필요 없는 경우가 많지만, 3D 텍스처에는 보통 켜는 편이 안정적입니다.

### 4. 프로젝트 용량이 갑자기 커짐

큰 원본 이미지, 작업용 PSD, 고해상도 배경을 많이 넣으면 프로젝트가 무거워집니다. 수업 프로젝트에서는 최종 사용 이미지와 작업 원본을 구분하고, 사용하지 않는 큰 이미지는 정리합니다.

## 7. 추천 폴더 구조

```text
Assets/
  _Project/
    Art/
      UI/
      Sprites/
      Textures/
      Particles/
    Materials/
    Prefabs/
```

UI 아이콘과 3D 텍스처를 같은 폴더에 섞어 두면 설정을 찾기 어려워집니다. 폴더를 나누면 나중에 어떤 파일이 어디에 쓰이는지 빠르게 파악할 수 있습니다.

## 8. 임포트 체크리스트

| 질문 | 확인할 것 |
| :--- | :--- |
| UI나 2D에 쓰나요? | `Texture Type`이 Sprite인지 확인 |
| 3D 표면에 쓰나요? | Default, Mipmap, Wrap Mode 확인 |
| 투명 배경이 필요한가요? | 원본 Alpha와 Alpha Source 확인 |
| 픽셀아트인가요? | `Filter Mode`를 Point로 확인 |
| 너무 흐릿하거나 깨지나요? | Max Size, Compression, 실제 표시 크기 확인 |
| 용량이 너무 큰가요? | 해상도와 압축 설정, 사용하지 않는 원본 정리 |

## 생각해보기

1. UI 아이콘에 `JPG`보다 `PNG`가 자주 쓰이는 이유는 무엇일까요?
2. 3D 바닥 텍스처에는 Mipmap이 도움이 되지만 UI 아이콘에는 보통 필요 없는 이유는 무엇일까요?
3. 픽셀아트 이미지를 Bilinear로 확대하면 어떤 문제가 생길까요?

## 오늘의 정리

- 이미지 파일은 용도에 따라 포맷과 Import Settings를 다르게 잡아야 합니다.
- 투명 배경이 필요하면 보통 `PNG`를 사용하고, 큰 사진형 배경은 `JPG`를 사용할 수 있습니다.
- UI와 2D 이미지는 `Sprite (2D and UI)`, 3D 표면 텍스처는 `Default`, 노멀맵은 `Normal map`으로 설정합니다.
- `Max Size`, `Compression`, `Mip Maps`, `Filter Mode`, `Wrap Mode`는 품질과 성능에 직접 영향을 줍니다.
- 실무에서는 원본 이미지, 사용 이미지, 폴더 구조, 압축 설정을 함께 관리합니다.
