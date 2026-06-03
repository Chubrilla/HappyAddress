import { C, base, footer, pill } from "./common.mjs";

export async function slide04(presentation, ctx) {
  const slide = presentation.slides.add();
  base(slide, ctx, "Технологический стек");
  const items = [
    ["C#", C.blue, C.blue2], [".NET 8", C.blue, C.blue2], ["ASP.NET Core MVC", C.blue, C.blue2],
    ["MySQL", C.green, C.green2], ["EF Core", C.green, C.green2], ["Pomelo MySQL", C.green, C.green2],
    ["HTML/CSS", C.purple, C.purple2], ["JavaScript", C.purple, C.purple2], ["Bootstrap", C.purple, C.purple2],
    ["Leaflet", C.orange, C.orange2], ["OpenStreetMap", C.orange, C.orange2], ["BCrypt", C.red, C.red2],
  ];
  let i = 0;
  for (let row = 0; row < 4; row++) {
    for (let col = 0; col < 3; col++) {
      const [text, color, fill] = items[i++];
      pill(slide, ctx, 124 + col * 338, 218 + row * 82, text, color, fill);
    }
  }
  ctx.addShape(slide, { x: 120, y: 570, width: 1040, height: 72, fill: C.white, line: { style: "solid", fill: C.line, width: 2 }, geometry: "roundRect" });
  ctx.addText(slide, {
    text: "Выбранный стек обеспечивает MVC-структуру, работу с реляционной базой, серверную валидацию, загрузку файлов и интерактивный интерфейс.",
    x: 150,
    y: 590,
    width: 980,
    height: 36,
    fontSize: 21,
    color: C.ink,
    align: "center",
    typeface: "Arial",
  });
  footer(slide, ctx, 4);
  return slide;
}
