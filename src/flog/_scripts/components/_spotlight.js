import u from "umbrellajs";

  u(".spotlights section .chrome").each(function (node, i) {
    var uimg = u(node).find("img");

    if (uimg.length > 0) {
      var src = uimg.attr("src");
      node.style.backgroundImage = "url(" + src + ")";
      uimg.remove();
    }
  });
