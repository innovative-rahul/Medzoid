

google.charts.load("current", {packages: ["corechart"] });
google.charts.setOnLoadCallback(drawChart);



function drawChart(online,offline) {
    var data = google.visualization.arrayToDataTable([
        ['Task', 'Hours per Day'],
        ['Online Appointment', online],
        ['Offline Appointment', offline],
        //['Watch TV', 2],
        //['Sleep',    7]
    ]);

    var options = {
        title: 'My Practices',
        is3D: true,
        width: 800,
        height: 600,
        colors: ['#80B545', '#FFA200'],
        sliceVisibilityThreshold: 0
    };

    var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));
    chart.draw(data, options);

}

$(document).ready(function () {

    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        url: "../Doctor/GetAppointmentDetails",
        data: '{ }',
        success: function (data) {
            if (data.Success == true) {
                for (var i = 0; i < data.data.clinics.length; i++) {
                    $("#ddlClinic").append('<option value="' + data.data.clinics[i].Id + '">' + data.data.clinics[i].Name + '</option>');
                }

                $("#tdTotal").html(data.data.TotalAppointments);
                $("#tdOffline").html(data.data.TotalOffline);
                $("#tdOnline").html(data.data.TotalOnline);
                drawChart(data.data.TotalOnline, data.data.TotalOffline)
            }
            else if (data.Success == false) {

            }
        },
        error: function (xhr) {
            alert(xhr.responseText)
        }
    });



    $('#ddlClinic').change(function () {
        var param = new Object();

        param.ClinicId = $('#ddlClinic').val();
        param.Month = $('#ddlMonth').val();
        param.Year = $('#ddlYear').val();

        Getdata(param);
       
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: "../Doctor/GetAppointmentDetails",
            data: '{ param: ' + JSON.stringify(param) + '}',
            success: function (data) {
                if (data.Success == true) {
                    if (data.data.timeschedule != null) {
                        for (var i = 0; i < data.data.timeschedule.length; i++) {
                            $("#ddlTimeSchedule").append('<option value="' + data.data.timeschedule[i].Id + '">' + data.data.timeschedule[i].Time + '</option>');
                        }
                    }
                    
                    $("#tdTotal").html(data.data.TotalAppointments);
                    $("#tdOffline").html(data.data.TotalOffline);
                    $("#tdOnline").html(data.data.TotalOnline);

                    drawChart(data.data.TotalOnline, data.data.TotalOffline)
                }
                else if (data.Success == false) {

                }
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        });
    });


    $('#ddlMonth').change(function () {
        Getdata();

    });

    $('#ddlYear').change(function () {
        Getdata();
    });

    $('#ddlTimeSchedule').change(function () {
        Getdata();
    });


    $('#ddlAppointmentStatus').change(function () {
        Getdata();
    });
    

    function Getdata() {

        var param = new Object();
        param.ClinicId = $('#ddlClinic').val();
        param.Month = $('#ddlMonth').val();
        param.Year = $('#ddlYear').val();
        param.ScheduleId = $('#ddlTimeSchedule').val();
        param.AppointmentStatus = $('#ddlAppointmentStatus').val();

        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: "../Doctor/GetAppointmentDetails",
            data: '{ param: ' + JSON.stringify(param) + '}',
            success: function (data) {
                if (data.Success == true) {

                    //if (param.ClinicId == 0) {
                    //    for (var i = 0; i < data.data.clinics.length; i++) {
                    //        $("#ddlClinic").append('<option value="' + data.data.clinics[i].Id + '">' + data.data.clinics[i].Name + '</option>');
                    //    }
                    //}

                    //if (param.ScheduleId == 0) {
                    //    for (var i = 0; i < data.data.timeschedule.length; i++) {
                    //        $("#ddlTimeSchedule").append('<option value="' + data.data.timeschedule[i].Id + '">' + data.data.timeschedule[i].Time + '</option>');
                    //    }
                    //}

                }
                else if (data.Success == false) {

                }

                $("#tdTotal").html(data.data.TotalAppointments);
                $("#tdOffline").html(data.data.TotalOffline);
                $("#tdOnline").html(data.data.TotalOnline);

                drawChart(data.data.TotalOnline, data.data.TotalOffline)

            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        });

    }

});