import { C, base, footer, card } from "./common.mjs";

export async function slide10(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Итоги и перспективы");
  card(slide, ctx, 80, 215, 500, 260, "Итог практики", "Создана промежуточная версия веб-приложения HappyAddress: каталог недвижимости, личный кабинет, объявления, фото, карта, избранное, модерация и чат.", C.green, C.green2);
  card(slide, ctx, 700, 215, 500, 260, "Дальнейшее развитие", "Рекомендации объектов по предпочтениям, уведомления, аналитика просмотров, мобильная адаптация и интеграция с внешними площадками.", C.blue, C.blue2);
  ctx.addShape(slide, { x: 180, y: 545, width: 920, height: 76, fill: C.ink, geometry: "roundRect" });
  ctx.addText(slide, {
    text: "Проект решает основные задачи преддипломной практики и может быть использован как основа дипломной работы.",
    x: 220,
    y: 566,
    width: 840,
    height: 36,
    fontSize: 25,
    bold: true,
    color: C.white,
    align: "center",
    typeface: "Arial",
  });
  footer(slide, ctx, 10);
  return slide;
}
