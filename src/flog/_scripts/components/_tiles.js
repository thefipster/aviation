import $ from 'jquery';
    
$(function(){
  $(".tile").each(function () {
    var $this = $(this),
      $image = $this.find(".tile-image"),
      $img = $image.find("img");
    if ($img.length > 0) {
      $this.css("background-image", "url(" + $img.attr("src") + ")");
      $image.remove();
    }
  });
});
