import { C, footer } from "./common.mjs";

export async function slide01(presentation, ctx) {
  const slide = presentation.slides.add();
  ctx.addShape(slide, { x: 0, y: 0, width: ctx.W, height: ctx.H, fill: C.bg });
  ctx.addShape(slide, { x: 0, y: 0, width: 430, height: ctx.H, fill: C.ink });
  ctx.addText(slide, {
    text: "HappyAddress",
    x: 58,
    y: 86,
    width: 330,
    height: 60,
    fontSize: 42,
    bold: true,
    color: C.white,
    typeface: "Arial",
  });
  ctx.addText(slide, {
    text: "WEB-ориентированная система автоматизации учёта и обработки сделок с недвижимостью",
    x: 58,
    y: 170,
    width: 310,
    height: 180,
    fontSize: 24,
    color: "#D6E4FF",
    typeface: "Arial",
    autoFit: "shrinkText",
  });
  ctx.addText(slide, {
    text: "Отчёт о преддипломной практике",
    x: 500,
    y: 120,
    width: 660,
    height: 52,
    fontSize: 32,
    bold: true,
    color: C.ink,
    typeface: "Arial",
  });
  ctx.addText(slide, {
    text: "Чубаров Ярослав Валерьевич · группа ИСИП-406\nСпециальность 09.02.07 Информационные системы и программирование\n2026 год",
    x: 502,
    y: 198,
    width: 700,
    height: 120,
    fontSize: 23,
    color: C.muted,
    typeface: "Arial",
  });
  ctx.addShape(slide, { x: 500, y: 390, width: 680, height: 126, fill: C.blue2, line: { style: "solid", fill: C.blue, width: 2 }, geometry: "roundRect" });
  ctx.addText(slide, {
    text: "Цель проекта: создать веб-приложение, где пользователь может найти объект недвижимости, опубликовать своё объявление и связаться с продавцом.",
    x: 530,
    y: 421,
    width: 620,
    height: 72,
    fontSize: 23,
    bold: true,
    color: C.ink,
    typeface: "Arial",
    autoFit: "shrinkText",
  });
  footer(slide, ctx, 1);
  return slide;
}
