// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    let queryString = new URLSearchParams(window.location.search);
    let runType, distance;

    if (queryString.has('runType')) {
        runType = queryString.get('runType');
        setDistanceButtons(runType);
        $("input[name=runningFilters]").val([runType]);
    }
    else {
        $("input[name=runningFilters]").val(["RunsOnly"]);
    }

    if (queryString.has('distance')) {
        distance = queryString.get('distance');
    }
    else {
        distance = 'TenKilometers';
    }

    var radioSelected = $('input[name=runningFilters]:checked').val();

    //Initial load
    init(distance, radioSelected);
    
    $('.custom-control-input').change(function () {

        var radioSelected = $('input[name=runningFilters]:checked').val();
        setDistanceButtons(radioSelected);

        refreshActivities(distance, radioSelected);
        
    });

    $('.btn-distance').click(function (e) {
        e.preventDefault();

        var url = new URL(e.target.href);

        distance = url.searchParams.get("distance"); // setting your param

        var radioSelected = $('input[name=runningFilters]:checked').val();
        refreshActivities(distance, radioSelected);
    });

    function init(distance, radioSelected) {

        refreshActivities(distance, radioSelected);

    }

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

    function refreshActivities(distance, runType) {
        var activities = $("#activities");

        activities.fadeOut(500, function () {
            $(this).empty();
        });

        setTimeout(function () {
            $("#activities").load('Home/_Activities?distance=' + distance + '&runType=' + runType, null, function () {
                $('.loading-msg').css("visibility", "hidden");
            }).fadeIn(500);
        }, 500);
        
    }
});