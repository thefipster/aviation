import u from "umbrellajs";
    
u(".tile").each(function (node, i) {
    var uimage = u(node).find(".tile-image");
    var uimg = uimage.find("img");
  if (uimg.length > 0) {
    var src = uimg.attr("src");
    node.style.backgroundImage = "url(" + src + ")";
    uimage.remove();
  }
});
