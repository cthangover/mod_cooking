# Cooking — căn bếp của Cthangover

Cooking là căn bếp của ngôi nhà trong Cthangover: người chơi học công thức từ vật phẩm công thức, nấu ăn tại bàn bếp trong bếp và để ý xem khẩu phần của đội còn đủ bao nhiêu ngày. Đây là một cơ chế của trò chơi Cthangover — mở trong cảnh `home_kitchen` và dùng các vật phẩm của trò đó — và khác hẳn với Crafting, mod bàn chế tạo chung mà RPG Plus dùng (so sánh ở dưới).

## Học một công thức

Công thức đến dưới dạng vật phẩm. Chọn một vật phẩm trong túi đồ và bấm **Học công thức**; vật phẩm bị tiêu hao và công thức trở thành đã biết:

![Túi đồ với "Recipe: ration from wolf meat" đang được chọn — Usable, Recipe, "Teaches how to cook wolf meat into rations that are safe to eat", hành động action/recipe/wolf_meat_to_ration, động từ Learn recipe — cạnh thịt sói ×10 và cành cây ×10.](assets/learn-recipe.jpg)

## Căn bếp

Trong bếp, cốt truyện hỏi bạn muốn làm gì. Bảng nhỏ ở góc trên bên trái đếm khẩu phần:

![Căn bếp lúc 18:13 với hai lựa chọn Leave và Cook, dòng "A room equipped as a kitchen", ở trên bên trái: Rations: 0, Daily consumption: 1, Supplies will last 0 days.](assets/kitchen.jpg)

- **Khẩu phần** — bao nhiêu cái trong túi đồ.
- **Tiêu thụ mỗi ngày** — số nhân vật trong đội × khẩu phần mỗi người ăn mỗi ngày (thiết lập của mod, mặc định là 1).
- **Lương thực đủ dùng** — số ngày tròn trọn mà khẩu phần đủ dùng.

**Nấu ăn** mở bàn bếp đè lên hình căn bếp. Bên trái là các công thức nấu ăn đã học — nguyên liệu kèm số lượng, tên và thời gian — còn công thức đang chọn được mô tả bên phải:

![Bàn bếp "Food preparation": bên trái là công thức "Ration from wolf meat" với nguyên liệu (thịt sói 1, cành cây 2) và "15 minutes"; bên phải là tên và mô tả của nó, dưới cùng là hai nút Cook và Close.](assets/workbench.jpg)

Bấm vào một công thức rồi bấm **Nấu ăn**. Nếu túi đồ có đủ nguyên liệu, chúng bị lấy đi, sản phẩm được thêm vào, đồng hồ chạy thêm đúng thời gian của công thức và tiếng nấu ăn vang lên. Những công thức nấu được ngay xếp đầu danh sách.

Sau khi nấu một khẩu phần từ một thịt sói và hai cành cây:

![Túi đồ sau khi nấu: một khẩu phần, thịt sói ×10 và cành cây ×10.](assets/bag-after.jpg)

## Dữ liệu

Công thức:

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

và vật phẩm dạy công thức đó:

```json
{ "Id": "recipe/wolf_meat_to_ration", "ItemType": "Recipe,Used", "ItemAction": "action/recipe/wolf_meat_to_ration" }
```

Bàn bếp chỉ hiển thị những công thức đã học thuộc loại `Cooking`. Hành động vật phẩm dạy công thức này là mã nằm trong mod (mỗi công thức một lớp).

Kịch bản của căn bếp:

```scenario
scene: home_kitchen
priority: 5
---
select "Một căn phòng được cải tạo thành nhà bếp" key=rooms/kitchen/prompt
option "Rời đi" key=rooms/kitchen/opt_exit -> :exit
option "Nấu ăn" key=rooms/kitchen/opt_cook -> :cook

:cook
action toggle_cooking_workbench
end
```

## Cooking và Crafting

- **Cooking** hiển thị những công thức người chơi đã *học*, thuộc loại `Cooking`, trong bàn bếp toàn màn hình; mở bằng `toggle_cooking_workbench`; thuộc về Cthangover và đếm khẩu phần.
- **Crafting** hiển thị các công thức mà một *trạm* liệt kê, không cần học; trò chơi nào cũng có thể mở trạm bằng `craft.open station=…`; nguyên liệu và sản phẩm được kiểm tra trước khi lấy bất cứ thứ gì.

Cả hai đều lấy nguyên liệu từ túi đồ, đưa ra sản phẩm và đẩy đồng hồ thêm `Time` của công thức.

## Khi có gì đó không ổn

1. **Bàn bếp trống trơn.** Chưa học công thức nấu ăn nào — hãy dùng vật phẩm công thức trong túi đồ trước.
2. **Nấu ăn không làm gì cả.** Chưa chọn công thức, hoặc thiếu một nguyên liệu.
3. **Khẩu phần không thể ăn.** Khẩu phần không có hành động vật phẩm; bảng chỉ đếm xem lương thực đủ dùng bao lâu, và chưa có gì tự ăn chúng.
