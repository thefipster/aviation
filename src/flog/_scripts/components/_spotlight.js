import $ from "jquery";

$(function () {
  $(".spotlights section").each(function () {
    
    var $chrome = $(this).find(".chrome");
    var $img = $chrome.find("img");

    if ($img.length > 0) {
      $chrome.css("background-image", "url(" + $img.attr("src") + ")");
      $img.remove();
    }
  });
});
