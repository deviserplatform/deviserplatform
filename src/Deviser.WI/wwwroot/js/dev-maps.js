(function () {
    var sdMap = function (options) {
        var map,
            mapElementId = options.mapElementId,
            markerImage = options.markerImage,
            locations = options.locations,
            showMoreLabel = options.showMoreLabel,
            mapOptions = options.mapOptions,
            mapStyles = options.mapStyles,
            infoBox;

        if (options.infoBoxOptions) {
            infoBox = new InfoBox(options.infoBoxOptions);
        }

        this.init = init;

        function init() {
            map = new google.maps.Map(document.getElementById(mapElementId), mapOptions);
            var bounds = new google.maps.LatLngBounds();

            var styledMapType = new google.maps.StyledMapType(mapStyles, {
                name: 'styled_map'
            });
            map.mapTypes.set('styled_map', styledMapType);
            map.setMapTypeId('styled_map');

            for (var i = 0; i < locations.length; i++) {
                var objMap = locations[i];
                var position = new google.maps.LatLng(objMap.latitude, objMap.longitude);
                bounds.extend(position);
                var marker = new google.maps.Marker({
                    icon: markerImage,
                    map: map,
                    position: position,
                    title: objMap.Title,
                    animation: google.maps.Animation.DROP
                });

                if (infoBox) {
                    google.maps.event.addListener(marker, 'click', (function (mrkr, ctnt) {
                        return function () {
                            infoBox.setContent(ctnt);
                            infoBox.open(map, mrkr);
                            map.panTo(mrkr.getPosition());
                            map.panBy(mapOptions.pan.x, mapOptions.pan.y);
                        }
                    })(marker, objMap.content));
                }
            }

            if (locations.length === 1) {
                objMap = locations[0];
                map.center = new google.maps.LatLng(objMap.latitude, objMap.longitude);
            }
            else {
                map.fitBounds(bounds);
            }
            map.panBy(mapOptions.pan.x, mapOptions.pan.y);
        }
    }
    window.sdMap = sdMap;
}());