# 광산 미니게임 시스템 전달 문서

## 개요

마우스 주변 광석을 자동 채굴하는 미니게임입니다.

기능 구성:

* 시작 화면(StartUI)
* 세션 타이머(TimerUI)
* 경험치 및 레벨(XPBarUI)
* 광석 보유량 표시(OreCounterGroup)
* 채굴 종료 연출(ResultUI)
* 세션 결과 정산(ResultUI)
* 광석 통계 패널(StatPanelUI)

---

# 프리팹 목록

## 1. StartUI

기능:

* 게임 시작 연출
* 시작 버튼 제공
* 게임 시작 시 GameFlowController 호출

필수 연결:

* GameFlowController

주의:

* StartUI가 비활성화되면 안됨
* Start() 시 자동 재생

---

## 2. TimerUI

기능:

* 남은 시간 표시

필수 연결:

* TimerSystem
* TextMeshProUGUI

Provider Object:

* TimerSystem

필수 인터페이스:

```csharp
ITimerProvider
```

현재 표시 형식:

```text
29.98
29.97
29.96
```

---

## 3. XPBarUI

기능:

* 경험치 게이지 표시
* 현재 경험치 표시

표시 형식:

```text
45 / 100
88 / 100
120 / 150
```

Provider Object:

* XPSystem

필수 인터페이스:

```csharp
IXPProvider
```

---

## 4. OreCounterGroup

기능:

* 현재 보유 광석 수량 표시

예시:

```text
Copper x10
Iron x4
Gold x1
```

Provider Object:

* RuntimeOreInventoryProvider

필수:

* OreManager

사용 방식:

OreManager 값 변경 시 자동 갱신

---

## 5. ResultUI

기능:

* MINING END 연출
* 세션 결과 표시
* Restart 버튼
* Stat 버튼

표시 내용:

```text
Copper
Session : 3
Total : 15

Iron
Session : 2
Total : 20
```

Provider Object:

* RuntimeResultProvider

필수 인터페이스:

```csharp
IResultProvider
```

---

## 6. StatPanelUI

기능:

* 광석 통계 패널

버튼:

```text
ResultUI
  └ StatButton
```

로 열고 닫음

---

## 7. GameLoopUI

기능:

게임 중 사용하는 UI를 묶는 루트

포함:

```text
GameLoopUI
 ├ XPBarUI
 ├ TimerUI
 ├ OreCounterGroup
 └ StatPanelUI
```

주의:

ResultUI는 분리 프리팹

---

# 시스템 목록

## GameFlowController

게임 전체 흐름 관리

순서:

```text
StartUI
↓
게임 시작
↓
타이머 진행
↓
채굴
↓
시간 종료
↓
ResultUI
↓
Restart
```

---

## TimerSystem

기능:

* 세션 시간 관리
* 종료 시 EndGame 호출

필수 연결:

```csharp
[SerializeField]
private GameFlowController flow;
```

---

## XPSystem

기능:

* 세션 XP
* 총 XP
* 레벨 관리

예시:

```text
0 / 100

100 달성

↓

레벨업

↓

0 / 150
```

---

## RewardSystem

기능:

결과창 전용 데이터

저장:

```text
세션 획득량
총 획득량
```

사용 예:

```csharp
RewardSystem.Instance.Add(
    ore.oreType,
    1);
```

---

## OreManager

기능:

실제 인벤토리 데이터

사용 예:

```csharp
oreManager.AddRange(...)
```

주의:

RewardSystem과 역할이 다름

RewardSystem:
결과창용

OreManager:
실제 보유 광석용

---

## OreResetSystem

기능:

Restart 시 모든 광석 복원

사용:

```csharp
oreResetSystem.ResetAllOres();
```

---

# Ore 구조

## Ore

광석 데이터

보유 정보:

```csharp
OreType oreType
int xpValue
```

기능:

```csharp
SetMined()
ResetOre()
```

---

## OreView

광석 연출

기능:

```csharp
PlayCollect()
PlayDisappear()
ResetView()
```

---

# Restart 시 실행 순서

```text
Restart 버튼
↓
GameFlowController.RestartGame()
↓
OreResetSystem.ResetAllOres()
↓
RewardSystem.ClearSession()
↓
XPSystem.ResetSession()
↓
ResultUI 비활성화
↓
Timer 재시작
↓
Mining 재시작
```

---

# MiningSystem 사용 방식

채굴 흐름

```text
마우스 위치
↓
반경 검색
↓
Ore 발견
↓
RewardSystem 추가
↓
OreManager 추가
↓
XP 추가
↓
연출 재생
↓
광석 비활성화
```

---

# 현재 의존 관계

```text
GameFlowController
 ├ StartUI
 ├ TimerSystem
 ├ MiningSystem
 ├ ResultUI
 └ OreResetSystem

TimerUI
 └ TimerSystem

XPBarUI
 └ XPSystem

OreCounterGroup
 └ OreManager

ResultUI
 └ RuntimeResultProvider
```

---

# 주의사항

1.

RewardSystem과 OreManager는 역할이 다름

삭제하면 안됨

2.

ResultUI Provider Object는 RuntimeResultProvider 연결

3.

TimerUI Provider Object는 TimerSystem 연결

4.

XPBarUI Provider Object는 XPSystem 연결

5.

OreCounterGroup Provider Object는 RuntimeOreInventoryProvider 연결

6.

Restart 기능은 OreResetSystem 필수

7.

광석 추가 시 Ore 컴포넌트만 붙어 있으면 자동 복원 가능

8.

TestOreManager는 현재 사용하지 않음

삭제 가능

9.

ResultUI가 안 뜨면 먼저 Provider Object 연결 확인

10.

NullReferenceException 발생 시 대부분 Inspector 참조 누락 문제
