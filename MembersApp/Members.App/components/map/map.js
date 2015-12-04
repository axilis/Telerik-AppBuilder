'use strict';

app.map = kendo.observable({
    onShow: function () { },
    afterShow: function () {

    },

    gMap: {},

    longitude: 45.8,
    latitude: 16,

    initMap: function () {

        app.map.gMap = new google.maps.Map(document.getElementById('map'), {
            center: { lat: app.map.longitude, lng: app.map.latitude },
            zoom: 12
        });

        app.map.gMap.addListener('center_changed', function () {
            app.map.mapMove();
        });
        
    },
    mapMove: function () {

        var center = app.map.gMap.getCenter();

        app.map.set("longitude", center.lat());
        app.map.set("latitude", center.lng());

        console.log(center.lat());

    },
    locateMe: function() {

        var options = {
            enableHighAccuracy: true
        },
        that = this;

        showToast("Locating...")

        app.mobileApp.pane.loader.show();

        navigator.geolocation.getCurrentPosition(function (position) {
            
            console.log(position);

            var location = { lat: position.coords.latitude, lng: position.coords.longitude };

            app.map.gMap.panTo(location);

            var marker = new google.maps.Marker({
                position: location,
                map: app.map.gMap,
                title: "I'm here!"
            });

            app.mobileApp.pane.loader.hide();

        }, function () {
            app.mobileApp.pane.loader.hide();
            alert("Error getting location");
        }, options);
    }

});


function initMap() {
    app.map.initMap();
}
