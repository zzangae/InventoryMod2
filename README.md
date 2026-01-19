# Inventory Mod 2.1 (인벤토리 모드 2.1)

The Forest 게임용 ModAPI 인벤토리 모드

## Description

Inventory Mod 2 provides abilities to add more than one of certain items and has them sorted alphabetically and by category.

인벤토리모드 2.1은 특정 항목을 두 개 이상 추가할 수 있는 능력을 제공하며 알파벳순, 범주별로 정렬합니다.

## Changelog

### 1.0.73.8 fix
#### MaxAmount 버튼
* 0 이상 : Add + MAX
* 0 이하 : Add만

### 1.0.73.8
- GUI 코드 리팩토링
- flintlockAmmo MAX 버튼 추가

### 1.0.73.7
- Open Inventory Mod Menu - 인벤토리 메뉴열기
- Supports mod in SP/MP - 싱글/멀티플레이 지원
- All itemList tab added - 모든 아이템목록 TAB 추가

### 1.0.73.6
- Layout-tab batch - 레이아웃 배치

### 1.73.2
- MAX button added - 최대치 버튼 추가됨

## Binding Key

| 키 | 설명 |
|---|------|
| F4 | Open Menu (메뉴 열기) |

## 설치

1. [ModAPI](https://modapi.survivetheforest.net/)를 설치합니다.
2. `Inventory.cs`와 `ModInfo.xml`을 ModAPI 모드 폴더에 배치합니다.
3. ModAPI에서 모드를 빌드합니다.

## 사용법

- **F4**: 인벤토리 메뉴 열기/닫기

## 기능

| 탭 | 설명 |
|---|------|
| Weapons | 무기류 (도끼, 활, 창 등) |
| Items | 장비류 (손전등, 갑옷, 나침반 등) |
| Res. | 자원류 (나무, 돌, 천 등) |
| Head | 동물 머리 |
| Other | 기타 아이템 |
| All | 전체 아이템 목록 + Give All |

## 버튼

| 버튼 | 설명 |
|------|------|
| Add | 아이템 1개 추가 |
| MAX | 스택 가능 아이템 최대치 추가 |
| Give All | 모든 아이템 지급 |

## 파일 구조

```
InventoryMod2/
├── Inventory.cs
├── ModInfo.xml
└── README.md
```

## 호환성

- The Forest: 1.11b
- 모드 버전: 1.0.73.8

## 라이선스

GPL-3.0 License
