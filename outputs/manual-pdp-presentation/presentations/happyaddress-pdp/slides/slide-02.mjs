import { C, base, footer, card } from "./common.mjs";

export async function slide02(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Актуальность разработки");
  card(slide, ctx, 74, 205, 340, 230, "Проблема", "Данные об объектах, фотографиях, контактах, статусах и переписке часто хранятся разрозненно.", C.red, C.red2);
  card(slide, ctx, 470, 205, 340, 230, "Риск", "Ошибки в цене, адресе, площади или статусе объявления снижают доверие и увеличивают время обработки.", C.orange, C.orange2);
  card(slide, ctx, 866, 205, 340, 230, "Решение", "Единая веб-система централизует объявления, упрощает поиск и вводит модерацию публикаций.", C.green, C.green2);
  ctx.addText(slide, {
    text: "HappyAddress объединяет каталог недвижимости, личный кабинет, карту, избранное, модерацию и коммуникацию между пользователями.",
    x: 112,
    y: 508,
    width: 1056,
    height: 80,
    fontSize: 28,
    bold: true,
    color: C.ink,
    align: "center",
    typeface: "Arial",
  });
  footer(slide, ctx, 2);
  return slide;
}
