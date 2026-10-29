# Cooking — the kitchen of Cthangover

Cooking is the kitchen of Cthangover's house: the player learns recipes from recipe items, cooks at the kitchen workbench, and watches how many days the party's rations will last. It is a mechanic of the Cthangover game — it opens in the `home_kitchen` scene and uses that game's items — and it is a different thing from Crafting, the general workbench mod used by RPG Plus (see the comparison below).

## Learning a recipe

Recipes come as items. Select one in the bag and press **Learn recipe**; the item is used up and the recipe becomes known:

![The inventory with "Recipe: ration from wolf meat" selected — Usable, Recipe, "Teaches how to cook wolf meat into rations that are safe to eat", action action/recipe/wolf_meat_to_ration, verb Learn recipe — next to wolf meat ×10 and branches ×10.](assets/learn-recipe.jpg)

## The kitchen

In the kitchen the story asks what to do. The small panel at the top left counts the rations:

![The kitchen at 18:13 with the answers Leave and Cook, the line "A room equipped as a kitchen", and at the top left: Rations: 0, Daily consumption: 1, Supplies will last 0 days.](assets/kitchen.jpg)

- **Rations** — how many are in the bag.
- **Daily consumption** — characters in the roster × the rations each eats a day (a setting of the mod, 1 by default).
- **Supplies will last** — whole days the rations are enough for.

**Cook** opens the workbench over the kitchen picture. Known cooking recipes are on the left — the ingredients with their counts, the name and the time — and the selected one is described on the right:

![The Food preparation workbench: on the left the recipe "Ration from wolf meat" with its ingredients (wolf meat 1, branches 2) and "15 minutes"; on the right its name and description, and the Cook and Close buttons at the bottom.](assets/workbench.jpg)

Click a recipe, then **Cook**. If the bag holds the ingredients, they are taken, the products are added, the clock moves on by the recipe's time, and a cooking sound plays. Recipes that can be cooked now are listed first.

After cooking one ration from one wolf meat and two branches:

![The bag after cooking: a Ration, wolf meat ×10 and branches ×10.](assets/bag-after.jpg)

## The data

The recipe:

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

and the item that teaches it:

```json
{ "Id": "recipe/wolf_meat_to_ration", "ItemType": "Recipe,Used", "ItemAction": "action/recipe/wolf_meat_to_ration" }
```

The workbench shows only learned recipes of type `Cooking`. The item action that teaches this recipe is code in the mod (one class per recipe).

The kitchen scenario:

```scenario
scene: home_kitchen
priority: 5
---
select "A room equipped as a kitchen" key=rooms/kitchen/prompt
option "Leave" key=rooms/kitchen/opt_exit -> :exit
option "Cook" key=rooms/kitchen/opt_cook -> :cook

:cook
action toggle_cooking_workbench
end
```

## Cooking and Crafting

- **Cooking** shows the recipes the player has *learned*, of type `Cooking`, in a full-screen kitchen workbench; it opens with `toggle_cooking_workbench`; it belongs to Cthangover and counts rations.
- **Crafting** shows the recipes a *station* lists, without learning; any game opens any station with `craft.open station=…`; ingredients and products are checked before anything is taken.

Both take ingredients from the bag, give products and move the clock by the recipe's `Time`.

## If something goes wrong

1. **The workbench is empty.** No cooking recipe has been learned yet — use a recipe item from the bag first.
2. **Cook does nothing.** No recipe is selected, or an ingredient is missing.
3. **Rations cannot be eaten.** The ration has no item action; the panel only counts how long the supplies would last, and nothing eats them by itself yet.
