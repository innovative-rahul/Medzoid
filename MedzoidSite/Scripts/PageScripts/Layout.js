$(document).ready(function () {

    var userString = sessionStorage.getItem('details');
    var user = JSON.parse(userString);
    var firstName = user.userDetails.Name.split(' ')[0];


    $('#hdUid').val(user.userDetails.Id);
    $('#lblName').html(firstName);
    $('#lblUserName').html(user.userDetails.Name);
    $('#lblUserCharacter').html(user.userDetails.Name.charAt(0))
    $('#lblNameChar').html(user.userDetails.Name.charAt(0))

    if (user.userDetails.UserType == "D") {
        $('#liMyFav').hide();
        $('#liMyOrders').hide();
        $('#liQuotation').hide();
        $('#liMyBloodReq').hide();
        $('#liDonorReq').hide();
        $('#liMyPractise').show();
        $('#liNPS').show();
        $('#lnkMyProfile').show();
        $('#lnkUpgradeProfile').hide();


        //Bind Dropdown
        $("#kt_select2_3 optgroup").empty();
        for (var i = 0; i < user.Departments.length; i++) {
            $("#kt_select2_3 optgroup").append('<option value="' + user.Departments[i].Id + '">' + user.Departments[i].DepartmentName + '</option>');
        }


        //Set selected Items
        var selectedItems = new Array();

        if (user.DoctorDetails.deptId != null) {
            var docDepts = user.DoctorDetails.deptId.split(',');
            for (var i = 0; i < docDepts.length; i++) {
                selectedItems.push(docDepts[i]);
            }
        }

        $('#txtMCIReg').val(user.DoctorDetails.MciNo);
        $('#txtExperience').val(user.DoctorDetails.experience);
        $('#txtFee').val(user.DoctorDetails.Fee);
        $('#txtHospitalAffiliation').val(user.DoctorDetails.HospitalAffiliation);
        $('#txtAboutUs').val(user.DoctorDetails.AboutUs);
        $('#liAbout').html(user.DoctorDetails.AboutUs);


        $('#txtMembership').val(user.DoctorDetails.Membership)
        selectedItems.push();
        $('#kt_select2_3').val(selectedItems);
        $('#kt_select2_3').select2().trigger('change');

    }
    else if (user.userDetails.UserType == "B") {
        $('#liMyFav').show();
        $('#liMyOrders').show();
        $('#liQuotation').show();
        $('#liMyBloodReq').show();
        $('#liDonorReq').show();
        $('#liMyPractise').hide();
        $('#liNPS').hide();
        $('#dvdept').hide();
    }

    var image = 'https://api.medzoid.com/Documents/UserImages/' + user.userDetails.Id + '.jpg';

    $('.kt-widget__head img').attr('src', image);
    $('.kt-widget__media img').attr('src', image);

    $('.kt-avatar__holder').css('background-image', 'url(' + image + ')');

    $('#h4Username').html(user.userDetails.Name);
    $('#txtname').val(user.userDetails.Name);
    $('#liEmail').html(user.userDetails.Email);
    $('#liMobNo').html(user.userDetails.Mobile);
    $('#txtMobNo').val(user.userDetails.Mobile);
    $('#txtemail').val(user.userDetails.Email);
    $('#lidob').html(user.userDetails.DOB);
    $('#kt_datepicker_1').val(user.userDetails.DOB);
    $('#liGender').html(user.Gender);
    $("input[name=gender][value=" + user.userDetails.Gender + "]").prop('checked', true);
    $('#liBloodGroup').html(user.userDetails.BloodGroup);
    $('#txtbloodGroup').val(user.userDetails.BloodGroup);

    $('#liBloodDonor').html(user.userDetails.BloodDonor);
    $("input[name=bloodGroup][value=" + user.userDetails.BloodDonor + "]").prop('checked', true);

    //$('#liAddress').html(user.userDetails.Address.split('$')[0]);
    //$('#txtAddress').val(user.userDetails.Address.split('$')[0]);
    //$('#spnlocation').html(user.userDetails.Address.split('$')[1]);
    //$('#txtlocality').val(user.userDetails.Address.split('$')[1]);


    $("#formProfile :input").prop("disabled", true);

    $('#btnEdit').click(function () {
        $("#formProfile :input").prop("disabled", false);
    });

    $('#btnDoctorSubmit').click(function () {
        $("#myModal").modal("hide");
    });

    $('#txtAddress').keyup(function () {
        var add = $('#txtAddress').val();


        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: "../Home/GetAddress",
            data: "{'search':'" + add + "'}",
            success: function (data) {
                debugger;
                if (data.Success == true) {

                    var array = [];
                    var arraywithPlace = [];
                    var dd = JSON.parse(data.suggestions);

                    for (var i = 0; i < dd.predictions.length; i++) {
                        array.push(dd.predictions[i].description);
                        arraywithPlace.push(dd.predictions[i]);
                    }

                    localStorage.setItem("Predictions", JSON.stringify(arraywithPlace));
                    $("#txtAddress").autocomplete({
                        source: array
                    });

                }
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        });

    });

    $('#btnUpgradeToDoctor').click(function () {
        var add = 'D';
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: "../Home/UpdateUserProfileType",
            data: "{'userType':'" + add + "'}",
            success: function (data) {
                if (data.Success == true && data.Msg == "Updated") {
                    location.href = "/Home/Login";
                }
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        });
    });

    $('#lnkMyProfile').click(function () {
        $('#dvAccount').hide(500);
        $('#dvProfile').show(500);

        $('#btnProfile').show();
        $('#btnSubmit').hide();
    });

    $('#lnkMyAccount').click(function () {
        $('#dvAccount').show(500);
        $('#dvProfile').hide(500);
        $('#btnProfile').hide();
        $('#btnSubmit').show();
    });

    $('#btnSubmit').click(function () {

        var userMaster = new Object();
        userMaster.id = $('#hdUid').val();
        userMaster.name = $('#txtname').val();
        userMaster.date_of_birth = $('#kt_datepicker_1').val();
        userMaster.gender = $("input[name='gender']:checked").val();
        userMaster.blood_group = $('#txtbloodGroup').val();
        userMaster.blood_donate = $("input[name='bloodGroup']:checked").val();
        userMaster.address = $('#txtAddress').val() + "$" + $('#txtlocality').val();

        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: "../Home/UserUpdateProfile",
            data: '{ userMaster: ' + JSON.stringify(userMaster) + '}',
            success: function (data) {
                if (data.Success == true) {
                    alert('Profile Updated Successfully');
                }
                if (data.Success == false) {

                }
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        });
    });

    $('#btnProfile').click(function () {

        var model = new Object();
        model.name = $('#txtname').val()
        model.docId = $('#hdUid').val();
        model.mci_no = $('#txtMCIReg').val();
        model.OnlineAppointment = $('#ddlAppointment').val();
        model.experience = $("#txtExperience").val();
        model.Fee = $('#txtFee').val();
        model.HospitalAffiliation = $("#txtHospitalAffiliation").val();
        model.AboutUs = $('#txtAboutUs').val();
        model.deptId = $('#kt_select2_3').val();


        var depts = [];
        if (model.deptId.length > 0) {

            for (var i = 0; i < model.deptId.length; i++) {
                depts += model.deptId[i] + ",";
            }
            model.deptId = depts;
        }


        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: "../Home/UpdateDoctorProfile",
            data: '{ model: ' + JSON.stringify(model) + '}',
            success: function (data) {
                if (data.Success == true) {
                    alert('Profile Updated Successfully');
                }
                if (data.Success == false) {

                }
            },
            error: function (xhr) {
                alert(xhr.responseText)
            }
        });


    });

});

function imgError() {

    //if (user.userDetails.UserType == "D") {
    var image = 'http://api.medzoid.com/Documents/UserImages/NoUser.png';
    //}
    //else {
    //    var image = 'http://api.medzoid.com/Documents/UserImages/NoUser.png';
    //}
    $('.kt-widget__head img').attr('src', image);
    $('.kt-avatar__holder').css('background-image', 'url(' + image + ')');
}