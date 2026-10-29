# Cooking — Cthangover의 부엌

Cooking은 Cthangover 집안의 부엌입니다. 플레이어는 레시피 아이템으로 레시피를 익히고, 부엌 작업대에서 요리하며, 파티의 식량이 며칠을 버틸지 살핍니다. Cthangover 게임의 메카닉으로 `home_kitchen` 장면에서 열리고 그 게임의 아이템을 씁니다. RPG Plus가 쓰는 범용 작업대 mod인 Crafting과는 다른 것이니 혼동하지 마세요(비교는 아래).

## 레시피 익히기

레시피는 아이템으로 옵니다. 가방에서 하나를 고르고 **레시피 배우기**를 누르면 아이템은 소모되고 레시피를 익힌 상태가 됩니다:

![가방에서 "Recipe: ration from wolf meat"를 고른 모습 — Usable, Recipe, "Teaches how to cook wolf meat into rations that are safe to eat", 액션 action/recipe/wolf_meat_to_ration, 동사 Learn recipe — 옆에 늑대 고기 ×10와 나뭇가지 ×10.](assets/learn-recipe.jpg)

## 부엌

부엌에서는 이야기가 무엇을 할지 물어봅니다. 왼쪽 위의 작은 패널이 식량을 셉니다:

![18:13의 부엌. 답은 Leave와 Cook, 줄은 "A room equipped as a kitchen", 왼쪽 위에는 Rations: 0, Daily consumption: 1, Supplies will last 0 days.](assets/kitchen.jpg)

- **식량** — 가방에 들어 있는 수.
- **하루 소비량** — 파티 인원 × 한 사람이 하루에 먹는 식량 수(mod 설정, 기본값 1).
- **보급품 지속일** — 식량이 버틸 수 있는 온전한 일수.

**요리하기**를 누르면 부엌 그림 위에 작업대가 열립니다. 왼쪽에는 익힌 요리 레시피 — 재료와 개수, 이름, 시간 — 이 나열되고, 고른 레시피의 설명이 오른쪽에 표시됩니다:

!["Food preparation" 작업대: 왼쪽에 레시피 "Ration from wolf meat"와 재료(늑대 고기 1, 나뭇가지 2), "15 minutes". 오른쪽에 이름과 설명, 아래에 Cook과 Close 버튼.](assets/workbench.jpg)

레시피를 클릭하고 **요리하기**를 누르세요. 가방에 재료가 있으면 재료를 소모하고 결과물을 더하며, 게임 시계를 레시피 시간만큼 앞으로 돌리고 요리 소리가 납니다. 지금 만들 수 있는 레시피가 목록 앞쪽에 옵니다.

늑대 고기 하나와 나뭇가지 두 개로 식량 하나를 만든 뒤:

![요리 후 가방: 식량 하나, 늑대 고기 ×10, 나뭇가지 ×10.](assets/bag-after.jpg)

## 데이터

레시피:

```json
{
  "Kind": "recipe",
  "Items": [
    {
      "Id": "recipe/wolf_meat_to_ration",
      "Name": "recipe/wolf_meat_to_ration/name",
      "Description": "recipe/wolf_meat_to_ration/desc",
      "Type": "Cooking",
      "Time": 15,
      "Input": [ { "Id": "food/wolf_meat", "Count": 1 }, { "Id": "resource/branches", "Count": 2 } ],
      "Output": [ { "Id": "food/ration", "Count": 1 } ]
    }
  ]
}
```

그리고 그것을 가르쳐 주는 아이템:

```json
{ "Id": "recipe/wolf_meat_to_ration", "ItemType": "Recipe,Used", "ItemAction": "action/recipe/wolf_meat_to_ration" }
```

작업대에는 `Cooking` 유형의 익힌 레시피만 표시됩니다. 이 레시피를 가르치는 아이템 액션은 mod 안의 코드입니다(레시피당 클래스 하나).

부엌 시나리오:

```scenario
scene: home_kitchen
priority: 5
---
select "주방으로 꾸며진 방" key=rooms/kitchen/prompt
option "나가기" key=rooms/kitchen/opt_exit -> :exit
option "요리하기" key=rooms/kitchen/opt_cook -> :cook

:cook
action toggle_cooking_workbench
end
```

## Cooking과 Crafting

- **Cooking**은 플레이어가 *익힌* `Cooking` 유형 레시피를 전체 화면 부엌 작업대에 보여 줍니다. `toggle_cooking_workbench`로 열리고, Cthangover에 속하며 식량을 셉니다.
- **Crafting**은 *작업대*가 나열하는 레시피를 익히지 않고 보여 줍니다. 어떤 게임이든 `craft.open station=…`으로 작업대를 열 수 있습니다. 재료와 결과물은 무언가를 소모하기 전에 검사합니다.

둘 다 가방에서 재료를 가져오고, 결과물을 주고, 레시피의 `Time`만큼 시계를 움직입니다.

## 문제가 생겼을 때

1. **작업대가 비어 있습니다.** 아직 익힌 요리 레시피가 없습니다 — 먼저 가방의 레시피 아이템을 사용하세요.
2. **요리하기가 아무것도 하지 않습니다.** 레시피를 고르지 않았거나 재료가 부족합니다.
3. **식량을 먹을 수 없습니다.** 식량에는 아이템 액션이 없습니다. 패널은 보급품이 얼마나 버틸지 셀 뿐이고, 아직 스스로 식량을 먹는 것은 없습니다.
