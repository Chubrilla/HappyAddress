import { C, base, footer, card } from "./common.mjs";

export async function slide08(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Валидация, безопасность и модерация");
  card(slide, ctx, 80, 210, 330, 250, "Валидация", "обязательные поля, положительная цена, координаты, допустимые типы сделок, проверка этажа", C.blue, C.blue2);
  card(slide, ctx, 475, 210, 330, 250, "Безопасность", "BCrypt-хеширование паролей, подтверждение email, сессии и проверка владельца объявления", C.green, C.green2);
  card(slide, ctx, 870, 210, 330, 250, "Модерация", "статусы «На модерации», «Опубликовано», «Отклонено», причина отклонения и проверка фото", C.red, C.red2);
  ctx.addText(slide, {
    text: "Цель этих механизмов - не допустить некорректные данные в каталог и защитить действия пользователя от случайного или чужого изменения.",
    x: 130,
    y: 540,
    width: 1020,
    height: 58,
    fontSize: 24,
    bold: true,
    color: C.ink,
    align: "center",
    typeface: "Arial",
  });
  footer(slide, ctx, 8);
  return slide;
}
