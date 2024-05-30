import u from "umbrellajs";

u(".menu-button").on("click", function (event) {
  event.stopPropagation();
  event.preventDefault();
  u("#menu").addClass("open");
});

u("#menu").on("click", function (event) {
  u("#menu").removeClass("open");
});