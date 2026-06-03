import { C, base, footer } from "./common.mjs";

export async function slide06(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Алгоритм работы приложения");
  const box = (x, y, w, h, title, body, color, fill) => {
    ctx.addShape(slide, { x, y, width: w, height: h, fill, line: { style: "solid", fill: color, width: 2 }, geometry: "roundRect" });
    ctx.addText(slide, { text: title, x: x + 14, y: y + 12, width: w - 28, height: 26, fontSize: 18, bold: true, color: C.ink, align: "center", typeface: "Arial" });
    ctx.addText(slide, { text: body, x: x + 14, y: y + 42, width: w - 28, height: h - 52, fontSize: 14, color: C.muted, align: "center", typeface: "Arial", autoFit: "shrinkText" });
  };
  const arr = (x1, y1, x2, y2, color = C.muted) => {
    ctx.addShape(slide, { x: x1, y: y1, width: Math.max(1, x2 - x1), height: 2, fill: color, line: { style: "solid", fill: color, width: 1 } });
    ctx.addShape(slide, { x: x2 - 8, y: y2 - 6, width: 12, height: 12, fill: color, geometry: "triangle" });
  };

  box(74, 228, 180, 92, "1. Старт", "пользователь открывает сайт", C.blue, C.blue2);
  box(288, 228, 180, 92, "2. Авторизация", "сессия, вход или регистрация", C.green, C.green2);
  box(502, 228, 220, 92, "3. Каталог", "опубликованные объявления", C.purple, C.purple2);
  box(772, 205, 190, 78, "Поиск", "фильтры, сортировка, карточка", C.blue, C.white);
  box(772, 305, 190, 78, "Избранное", "сохранение вариантов", "#CA8A04", "#FEFCE8");
  box(1012, 228, 190, 92, "Чат", "сообщения покупателя и продавца", C.purple, C.white);

  box(288, 450, 200, 88, "Создание", "данные объекта, координаты, фотографии", C.green, C.white);
  box(548, 450, 180, 88, "Проверка", "валидация формы и файлов", C.orange, C.orange2);
  box(788, 450, 180, 88, "Модерация", "администратор проверяет", C.red, C.red2);
  box(1028, 410, 174, 70, "Опубликовано", "попадает в каталог", C.green, C.green2);
  box(1028, 504, 174, 82, "Отклонено", "причина для исправления", C.orange, C.orange2);

  arr(254, 274, 288, 274, C.muted);
  arr(468, 274, 502, 274, C.muted);
  arr(722, 274, 772, 244, C.blue);
  arr(722, 274, 772, 344, "#CA8A04");
  arr(962, 244, 1012, 274, C.purple);
  ctx.addShape(slide, { x: 602, y: 320, width: 3, height: 130, fill: C.green });
  ctx.addShape(slide, { x: 488, y: 493, width: 60, height: 3, fill: C.green });
  arr(728, 494, 788, 494, C.orange);
  arr(968, 494, 1028, 445, C.green);
  arr(968, 494, 1028, 545, C.orange);

  ctx.addText(slide, {
    text: "Ключевая логика: пользовательские действия проходят через валидацию и статусы, а публикация объявления становится доступной только после модерации.",
    x: 112,
    y: 618,
    width: 1056,
    height: 34,
    fontSize: 20,
    bold: true,
    color: C.ink,
    align: "center",
    typeface: "Arial",
  });
  footer(slide, ctx, 6);
  return slide;
}
