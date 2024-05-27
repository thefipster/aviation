import u from "umbrellajs";

u("tr").each(function (node, i) {
  var href =  u(node).attr("href");
  console.log(href);
  if (u(node).attr("href")) u(node).addClass("clickable");
});

u("tr.clickable").on("click", function (e) {
  if (u(this).attr("href")) window.location.href = u(this).attr("href");
});
