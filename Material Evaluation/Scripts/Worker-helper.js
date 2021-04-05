function messageHandler(e) {
    
    if (e.data === "fetch") {
        GetReportData();
    }
}

function GetReportData() {
    try {
        debugger;

        var MainData = [];
        var DataDet = [];
        var url = "http://localhost:49703/" + "/EmetServices/CompledRequest/MyJson.asmx/LoadData";
        var _Plant = "2100";
        var _Status = "ALL";
        var _SMNStatus = "ALL";
        var _ReqType = "ALL";
        var _ReqStatus = "Closed";
        var _FltrDate = "RequestDate";
        var _From = "";
        var _To = "";

        var _FilterBy = "Plant";
        var _FilterValue = "";

        $.ajax({
            url: url,
            cache: false,
            type: "POST",
            dataType: 'json',
            //contentType: "application/json; charset=utf-8",
            data: {
                Plant: _Plant, Status: _Status, SMNStatus: _SMNStatus, ReqType: _ReqType, ReqStatus: _ReqStatus,
                FltrDate: _FltrDate, From: _From, To: _To, FilterBy: _FilterBy, FilterValue: _FilterValue, VendorCode: ""
            },
            async: false,
            beforeSend: function () {
            },
            complete: function () {
                //SetupAndLoadData(MainData, DataDet);
            },
            success: function (data) {
                if (data.success == true) {
                    MainData = data.MainData;
                    DataDet = data.AllRequestData;
                }
                else {
                    //alert(data.message);
                }
            },
            error: function (xhr, status, error) {
                //alert(error);
            }
        });
    } catch (e) {
        //alert("GetReportData : " + e);
    }
}


addEventListener("message", messageHandler, true);


//onmessage = function (e) {
//    debugger;
    
//    self.addEventListener("message", function (e) {
//        // `e.data` contains data sent from main thread
//        const myFunction = JSONfn.parse(e.data.myFunction);
//        myFunction(e.data.payload); // reconstructed and callable
//    });

//    var workerResult = 'Result: ok';
//    console.log('Posting message back to main script');
//    postMessage(workerResult);
//}
