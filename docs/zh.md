# Cooking——Cthangover 的厨房

Cooking 就是 Cthangover 里的家用厨房：玩家从配方物品中学习配方，在厨房工作台做饭，并留意队伍的口粮还能撑几天。它是 Cthangover 游戏的一个机制——在 `home_kitchen` 场景中打开，使用该游戏的物品——它与 Crafting（RPG Plus 使用的通用工作台 mod）不是一回事（对比如下）。

## 学习配方

配方以物品的形式出现。在背包中选中一个，按下**学习配方**；物品被消耗，配方随即学会：

![背包中选中了 "Recipe: ration from wolf meat"——Usable、Recipe、"Teaches how to cook wolf meat into rations that are safe to eat"、动作 action/recipe/wolf_meat_to_ration、动词 Learn recipe——旁边是狼肉 ×10 和树枝 ×10。](assets/learn-recipe.jpg)

## 厨房

在厨房里，剧情会问你要做什么。左上角的小面板统计口粮：

![18:13 的厨房：答案为 Leave 和 Cook，一行 "A room equipped as a kitchen"，左上角是 Rations: 0、Daily consumption: 1、Supplies will last 0 days。](assets/kitchen.jpg)

- **口粮**——背包里有多少。
- **每日消耗**——队伍中的人数 × 每人每天吃的口粮数（mod 的设置项，默认为 1）。
- **补给可维持**——口粮够用几个整天。

按下**烹饪**，会在厨房画面上打开工作台。左侧列出已经学会的烹饪配方——原料及数量、名称和时间——右侧是所选配方的说明：

!["Food preparation" 工作台：左侧是配方 "Ration from wolf meat" 及其原料（狼肉 1、树枝 2）和 "15 minutes"；右侧是名称和描述，底部是 Cook 和 Close 按钮。](assets/workbench.jpg)

点选一个配方，再按**烹饪**。如果背包里有原料，就会消耗原料、加入成品、把时钟拨快配方所需的时间，并播放烹饪音效。现在就能做的配方排在列表最前面。

用一份狼肉和两根树枝做出一份口粮之后：

![烹饪后的背包：一份口粮、狼肉 ×10 和树枝 ×10。](assets/bag-after.jpg)

## 数据

配方：

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

以及教会这个配方的物品：

```json
{ "Id": "recipe/wolf_meat_to_ration", "ItemType": "Recipe,Used", "ItemAction": "action/recipe/wolf_meat_to_ration" }
```

工作台只显示已学习的 `Cooking` 类型配方。教会这个配方的物品动作是 mod 里的代码（每个配方一个类）。

厨房场景的剧本：

```scenario
scene: home_kitchen
priority: 5
---
select "一个布置成厨房的房间" key=rooms/kitchen/prompt
option "离开" key=rooms/kitchen/opt_exit -> :exit
option "烹饪" key=rooms/kitchen/opt_cook -> :cook

:cook
action toggle_cooking_workbench
end
```

## Cooking 与 Crafting

- **Cooking** 显示玩家已*学习*的 `Cooking` 类型配方，在厨房工作台中全屏打开；用 `toggle_cooking_workbench` 开启；属于 Cthangover，并统计口粮。
- **Crafting** 显示*站点*列出的配方，无需学习；任何游戏都能用 `craft.open station=…` 打开任何站点；在拿走任何东西之前，会先检查原料和成品。

两者都从背包取原料、产出成品，并按配方的 `Time` 推进时钟。

## 出问题了？

1. **工作台是空的。** 还没有学习任何烹饪配方——先使用背包里的配方物品。
2. **按“烹饪”没有反应。** 没有选中配方，或者缺少原料。
3. **口粮不能吃。** 口粮没有物品动作；面板只是统计补给能维持多久，目前还没有任何东西会自动吃掉它们。
