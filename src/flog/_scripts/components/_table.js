import * as $ from "jquery";

(function ($) {
  $("tr").each(function () {
    if ($(this).attr("href")) $(this).addClass("clickable");
  });

  $("tr.clickable").on("click", function () {
    if ($(this).attr("href")) window.location.href = $(this).attr("href");
  });
})(jQuery);
