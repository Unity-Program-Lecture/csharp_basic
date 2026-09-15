# 게임 데이터베이스 프로그래밍 환경 준비

DAY01 전에 .NET SDK, DB Browser for SQLite, Visual Studio 또는 터미널을 준비합니다. DAY11은 별도 Unity 6 프로젝트와 `com.gilzoide.sqlite-net` 패키지가 필요합니다.

## 준비 확인

1. 터미널에서 `dotnet --version`을 실행해 .NET SDK 버전이 출력되는지 확인합니다.
2. DB Browser for SQLite를 실행하고 빈 DB를 만들 수 있는지 확인합니다.
3. DAY01에 사용할 실습 폴더에 쓰기 권한이 있는지 확인합니다.
4. Unity 6 프로젝트가 필요한 경우 DAY11 전에 Package Manager를 열 수 있는지 확인합니다.

### 완료 확인

- [ ] `dotnet --version`이 오류 없이 출력된다.
- [ ] DB Browser for SQLite에서 `.db` 파일을 열 수 있다.
- [ ] DAY01 실습 폴더에 `GameShop.db` 파일을 저장할 수 있다.
- [ ] Unity 실습 대상자는 Package Manager를 열 수 있다.

## 문제가 생기면

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| `dotnet` 명령을 찾을 수 없음 | .NET SDK 설치와 터미널 재시작 |
| DB Browser 실행 실패 | 설치 권한과 운영체제에 맞는 설치 파일 |
| NuGet 패키지 다운로드 실패 | 인터넷, 학교 방화벽, 프로젝트 폴더 쓰기 권한 |
| Unity 패키지 설치 실패 | Console 컴파일 오류와 Git URL 입력값 |
