// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('#includeTreadmill').change(function () {

    var distanceButtons = $('.btn-distance');

    for (var i = 0; i < distanceButtons.length; i++) {

        var button = distanceButtons[i];

        var currentUrl = button.href;
        var url = new URL(currentUrl);
        url.searchParams.set("includeTreadmill", $(this).prop('checked')); // setting your param
        button.href = url.href;

        }
    });
});