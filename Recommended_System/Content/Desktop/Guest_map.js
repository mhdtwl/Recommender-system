//==================================== Map =============================
function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
    var R = 6371; // Radius of the earth in km
    var dLat = deg2rad(lat2 - lat1);  // deg2rad below
    var dLon = deg2rad(lon2 - lon1);
    var a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c; // Distance in km
    return d;
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}
//===============
function set_my_location(s)
{
    $.ajax({
        type: "GET",
        traditional: true,
        data: { latlon: s },
        url: "/Item/set_guest_locat",
        dataType: "json",
        success: console.log("Arive"),
        error: console.log("AjaxError")
    });
}
function Get_Distance(lat2, lon2) {
    lat1 = UseMyLocation()[0] ;
    lon1 = UseMyLocation()[1] ;
    d = getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2)
    document.getElementById("Distace_div").innerHTML = d;
}
//----------------
function UseMyLocation() {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;
    return { lat : lng}
}
function success(position) {
    var s = document.querySelector('#status');

    if (s.className == 'success') {
        //showPosition();
        // not sure why we're hitting this twice in FF, I think it's to do with a cached result coming back
        return;
    }
    //navigator.geolocation.getCurrentPosition(showPosition);
    s.innerHTML = "found you!";
    s.className = 'success';
    var lat = position.coords.latitude  ;
    var lon = position.coords.longitude ;
    set_my_location( [ lat, lon]); 
    var mapcanvas = document.createElement('div');
    mapcanvas.id = 'mapcanvas';
    mapcanvas.style.height = '400px';
    mapcanvas.style.width = '560px';

    document.querySelector('article').appendChild(mapcanvas);

    var latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    var myOptions = {
        zoom: 15,
        center: latlng,
        mapTypeControl: false,
        navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL },
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    showPosition(latlng)
    var map = new google.maps.Map(document.getElementById("mapcanvas"), myOptions);
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: "You are here! (at least within a " + position.coords.accuracy + " meter radius)"
    });

}

function error(msg) {
    var s = document.querySelector('#status');
    s.innerHTML = typeof msg == 'string' ? msg : "failed";
    s.className = 'fail';

    // console.log(arguments);
}

if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(success, error);
} else {
    error('not supported');
}
function showPosition(position) {
    //alert(position)
    var x = document.getElementById("infoPanel");
    x.innerHTML = position;
}
