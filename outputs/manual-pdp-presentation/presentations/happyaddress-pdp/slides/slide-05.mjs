import { C, base, footer, card } from "./common.mjs";

export async function slide05(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Архитектура и база данных");
  card(slide, ctx, 72, 215, 300, 140, "Models", "User, Ad, AdImage, Favorite, Chat, ChatMessage", C.blue, C.blue2);
  card(slide, ctx, 490, 215, 300, 140, "Controllers", "Account, Home, Ads, Admin, Chat, Users", C.purple, C.purple2);
  card(slide, ctx, 908, 215, 300, 140, "Views", "Razor-страницы каталога, профиля, объявления и админ-панели", C.green, C.green2);
  ctx.addShape(slide, { x: 595, y: 395, width: 90, height: 58, fill: C.ink, geometry: "roundRect" });
  ctx.addText(slide, { text: "MVC", x: 611, y: 410, width: 58, height: 28, fontSize: 24, bold: true, color: C.white, align: "center", typeface: "Arial" });
  card(slide, ctx, 190, 500, 410, 120, "AppDbContext", "Единая точка доступа к данным и миграциям Entity Framework Core", C.orange, C.orange2);
  card(slide, ctx, 680, 500, 410, 120, "MySQL", "Хранение пользователей, объявлений, фото, избранного и сообщений", C.orange, C.orange2);
  footer(slide, ctx, 5);
  return slide;
}
