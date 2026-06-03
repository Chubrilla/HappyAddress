import { C, base, footer, card } from "./common.mjs";

export async function slide07(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Реализованные модули");
  const data = [
    ["Аккаунт", "регистрация, вход, подтверждение email, профиль и аватар", C.blue, C.blue2],
    ["Объявления", "создание, редактирование, удаление, фотографии и статусы", C.green, C.green2],
    ["Каталог", "поиск, фильтрация, сортировка и постраничный вывод", C.purple, C.purple2],
    ["Карта", "координаты объекта и отображение расположения", C.orange, C.orange2],
    ["Избранное", "сохранение интересных объектов для сравнения", "#CA8A04", "#FEFCE8"],
    ["Чат", "переписка покупателя и продавца по объявлению", C.red, C.red2],
  ];
  data.forEach((item, index) => {
    const col = index % 3;
    const row = Math.floor(index / 3);
    card(slide, ctx, 70 + col * 405, 205 + row * 190, 340, 138, item[0], item[1], item[2], item[3]);
  });
  footer(slide, ctx, 7);
  return slide;
}
