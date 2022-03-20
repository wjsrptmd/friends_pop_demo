# friends_pop_demo

### 목적 : 프렌즈팝 레벨 5를 모방한 연습용 프로젝트
* 프렌즈팝 레벨 5 https://www.youtube.com/watch?v=s0N5e7URge0&list=WL&index=7&t=9s

### Unity Editor 에서 실행 방법
* 파일을 내려 받는다. (Code/MapData/Resources)
* 내려 받은 파일을 Unity Editor 의 `Asset` 밑으로 이동시킨다.
* Unity Editor 에서 빈 오브젝트를 생성한다. `Create Epmpty`
  * 빈 오브젝트의 이름은 임의로 정한다. (예 : `GameService`)
  * 빈 오브제트에 `Code/GameService.cs` 스크립트를 추가한다. `AddComponent`
* 게임을 실행해서 정상 동작 되는지 확인한다.

### 진행 상황
* 3매칭 블록 체크
  * 세로 / 오른쪽 아래 대각선 / 왼쪽 아래 대각선
  * 3개 이상 매칭 되었을 때는 스페셜 블록을 생성 시킨다.
* 빈 타일에 블록을 채우고 블록을 이동 시킨다.
* 사용자가 임의로 블록 두개를 이동 시킬 수 있다.
  * 블록 스위칭 했을 때 3매칭이 되지 않는다면, 다시 롤백한다.
