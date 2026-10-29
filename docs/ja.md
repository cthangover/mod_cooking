# Cooking——Cthangover の台所

Cooking は Cthangover の家の台所です。プレイヤーはレシピのアイテムからレシピを覚え、台所の作業台で調理し、パーティの糧食があと何日もつかを見張ります。Cthangover のメカニクスのひとつで、`home_kitchen` シーンで開き、このゲームのアイテムを使います。RPG Plus が使う汎用作業台の mod「Crafting」とは別物です（比較は下記）。

## レシピを覚える

レシピはアイテムとして手に入ります。かばんの中で選んで **レシピを覚える** を押すと、アイテムは消費され、レシピを覚えた状態になります:

![かばんで "Recipe: ration from wolf meat" を選択したところ — Usable、Recipe、"Teaches how to cook wolf meat into rations that are safe to eat"、アクション action/recipe/wolf_meat_to_ration、動詞 Learn recipe — 隣に狼の肉 ×10 と枝 ×10。](assets/learn-recipe.jpg)

## 台所

台所では、物語が「何をする?」と尋ねてきます。左上の小さなパネルが糧食を数えます:

![18:13 の台所。答えは Leave と Cook、行は "A room equipped as a kitchen"、左上には Rations: 0、Daily consumption: 1、Supplies will last 0 days。](assets/kitchen.jpg)

- **糧食** — かばんの中にある数。
- **1 日の消費** — パーティの人数 × 一人あたり 1 日に食べる糧食（mod の設定、初期値は 1）。
- **食料のもつ日数** — 糧食が何日もつかを切り捨てた日数。

**料理する** を押すと、台所の絵の上に作業台が開きます。左側には覚えた調理レシピ — 材料とその数、名前、時間 — が並び、選んだレシピの説明が右側に出ます:

!["Food preparation" の作業台: 左にレシピ "Ration from wolf meat" と材料（狼の肉 1、枝 2）、"15 minutes"。右に名前と説明、下に Cook と Close のボタン。](assets/workbench.jpg)

レシピをクリックして **料理する**。かばんに材料があれば、材料を消費し、完成品を加え、時計をレシピの時間だけ進め、調理の音が鳴ります。今すぐ作れるレシピが一覧の先頭に並びます。

狼の肉 1 と枝 2 から糧食 1 を作ったあと:

![調理後のかばん: 糧食 1、狼の肉 ×10、枝 ×10。](assets/bag-after.jpg)

## データ

レシピ:

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

そしてそれを覚えさせるアイテム:

```json
{ "Id": "recipe/wolf_meat_to_ration", "ItemType": "Recipe,Used", "ItemAction": "action/recipe/wolf_meat_to_ration" }
```

作業台に表示されるのは、`Cooking` 型で覚えたレシピだけです。このレシピを覚えさせるアイテムアクションは mod 内のコードです（レシピごとに 1 クラス）。

台所のシナリオ:

```scenario
scene: home_kitchen
priority: 5
---
select "台所として整えられた部屋" key=rooms/kitchen/prompt
option "出る" key=rooms/kitchen/opt_exit -> :exit
option "料理する" key=rooms/kitchen/opt_cook -> :cook

:cook
action toggle_cooking_workbench
end
```

## Cooking と Crafting

- **Cooking** はプレイヤーが*覚えた* `Cooking` 型のレシピを、全画面の台所用作業台に表示します。開くのは `toggle_cooking_workbench` です。Cthangover 専用で、糧食を数えます。
- **Crafting** は*ステーション*が列挙するレシピを、覚えずにそのまま表示します。どのゲームでも `craft.open station=…` でステーションを開けます。材料と完成品は、何かを消費する前に検査されます。

どちらも材料をかばんから取り、完成品を渡し、レシピの `Time` だけ時計を進めます。

## うまくいかないとき

1. **作業台が空のまま。** まだ調理レシピをひとつも覚えていません — まずかばんのレシピアイテムを使ってください。
2. **料理するが何もしない。** レシピが選ばれていないか、材料が足りません。
3. **糧食を食べられない。** 糧食にはアイテムアクションがありません。パネルは備蓄が何日もつかを数えるだけで、それを自動で食べる仕組みはまだありません。
