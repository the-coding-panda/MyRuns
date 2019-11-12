// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    let queryString = new URLSearchParams(window.location.search);

    if (queryString.has('runType')) {
        let runType = queryString.get('runType');
        setDistanceButtons(runType);
        $("input[name=runningFilters]").val([runType]);
    }
    else {
        $("input[name=runningFilters]").val(["RunsOnly"]);
    }
    
    $('.custom-control-input').change(function () {

        var radioSelected = $('input[name=runningFilters]:checked').val();
        setDistanceButtons(radioSelected);        
    });

    function setDistanceButtons(runTypeValue) {
        var distanceButtons = $('.btn-distance');

        for (var i = 0; i < distanceButtons.length; i++) {

            var button = distanceButtons[i];

            var currentUrl = button.href;
            var url = new URL(currentUrl);
            url.searchParams.set("runType", runTypeValue); // setting your param
            button.href = url.href;

        }
    }
});