//====================================================================================



/////---------------------------Login View --------------------------

var login_toggle = function () {

    if ($(".Mydropdown_menu").css("display") == 'none')
        $(".Mydropdown_menu").css({ 'display': 'block' });
    else
        $(".Mydropdown_menu").css({ 'display': 'none' });

}
/////---------------------------Login --------------------------
var LoginChecker = function () {
    user = $('#UsernameLogin').val();
    user = user.split(" ")
    pass = $('#PasswordLogin').val();
    var objList = [user[0], pass];
    var obj = JSON.stringify(objList);
    $.ajax({
        type: "GET",
        url: "LogOn/CheckLogin", /// controller
        data: { 'Login_Inputs': obj },  /// parameter
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: Login_result,
        error: console.log('err')

    });
};
var LogOn_Stts = {"Custm":0, "Error":1, "Invld":2}
var Login_result = function (data) {
    if (data == LogOn_Stts.Custm)
    {
        location.reload(this);
    }
    else if (data == LogOn_Stts.Error) {
        //  alert('Err as custom');
        $("#LoginAlerts").css({ "display": "block" });
    }
    else if (data == LogOn_Stts.Invld) alert('invald as custom');
    else
        alert('Invalid Login')
}
/////---------------------------Logout --------------------------
var LogoutChecker = function ()
{
//    //var objList = ["0", "1"];
//    //var obj = JSON.stringify(objList);
//    $.ajax({
//        type: "GET",
//        url: "LogOn/CheckLogout2", /// controller
//        //data: { 'Logout_Inputs': obj },  /// parameter
//        //contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: Logout_result,
//        error: console.log('err')

//    });
    var objList = ["0", "1"];
    var obj = JSON.stringify(objList);
    $.ajax({
        type: "GET",
        url: "LogOn/CheckLogout", /// controller
        data: { 'Logout_Inputs': obj },  /// parameter
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: Logout_result,
        error: console.log('err')

    });
}
var Logout_result = function (data) {
    if (data == "Ok_Logout") {
        //alert("Logout")
        location.reload(this);
    }
    else
        alert('Invalid Logout')
}
///====================================================================================
//////---------------------------Search --------------------------
var quickSearch = function () {
    var serachstring = $("#quick_search").val();
    var response;  //  @*'@(model.Url)'*@ 
    $.ajax({
        type: "GET",
        data: { keyword: serachstring },
        url: "/Home/SearchProduct",
        dataType: "json",
        success: success_func,
        error: console.log("AjaxError")
    });
    $('#productGrid').html();
};
var success_func = function (data) {
    $("#productGrid").empty();
    $("#productGrid").css("display", "block");

    /* display: none; */
    for (var i = 0; i < data.length; i++) {
        var str = data[i].T_Name;
        var t_id = data[i].T_Id

        //parts = "<tr id='qsrchTr'><td class='tdQSrc'><a href='#'class='aQSrch' >" + str + "</td></tr>"
        parts = "<a href='/Item/Details/" + t_id + "'class='aQSrch' ><li class='liQsrch' role='presentation' >" + str + "</li></a>"
        $('#productGrid').append(parts)
        $("#qsrchTr").css({ width: "100%", padding: "3px;" });
    }
}
///====================================================================================
//////---------------------------Compare --------------------------
TotalCompareList = [];

var set_cmplst_vals = function (lst) {
    var lst_itm_name = [];
    for (var i = 0; i < lst.length; i++) {
        lst_itm_name = lst[i].item_name;
    }
    TotalCompareList = lst_itm_name;

}
var sendLstValasClear = function () {
    var lstt = new Array();
    lstt = TotalCompareList;
    var cat_id = $('#CategoryDDL').find(":selected").val();
    //var lst_out
    //send_out_succ_ClearCmp

    $.ajax({
        type: "POST",
        traditional: true,
        url: "/Item/itmsSrchNameLstSetasClear",
        dataType: "json",
        data: { itmsCmprLst: lstt },
        success: send_out_succ_ClearCmp,
        error: console.log("no")
    });

}
var sendLstVal = function () {
    var lstt = new Array();
    lstt = TotalCompareList;
    var cat_id = $('#CategoryDDL').find(":selected").val();
    //var lst_out
    $.ajax({
        type: "GET",
        traditional: true,
        url: "/Item/itmsCmprNameLstSet",
        dataType: "json",
        data: { itmsCmprLst: lstt },
        success: send_out_succ,
        error: console.log("no")
    });
}
var send_out_succ_ClearCmp = function (data) {
    TotalCompareList = data; //set_cmplst_vals(TotalCompareList); 
    location.reload(this);
}
var send_out_succ = function (data) {
    TotalCompareList = data; //set_cmplst_vals(TotalCompareList); 
}


