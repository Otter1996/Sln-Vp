
var markers = [];
var infowindows = [];
var select_map;

function initMap(map, travelMode) {
    // 載入路線服務與路線顯示圖層
    var directionsService = new google.maps.DirectionsService();
    var directionsDisplay = new google.maps.DirectionsRenderer();


    // 初始化地圖
    select_map = new google.maps.Map(document.getElementById(map), {
        zoom: 10,
        center: { lat: 22.683366691184965, lng: 120.29669086057828 }
    });
    // 放置路線圖層
    directionsDisplay.setMap(select_map);

    // 路線相關設定
    var request = {
        origin: { lat: 22.687916655554677, lng: 120.30725314748948 },
        destination: { lat: 22.683366691184965, lng: 120.29669086057828 },
        travelMode: travelMode,
    };

    // 繪製路線
    directionsService.route(request, function (result, status) {
        if (status == 'OK') {
            var steps = result.routes[0].legs[0].steps;
            steps.forEach((e, i) => {
                console.log(steps);
                //加入地圖記號
                markers[i] = new google.maps.Marker({
                    position: { lat: e.start_location.lat(), lng: e.start_location.lng() },
                    map: select_map,
                    label: { text: i + '', color: "#fff" }
                });
                //加入資訊視窗
                infowindows[i] = new google.maps.InfoWindow({
                    content: e.instructions
                });
                //加入地圖標記點擊事件
                markers[i].addListener('click', function () {
                    //是否展開
                    if (infowindows[i].anchor) {
                        //關閉
                        infowindows[i].close();
                    } else {
                        //展開
                        infowindows[i].open(map, markers[i]);
                    }
                });
            });
            directionsDisplay.setDirections(result);
        } else {
            console.log(status);
        }
    });

}

$("#transit_header").click(function () {
$("#transit").collapse('show');
initMap('transit_map', 'TRANSIT');//啟動API
$("#drive").collapse('hide');
$("#walk").collapse('hide');
});
$("#drive_header").click(function () {
    $("#drive").collapse('show');
    initMap('drive_map', 'DRIVING');//啟動API
    $("#transit").collapse('hide');
    $("#walk").collapse('hide');
});
$("#walk_header").click(function () {
    $("#walk").collapse('show');
    initMap('walk_map', 'WALKING');//啟動API
    $("#transit").collapse('hide');
    $("#drive").collapse('hide');
});
