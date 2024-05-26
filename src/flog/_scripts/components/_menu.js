import $ from 'jquery';
    
$(function(){
  $(".menu-button").on("click", function (event) {
    event.stopPropagation();
    event.preventDefault();

    $("#menu").addClass("open");
  });

  $("#menu").on("click", function (event) {
    $("#menu").removeClass("open");
  });
});