var modifiLstCompareProducts = function (element, id) {
    var index = TotalCompareList.indexOf(element);
    if (id == 0) { }
    TotalCompareList.splice(index, 1);
    var dd = "#prnt_lbl_" + id;
    var s = $(dd).remove();
    var s = $("#pAndt_lbl_" + (id - 1)).remove();
    sendLstVal();

    location.reload();
}
var lstCompateProducts = function () {
    $("#content_cmpr").empty();
    var newVal = $("#itmsNameLst").val();
    if (typeof (TotalCompareList) == 'undefined')
        TotalCompareList = new Array();
    TotalCompareList.push(newVal);

    if (TotalCompareList.length != 0) {

        var h5 = "<h5>"; var h5s = "</h5>"
        var parts = "";
        for (var i = 0; i < TotalCompareList.length ; i++) { // style='font-size:large'>
            parts += "<span class='btn btn-info' style='cursor:default' id='prnt_lbl_" + i + "'><span id='lbl_" + i + "'>" + TotalCompareList[i] + "</span>"
                + "<button type='button' onclick='modifiLstCompareProducts(TotalCompareList[" + i + "], " + i + ")' class='close clsBtn'>&times;</button></span>";
            if (TotalCompareList.length != (i + 1))
            { parts += "<span> &nbsp;</span> <span class='label label-primary'id='pAndt_lbl_" + i + "' >&</span> <span> &nbsp;</span>" }
        }
        $("#content_cmpr").append(h5);
        $("#content_cmpr").append(parts).show('slow');
        $("#content_cmpr").append(h5s);

        sendLstVal();
    }
}
var searchForCompare = function () {

    var serachCat = $("#CategoryID").val();
    if (serachCat === "0") { alert('Please SELECT a Category FIRST  ,that to compare by !!'); return; }
    var serachstring = $("#itmsNameLst").val(); /// $( "#myselect option:selected" ).text();
    var cnt = { item: serachstring, category: serachCat };
    var response;
    //---------------------
    var objList = [serachstring, serachCat];
    var obj = JSON.stringify(objList);
    $.ajax({
        type: "GET",
        url: "/Item/CompareSearchProduct",
        data: { 'cmp_srch': obj },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func_cmp
    });
    //---------------------
    $('#productGrid_cmpr').html();
}
var success_func_cmp = function (data) {
    $("#productGrid_cmpr").empty();
    $("#productGrid_cmpr").css("display", "block");

    /* display: none; */
    for (var i = 0; i < data.length; i++) {
        var str = data[i].T_Name;
        //parts = "<tr id='qsrchTr'><td class='tdQSrc'><a href='#'class='aQSrch' >" + str + "</td></tr>"
        parts = "<li class='liQsrch' role='presentation' onclick='selectItemCmp(" + i + ")' id='liSlctProd_" + i + "' >" + str + "</li>"
        $('#productGrid_cmpr').append(parts)
        $("#qsrchTr").css({ width: "100%", padding: "3px;" });
    }

}
var selectItemCmp = function (ind) {
    var s = $("#liSlctProd_" + ind).html()
    $("#itmsNameLst").val(s);
}
//-----------------------------------------------------
$(document).ready(TotCmpLst);

var ClearComparision = function () {
    $("#content_cmpr").empty();
    TotalCompareList = new Array();
    sendLstValasClear();

}

var TotCmpLst = function () {
    //alert("hi cmp")
    //var TotalCompareList = $("#myHideModelinp").val()

    var t = new Array();
    var mod_lst = $("#content_cmpr").find(".prnt_cls")
    for (var i = 0; i < mod_lst.length; i++) {
        var tmp = mod_lst.find("#lbl_" + i).text();
        t.push(tmp);
    }
    TotalCompareList = t;
    var s = "";
    for (var i = 0; i < TotalCompareList.length; i++) {
        s += TotalCompareList.valueOf(i) + " , ";
    }


}
var assign_cmpLst = function (data) {
    if (typeof (data) != 'null') {
        TotalCompareList = data;
    }
}

///====================================================================================
var close_qSearch = function () {
    $('#productGrid').css("display", "none");
    $('#productGrid_cmpr').css("display", "none");

}
///====================================================================================
//------------------------ Map --------------------------------------
var shwGestLocat = function () {
    //var status = $(".Guest_Location").css('display' , '');
    //$(".GuestLocationUL").toggle(100);
    $("#map").toggle();

}
//--------------------------- Map View--------------------------------



var map_toggle = function () {

    //if ($("#GeoMap").css("display") == 'none')
    //    $("#GeoMap").css({ 'display': 'block' });
    //else
    //    $("#GeoMap").css({ 'display': 'none' });

    //=================
    if ($("#map").css("visibility") == 'hidden')
        $("#map").css({ 'visibility': 'visible' });
    else
        $("#map").css({ 'visibility': 'hidden' });
}


//////---------------------------Fixed QuickSeachBar --------------------------
