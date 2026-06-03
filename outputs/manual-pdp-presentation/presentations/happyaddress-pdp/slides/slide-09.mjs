import { C, base, footer } from "./common.mjs";

export async function slide09(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Тестирование");
  const rows = [
    ["Регистрация", "проверка возраста, телефона, email и подтверждения"],
    ["Авторизация", "верный/неверный пароль, запрет входа без подтверждения"],
    ["Объявления", "обязательные поля, цена, координаты, тип объекта"],
    ["Фотографии", "JPG/PNG, размер до 10 МБ, удаление файлов"],
    ["Фильтрация", "город, цена, тип сделки, характеристики недвижимости"],
    ["Чат", "запрет пустого сообщения и сообщения самому себе"],
  ];
  rows.forEach((row, i) => {
    const y = 205 + i * 66;
    ctx.addShape(slide, { x: 110, y, width: 1060, height: 48, fill: i % 2 ? C.white : "#EEF2FF", line: { style: "solid", fill: C.line, width: 1 }, geometry: "roundRect" });
    ctx.addText(slide, { text: row[0], x: 140, y: y + 11, width: 230, height: 24, fontSize: 19, bold: true, color: C.ink, typeface: "Arial" });
    ctx.addText(slide, { text: row[1], x: 390, y: y + 11, width: 620, height: 24, fontSize: 18, color: C.muted, typeface: "Arial" });
    ctx.addText(slide, { text: "пройдено", x: 1030, y: y + 11, width: 100, height: 24, fontSize: 17, bold: true, color: C.green, align: "right", typeface: "Arial" });
  });
  footer(slide, ctx, 9);
  return slide;
}
