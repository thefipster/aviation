import * as $ from "jquery";

(function ($) {
  $(function () {
    var $banner = $("#banner");

    // Banner.
    $banner.each(function () {
      var $this = $(this),
        $image = $this.find(".image"),
        $img = $image.find("img");

      // Image.
      if ($image.length > 0) {
        // Set image.
        $this.css("background-image", "url(" + $img.attr("src") + ")");

        // Hide original.
        $image.hide();
      }
    });
  });
})(jQuery);
