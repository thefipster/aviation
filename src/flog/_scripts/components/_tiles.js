import * as $ from "jquery";

(function ($) {
  $(function () {
    var $window = $(window);
    var $wrapper = $("#wrapper");

    // Clear transitioning state on unload/hide.
    $window.on("unload pagehide", function () {
      window.setTimeout(function () {
        $(".is-transitioning").removeClass("is-transitioning");
      }, 250);
    });

    // Tiles.
    var $tiles = $(".tiles > article");
    $tiles.each(function () {
      var $this = $(this),
        $image = $this.find(".image"),
        $img = $image.find("img"),
        $link = $this.find(".link"),
        x;

      // Image.

      // Set image.
      $this.css("background-image", "url(" + $img.attr("src") + ")");

      // Set position.
      if ((x = $img.data("position"))) $image.css("background-position", x);

      // Hide original.
      $image.hide();

      // Link.
      if ($link.length > 0) {
        var $x = $link.clone().text("").addClass("primary").appendTo($this);

        $x.attr("aria-label", $link.text());

        $link = $link.add($x);

        $link.on("click", function (event) {
          var href = $link.attr("href");

          // Prevent default.
          event.stopPropagation();
          event.preventDefault();

          // Start transitioning.
          $this.addClass("is-transitioning");
          $wrapper.addClass("is-transitioning");

          // Redirect.
          window.setTimeout(function () {
            if ($link.attr("target") == "_blank") window.open(href);
            else location.href = href;
          }, 500);
        });
      }
    });
  });
})(jQuery);
