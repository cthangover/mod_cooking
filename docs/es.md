# Cooking — la cocina de Cthangover

Cooking es la cocina de la casa de Cthangover: el jugador aprende recetas a partir de objetos-receta, cocina en el banco de la cocina y vigila cuántos días de raciones le quedan al grupo. Es una mecánica del juego Cthangover —se abre en la escena `home_kitchen` y usa los objetos de ese juego—, y no hay que confundirla con Crafting, el mod de bancos de trabajo general que usa RPG Plus (abajo tienes la comparación).

## Aprender una receta

Las recetas llegan como objetos. Selecciona uno en la bolsa y pulsa **Aprender receta**; el objeto se consume y la receta queda aprendida:

![El inventario con "Recipe: ration from wolf meat" seleccionado — Usable, Recipe, "Teaches how to cook wolf meat into rations that are safe to eat", acción action/recipe/wolf_meat_to_ration, verbo Learn recipe — junto a carne de lobo ×10 y ramas ×10.](assets/learn-recipe.jpg)

## La cocina

En la cocina, la historia pregunta qué hacer. El panel pequeño de la esquina superior izquierda lleva la cuenta de las raciones:

![La cocina a las 18:13 con las respuestas Leave y Cook, la línea "A room equipped as a kitchen" y, arriba a la izquierda: Rations: 0, Daily consumption: 1, Supplies will last 0 days.](assets/kitchen.jpg)

- **Raciones** — cuántas hay en la bolsa.
- **Consumo diario** — personajes del grupo × raciones que come cada uno al día (un ajuste del mod, 1 por defecto).
- **Las provisiones durarán** — días completos que alcanzan las raciones.

**Cocinar** abre el banco sobre la imagen de la cocina. A la izquierda aparecen las recetas de cocina conocidas —los ingredientes con sus cantidades, el nombre y el tiempo—, y la receta seleccionada se describe a la derecha:

![El banco "Food preparation": a la izquierda la receta "Ration from wolf meat" con sus ingredientes (carne de lobo 1, ramas 2) y "15 minutes"; a la derecha su nombre y descripción, y abajo los botones Cook y Close.](assets/workbench.jpg)

Haz clic en una receta y pulsa **Cocinar**. Si la bolsa tiene los ingredientes, se gastan, se añaden los productos, el reloj avanza lo que dura la receta y suena un sonido de cocina. Las recetas que se pueden cocinar ahora mismo encabezan la lista.

Tras cocinar una ración con una carne de lobo y dos ramas:

![La bolsa tras cocinar: una ración, carne de lobo ×10 y ramas ×10.](assets/bag-after.jpg)

## Los datos

La receta:

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

y el objeto que la enseña:

```json
{ "Id": "recipe/wolf_meat_to_ration", "ItemType": "Recipe,Used", "ItemAction": "action/recipe/wolf_meat_to_ration" }
```

El banco muestra solo las recetas aprendidas de tipo `Cooking`. La acción del objeto que enseña esta receta es código del mod (una clase por receta).

El escenario de la cocina:

```scenario
scene: home_kitchen
priority: 5
---
select "Una habitación acondicionada como cocina" key=rooms/kitchen/prompt
option "Salir" key=rooms/kitchen/opt_exit -> :exit
option "Cocinar" key=rooms/kitchen/opt_cook -> :cook

:cook
action toggle_cooking_workbench
end
```

## Cooking y Crafting

- **Cooking** muestra las recetas que el jugador ha *aprendido*, de tipo `Cooking`, en un banco de cocina a pantalla completa; se abre con `toggle_cooking_workbench`; pertenece a Cthangover y cuenta raciones.
- **Crafting** muestra las recetas que lista una *estación*, sin aprenderlas; cualquier juego abre cualquier estación con `craft.open station=…`; los ingredientes y los productos se comprueban antes de gastar nada.

Los dos toman ingredientes de la bolsa, entregan productos y avanzan el reloj según el `Time` de la receta.

## Si algo falla

1. **El banco está vacío.** Todavía no se ha aprendido ninguna receta de cocina — usa antes un objeto-receta de la bolsa.
2. **Cocinar no hace nada.** No hay receta seleccionada o falta un ingrediente.
3. **Las raciones no se pueden comer.** La ración no tiene acción de objeto; el panel solo cuenta cuánto aguantarían las provisiones, y de momento nada las come por su cuenta.
