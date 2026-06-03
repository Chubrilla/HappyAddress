import { C, base, footer, bullet } from "./common.mjs";

export async function slide03(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Цель и задачи практики");
  ctx.addShape(slide, { x: 70, y: 198, width: 500, height: 360, fill: C.blue2, line: { style: "solid", fill: C.blue, width: 2 }, geometry: "roundRect" });
  ctx.addText(slide, { text: "Цель", x: 100, y: 230, width: 220, height: 42, fontSize: 28, bold: true, color: C.blue, typeface: "Arial" });
  ctx.addText(slide, {
    text: "Разработать веб-приложение для размещения, поиска, обработки и сопровождения объявлений о недвижимости.",
    x: 100,
    y: 292,
    width: 420,
    height: 170,
    fontSize: 28,
    bold: true,
    color: C.ink,
    typeface: "Arial",
    autoFit: "shrinkText",
  });
  bullet(slide, ctx, 650, 210, "изучить предметную область и требования к сервису недвижимости");
  bullet(slide, ctx, 650, 285, "спроектировать структуру ASP.NET Core MVC-приложения и базы данных");
  bullet(slide, ctx, 650, 360, "реализовать регистрацию, объявления, фильтры, карту, избранное и чат");
  bullet(slide, ctx, 650, 435, "добавить модерацию объявлений и изображений");
  bullet(slide, ctx, 650, 510, "проверить основные пользовательские сценарии");
  footer(slide, ctx, 3);
  return slide;
}
