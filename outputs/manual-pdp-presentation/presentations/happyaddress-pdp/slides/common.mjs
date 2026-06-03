export const C = {
  ink: "#172033",
  muted: "#667085",
  blue: "#2563EB",
  blue2: "#DBEAFE",
  green: "#16A34A",
  green2: "#DCFCE7",
  purple: "#7C3AED",
  purple2: "#EDE9FE",
  orange: "#EA580C",
  orange2: "#FFEDD5",
  red: "#DC2626",
  red2: "#FEE2E2",
  line: "#CBD5E1",
  bg: "#F7F9FC",
  white: "#FFFFFF",
};

export function base(slide, ctx, title, kicker = "HappyAddress · преддипломная практика") {
  ctx.addShape(slide, { x: 0, y: 0, width: ctx.W, height: ctx.H, fill: C.bg });
  ctx.addText(slide, {
    text: kicker,
    x: 54,
    y: 34,
    width: 760,
    height: 26,
    fontSize: 17,
    color: C.muted,
    typeface: "Arial",
  });
  ctx.addText(slide, {
    text: title,
    x: 54,
    y: 72,
    width: 940,
    height: 76,
    fontSize: 34,
    bold: true,
    color: C.ink,
    typeface: "Arial",
  });
  ctx.addShape(slide, { x: 54, y: 150, width: 1172, height: 2, fill: C.line });
}

export function footer(slide, ctx, n) {
  ctx.addText(slide, {
    text: String(n).padStart(2, "0"),
    x: 1168,
    y: 664,
    width: 60,
    height: 28,
    fontSize: 16,
    bold: true,
    color: C.muted,
    align: "right",
    typeface: "Arial",
  });
}

export function card(slide, ctx, x, y, w, h, title, body, color = C.blue, fill = C.white) {
  ctx.addShape(slide, {
    x,
    y,
    width: w,
    height: h,
    fill,
    line: { style: "solid", fill: color, width: 2 },
    geometry: "roundRect",
  });
  ctx.addText(slide, {
    text: title,
    x: x + 22,
    y: y + 18,
    width: w - 44,
    height: 32,
    fontSize: 22,
    bold: true,
    color: C.ink,
    typeface: "Arial",
  });
  ctx.addText(slide, {
    text: body,
    x: x + 22,
    y: y + 58,
    width: w - 44,
    height: h - 74,
    fontSize: 17,
    color: C.muted,
    typeface: "Arial",
    autoFit: "shrinkText",
    insets: { left: 0, top: 0, right: 0, bottom: 0 },
  });
}

export function pill(slide, ctx, x, y, text, color, fill) {
  ctx.addShape(slide, {
    x,
    y,
    width: 188,
    height: 42,
    fill,
    line: { style: "solid", fill: color, width: 2 },
    geometry: "roundRect",
  });
  ctx.addText(slide, {
    text,
    x: x + 8,
    y: y + 9,
    width: 172,
    height: 24,
    fontSize: 16,
    bold: true,
    color,
    align: "center",
    typeface: "Arial",
  });
}

export function bullet(slide, ctx, x, y, text, color = C.blue) {
  ctx.addShape(slide, { x, y: y + 8, width: 10, height: 10, fill: color, geometry: "ellipse" });
  ctx.addText(slide, {
    text,
    x: x + 24,
    y,
    width: 500,
    height: 44,
    fontSize: 20,
    color: C.ink,
    typeface: "Arial",
    autoFit: "shrinkText",
  });
}

export function arrow(slide, ctx, x1, y1, x2, y2, color = C.muted) {
  const line = ctx.addShape(slide, {
    x: x1,
    y: y1,
    width: Math.max(1, x2 - x1),
    height: Math.max(1, y2 - y1),
    fill: "#00000000",
    line: { style: "solid", fill: color, width: 3 },
  });
  return line;
}
