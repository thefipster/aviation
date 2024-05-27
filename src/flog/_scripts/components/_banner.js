import u from "umbrellajs";

u("#banner").each(function (node, i) {
  var unode = u(node);
  var uimage = unode.find(".banner-image");
  var uimg = uimage.find("img");
  if (uimg.length > 0) {
    const src = uimg.attr("src");
    node.style.backgroundImage = "url(" + src + ")";
    uimage.remove();
  }
});

document.addEventListener("DOMContentLoaded", function (event) {
  addEventListener("scroll", (event) => {
    var banner = document.getElementById("banner");
    if (banner) {
      var scrolledY = window.scrollY;
      banner.style.backgroundPositionY = scrolledY * 0.5 + "px";
    }
  });
});
