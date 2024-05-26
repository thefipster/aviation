import * as $ from "jquery";

(function ($) {
  $("#banner").each(function () {
    var $this = $(this),
      $image = $this.find(".banner-image"),
      $img = $image.find("img");
    if ($img.length > 0) {
      $this.css("background-image", "url(" + $img.attr("src") + ")");
      $image.remove();
    }
  });

  $(window).on("scroll", () => {
    var banner = $("#banner");
    if (banner) {
      var scrolledY = $(window).scrollTop();
      $("#banner").css("background-position-y", scrolledY * 0.5 + "px");
    }
  });
})(jQuery);
