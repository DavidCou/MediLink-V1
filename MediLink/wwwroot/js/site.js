// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let arrayIdOffice = [];
let arrayIdLanguage = [];

let arrayIdAddRequestPract = [];
let arrayIdRemoveRequestPract = [];
let arrayPatientUpdateNewRequest = [];

//variable global to store the value of the select selected in the Practitioner home page to update patient request status
let valueSelectNewPatientRequest = "";

//disable save button in the Practictioner Home Page
var btnSaveAddres = document.getElementById("btn-save-address");
//disable save button in the Patien Request Practictioner Page
var btnSaveRequest = document.getElementById("btn-save-request-pract");

//disable save button in the New Patien Request in the Practictioner Page
var btnSaveRequestNewPat = document.getElementById("btn-save-request-pat");


if (btnSaveAddres != undefined) {
    btnSaveAddres.disabled = true;
}

if (btnSaveRequest != undefined) {
    btnSaveRequest.disabled = true;
}

if (btnSaveRequestNewPat != undefined) {
    btnSaveRequestNewPat.disabled = true;
}


//function to search medical offices
function searchOffice() {
    //declare variables
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("filterInputOffice");
    filter = input.value.toUpperCase();
    table = document.getElementById("data-table-offices");
    tr = table.getElementsByTagName("tr");


    //loop through all table rows and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerHTML;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

//function to search medical offices
function searchOfficeByTable(table, input) {
    //declare variables
    console.log("here pass")
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById(input);
    filter = input.value.toUpperCase();
    table = document.getElementById(table);
    tr = table.getElementsByTagName("tr");
    console.log(table);

    //loop through all table rows and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        console.log(td);
        if (td) {
            txtValue = td.textContent || td.innerHTML;
            console.log(txtValue);
            console.log("Filter", filter);
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

//function to search Walk-in Clinic by address
function searchWalkInClinicByAddress() {
    //declare variables
    var inputTime, filter, table, tr, td, tdTime, i, txtValue, txtValueTime, filterTime;
    input = document.getElementById("filter-walkin-adddres");
    inputTime = document.getElementById("filter-wait-time");
    filter = input.value.toUpperCase();
    filterTime = inputTime.Value;
    table = document.getElementById("data-table-walkinclinic");
    tr = table.getElementsByTagName("tr");


    //loop through all table rows and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        tdTime = tr[i].getElementsByTagName("td")[2];
        if (td) {
            txtValue = td.textContent || td.innerHTML;
            txtValueTime = tdTime.textContent || tdTime.innerHTML;
          
            if (filterTime > 0 ) {
              
                if (parseInt(txtValueTime) <= parseInt(filterTime) && txtValue.toUpperCase().indexOf(filter) > -1) {

                    tr[i].style.display = "";
                } else {

                    tr[i].style.display = "none";
                }

            } else {
              
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                } else {
                    tr[i].style.display = "none";
                }
            }
        }
    }
}

//function to search Walk-in Clinic by address
function searchWalkInClinicByWT() {
    //declare variables
    var input, filter, filterAddress, table, tr, td, tdAddress, i, txtValue, txtValueAddress;
    input = document.getElementById("filter-wait-time");
    filter = input.value;
    table = document.getElementById("data-table-walkinclinic");
    tr = table.getElementsByTagName("tr");

    inputAddress = document.getElementById("filter-walkin-adddres");
    filterAddress = inputAddress.value.toUpperCase();

    //loop through all table rows and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[2];
        tdAddress = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerHTML;
            txtValueAddress = tdAddress.textContent || tdAddress.innerHTML;
            if (inputAddress.value != "") {               
                 
                if (parseInt(txtValue) <= parseInt(filter) && txtValueAddress.toUpperCase().indexOf(filterAddress) > -1) {
                    
                    tr[i].style.display = "";
                } else {
                   
                    tr[i].style.display = "none";
                }
            }
            else {
                
                if (parseInt(txtValue) <= parseInt(filter)) {
                    tr[i].style.display = "";
                } else {
                    tr[i].style.display = "none";
                }
            }
        }
    }
}


//add the offices id to input
function addOfficePractitioner(button) {
   
    var id = button.id;
    var idclean = button.id.replace("offic-", "");
    var valueText = button.textContent;

    var btnClicked = document.getElementById(id);

    var inputListOffices = document.getElementById('list-id-offices');



    if (valueText == "Add") {
        btnClicked.textContent = "Remove";
        arrayIdOffice.push(idclean);
        btnClicked.classList.remove("btn-primary");
        btnClicked.classList.add("btn-danger");
    } else {
        btnClicked.textContent = "Add";
        arrayIdOffice = arrayIdOffice.filter(item => item != idclean);
        btnClicked.classList.remove("btn-danger");
        btnClicked.classList.add("btn-primary");
    }

    //get the btn save button
    var btnSaveAdre = document.getElementById("btn-save-address");

    //set the list of address id in the input
    if (arrayIdOffice.length > 0) {
        inputListOffices.value = arrayIdOffice.join(',');

        ///verify if exist the btn
        if (btnSaveAdre != undefined) {

            btnSaveAdre.disabled = false;
        }

    } else {

        inputListOffices.value = "";

        if (btnSaveAdre != undefined) {

            btnSaveAdre.disabled = true;
        }
    }

    


}


//add the languages id to input
function addLangPractitioner(button) {

    var id = button.id;
    var idclean = button.id.replace("lang-", "");
    var valueText = button.textContent;

    var btnClicked = document.getElementById(id);

    var inputListLang = document.getElementById('list-id-languages');



    if (valueText == "Add") {
        btnClicked.textContent = "Remove";
        arrayIdLanguage.push(idclean);
        btnClicked.classList.remove("btn-primary");
        btnClicked.classList.add("btn-danger");
    } else {
        btnClicked.textContent = "Add";
        arrayIdLanguage = arrayIdLanguage.filter(item => item != idclean);
        btnClicked.classList.remove("btn-danger");
        btnClicked.classList.add("btn-primary");
    }

    //set the list of lang id in the input
    if (arrayIdLanguage.length > 0) {
        inputListLang.value = arrayIdLanguage.join(',');
    } else {
        inputListLang.value = "";
    }

  
   

}

//function to search languages
function searchLanguage() {
    //declare variables
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("filterInputLang");
    filter = input.value.toUpperCase();
    table = document.getElementById("data-table-languages");
    tr = table.getElementsByTagName("tr");


    //loop through all table rows and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerHTML;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}


$(document).ready(function () {

    $('.btnRemoveAddress').click(function () {
        var parameterValue = $(this).data('parameter');

        var idRow = "#offic-tr-" + parameterValue;
        var idOfficeNameRow = "#office-name-" + parameterValue;

        
        var contentOfficeName = $(idOfficeNameRow).text();

        

        $.ajax({
            url: 'http://localhost:5220/RemovePractitionerAddress/' + parameterValue,
            type: 'GET',
            success: function (response) {
                // Handle the response data



                $(idRow).remove();



                var finalName = contentOfficeName + " has been removed "

                $('#message-home-pract').text(finalName);
                $('#message-home-pract2').text(finalName);
                console.log(response);


            },
            error: function (xhr, status, error) {
                // Handle errors

                console.error('AJAX request failed:', error);
            }
        });
    });

    $('#btn-add-address').click(function () {

        $.ajax({
            url: 'http://localhost:5220/ListOffices',
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                $.each(data, function (index, item) {

                    var newRow = "<tr class='mx-2'><td>" + item.officeName + "</td><td>" + item.officeTypeName + "</td><td>" + item.fullAddress + "</td><td><button id='offic-" + item.id + "' type='button' class='btn btn-primary mr-1 w-75' onclick='addOfficePractitioner(this)'>Add</button></td></tr>";
                    $("#data-table-add-offices tbody").append(newRow);
                });
            }
        });
    });




    $('.btnShowDetails').click(function () {
        var parameterValue = $(this).data('parameter');




        $.ajax({
            url: 'http://localhost:5220/PractitionerDetails/' + parameterValue,
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)
             

                $('#phone-pract').text(data.phoneNumber);
                $('#gender-pract').text(data.gender);
                $('#rating-pract').text(data.rating);
                $('#patient-acep-pract').text(data.isAcceptingNewPatients);
                $('#patient-last-pract').text(data.lastAcceptedPatientDate);
                $('#pract-type').text(data.practitionerType);
                $('#pract-lang').text(data.practitionerSpokenLanguages);
                $('#practemail').text(data.email);
                $('#id-patient').val(data.patientId);
                $('#id-pract').val(data.id);

                $("#searchpractoffices tbody").empty();

                $.each(data.officeAddresses, function (index, item) {

                    if (item.isRequested == true) {
                        var newRow = "<tr id='row-offic-pract-" + item.id + "' class='mx-2'><td>" + item.officeName + "</td><td>" + item.officeTypeName + "</td><td>" + item.fullAddress + "</td><td>" + item.dateRequest + "</td><td>" + item.statusRequest + "</td><td><button id='offic-pract-" + item.id + "' type='button' class='btn btn-danger mr-1 w-75' onclick='RequestPractitioner(this)'>Remove Request</button></td></tr>";
                    } else {
                        var newRow = "<tr id='row-offic-pract-" + item.id + "' class='mx-2'><td>" + item.officeName + "</td><td>" + item.officeTypeName + "</td><td>" + item.fullAddress + "</td><td>" + item.dateRequest + "</td><td>" + item.statusRequest + "</td><td><button id='offic-pract-" + item.id + "' type='button' class='btn btn-primary mr-1 w-75' onclick='RequestPractitioner(this)'>Add Request</button></td></tr>";
                    }


                    $("#searchpractoffices tbody").append(newRow);

                });

                $('#practdetails').modal('show');

            }
        });
    });

    

    $('.btnPatientRequests').click(function () {
        var parameterValue = $(this).data('parameter');




        $.ajax({
            url: 'http://localhost:5220/ViewNewPatientRequests/' + parameterValue,
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)

                                            

                $("#data-table-patient-requests tbody").empty();

                
                $.each(data, function (index, item) {

                

                    var newRow = "<tr id='row-patient-req-" + item.id + "' class='mx-2'><td>" + item.fullname + "</td><td>" + item.age + "</td><td>" + item.gender + "</td><td>" + item.fullAddress + "</td><td id='address-patient-req-" + item.officeId + "'>" + item.officeName + "</td><td>" + item.dateRequest + "</td>"
                    newRow += "<td id='" + parameterValue + "-" + item.id + "'>";
                    newRow += "<select id='action-pract-" + item.officeId + "' class='select-update-pat' name='action-pract' onchange='checkSelectedValues(this)'><option selected value='pending'>Pending</option><option value='waitlist'>WaitList</option><option value='approve' >Approve</option><option value='deny' >Deny</option></select>";
                    newRow += "</td ></tr>";
                   
                   
                    $("#data-table-patient-requests tbody").append(newRow);


                 

                });

                $(".select-update-pat").val(parameterValue);

                $('#patientrequests').modal('show');

            }
        });
    });

    $('#btnPatientWaitListRequest').click(function () {
        var parameterValue = $(this).data('parameter');




        $.ajax({
            url: 'http://localhost:5220/ViewPatientRequests/' + parameterValue,
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)



                $("#data-table-patient-home-requests tbody").empty();


                $.each(data, function (index, item) {



                    var newRow = "<tr id='rowcanc-" + item.id + "-" + item.practictionerId + "-" + item.officeId + "' class='mx-2'><td id='patient-req-canc-" + item.practictionerId + "' ><button id='" + item.practEmail + "' type='button' class='btn btn-link mr-1 w-100' onclick='showPractDetails(this)'>" + item.practictionerFullname + "</button></td><td id='patient-cancel-" + item.id + "'>" + item.practictionerType + "</td><td id='address-patient-home-" + item.officeId + "'>" + item.officeName + "</td><td>" + item.officeAddress + "</td><td>" + item.dateRequest + "</td>"
                    newRow += "<td id='" + parameterValue + "-" + item.practictionerId + "'>";
                    newRow += "<button id='canc-" + item.id + "-" + item.practictionerId + "-" + item.officeId + "' type='button' class='btn btn-danger mr-1 w-100' onclick='setIdRequesttoCancel(this)'>Cancel Request</button>";
                    newRow += "</td ></tr>";


                    $("#data-table-patient-home-requests tbody").append(newRow);




                });

                $(".select-update-pat").val(parameterValue);

                $('#patienthomerequests').modal('show');

            }
        });
    });

    $('#btnPatientApproved').click(function () {
        var parameterValue = $(this).data('parameter');




        $.ajax({
            url: 'http://localhost:5220/ViewPatientRequests/' + parameterValue,
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)



                $("#data-table-patient-home-approved tbody").empty();


                $.each(data, function (index, item) {



                    var newRow = "<tr id='rowcanc-" + item.id + "-" + item.practictionerId + "-" + item.officeId + "' class='mx-2'><td id='patient-req-canc-" + item.practictionerId + "' ><button id='" + item.practEmail + "' type='button' class='btn btn-link mr-1 w-100' onclick='showPractDetails(this)'>" + item.practictionerFullname + "</button></td><td id='patient-cancel-" + item.id + "'>" + item.practictionerType + "</td><td id='address-patient-home-" + item.officeId + "'>" + item.officeName + "</td><td>" + item.officeAddress + "</td><td>" + item.dateRequest + "</td><td>" + item.dateApproved + "</td>"
                    newRow += "<td id='" + parameterValue + "-" + item.practictionerId + "'>";
                    newRow += "</td ></tr>";


                    $("#data-table-patient-home-approved tbody").append(newRow);




                });

                $(".select-update-pat").val(parameterValue);

                $('#patienthomerequestsapproved').modal('show');

            }
        });
    });

    
    $('#btnPatientaApproved').click(function () {
        var parameterValue = $(this).data('parameter');




        $.ajax({
            url: 'http://localhost:5220/ViewNewPatientRequests/' + parameterValue,
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)



                $("#data-table-patient-approved tbody").empty();


                $.each(data, function (index, item) {



                    var newRow = "<tr id='row-patient-req-rem" + item.id + "' class='mx-2'><td>" + item.fullname + "</td><td>" + item.age + "</td><td>" + item.gender + "</td><td>" + item.fullAddress + "</td><td id='address-patient-req-" + item.officeId + "'>" + item.officeName + "</td><td>" + item.dateRequest + "</td><td>" + item.dateApproved + "</td>"
                    newRow += "<td>";
                    newRow += "<button id='offic-pract-rem-" + item.id + "' type='button' class='btn btn-danger mr-1 w-100' onclick='setIdPatientDelete(this)'>Remove</button>";
                    newRow += "</td ></tr>";
 

                    $("#data-table-patient-approved tbody").append(newRow);




                });

              
                $('#patientrequestsapproved').modal('show');

            }
        });
    });

    $('#btnAllOffices').click(function () {
       
        $.ajax({
            url: 'http://localhost:5220/ListAllOffices/',
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)



                $("#data-table-all-offices tbody").empty();


                $.each(data, function (index, item) {



                    var newRow = "<tr id='row-offic-pract-" + item.id + "' class='mx-2'><td>" + item.officeName + "</td><td>" + item.officeTypeName + "</td><td>" + item.fullAddress + "</td></tr>";


                    $("#data-table-all-offices tbody").append(newRow);




                });


                $('#systemoffices').modal('show');

            }
        });
    });

    $('#btn-add-office').click(function () {

        $.ajax({
            url: 'http://localhost:5220/ListAllOfficesTypes/',
            method: 'GET',
            dataType: 'json',
            success: function (data) {

                console.log("Data");
                console.log(data)



                $("#officeType-add").empty();

                var firstOption = "<option class= 'custom-select' disabled selected value='noselected' >Select a Office Type</option >";

                $("#officeType-add").append(firstOption);

                $.each(data, function (index, item) {



                    var newOption = "<option value='" + item.id + "'>" + item.officeTypeName + "</option>";


                    $("#officeType-add").append(newOption);




                });


                $('#systemaddoffices').modal('show');

            }
        });
    });

   



});

function showPractDetails(button) {
    console.log("paso");
    console.log(button.id);
    var parameterValue = $(button).data('parameter');
    $.ajax({
        url: 'http://localhost:5220/PractitionerDetails/' + button.id,
        method: 'GET',
        dataType: 'json',
        success: function (data) {

            console.log("Data");
            console.log(data)

            $('#pract-fullname').text(data.firstName + " " + data.lastName);
            $('#phone-pract').text(data.phoneNumber);
            $('#gender-pract').text(data.gender);
            $('#rating-pract').text(data.rating);
            $('#patient-acep-pract').text(data.isAcceptingNewPatients);
            $('#patient-last-pract').text(data.lastAcceptedPatientDate);
            $('#pract-type').text(data.practitionerType);
            $('#pract-lang').text(data.practitionerSpokenLanguages);
            $('#practemail').text(data.email);
            $('#id-patient').val(data.patientId);
            $('#id-pract').val(data.id);

            console.log("paso 2");
            
            $('#pract-details-patientpage').modal('show');

        }
    });

}

function openAddModal() {

    $('#systemoffices').modal('show');

}

//add request patient for practictioner by offices id to input
function RequestPractitioner(button) {

    var id = button.id;
    //get the parent row
    var row = button.parentNode.parentNode;
    //get an array pf td elements in the row
    var cells = row.getElementsByTagName("td");

    var idclean = button.id.replace("offic-pract-", "");
    var valueText = button.textContent;
    var btnClicked = document.getElementById(id);

    var inputListAddRequest = document.getElementById('id-office-add');
    var inputListRemoveRequest = document.getElementById('id-office-remove');

    if (valueText == "Add Request") {

        btnClicked.textContent = "Remove Request";
        btnClicked.classList.remove("btn-primary");
        btnClicked.classList.add("btn-danger");

        ;

        if (arrayIdRemoveRequestPract.includes(idclean)) {

            arrayIdRemoveRequestPract = arrayIdRemoveRequestPract.filter(item => item != idclean);




        } else {

            arrayIdAddRequestPract.push(idclean);



        }



    } else {


        btnClicked.textContent = "Add Request";
        btnClicked.classList.remove("btn-danger");
        btnClicked.classList.add("btn-primary");

        //remove the values in the dateRequest and status
        cells[3].innerHTML = "";
        cells[4].innerHTML = "";

        if (arrayIdAddRequestPract.includes(idclean)) {

            arrayIdAddRequestPract = arrayIdAddRequestPract.filter(item => item != idclean);



        } else {

            arrayIdRemoveRequestPract.push(idclean);



        }

    }
     

    inputListRemoveRequest.value = arrayIdRemoveRequestPract.join(",");
    inputListAddRequest.value = arrayIdAddRequestPract.join(",");

    //disable save button in the Practictioner Home Page
    // var btnSaveRequest = document.getElementById("btn-save-address");


    if (arrayIdAddRequestPract.length > 0 || arrayIdRemoveRequestPract.length > 0) {

        btnSaveRequest.disabled = false;
    }
    else {
        btnSaveRequest.disabled = true;
    }




}


//function to get patient id and status to update status of patient request for practictioner
//receive a aaray in format atring with patient id and status
function updatePatientNewRequest() {

    // Get the table element
    var table = document.getElementById("data-table-patient-requests");

    // Get all the rows in the table
    var rows = table.getElementsByTagName("tr");

    arrayPatientUpdateNewRequest = [];
       

    // Iterate over each row and get its ID
    for (var i = 1; i < rows.length; i++) {

        var row = rows[i];

        //get all the cells
        var cells = rows[i].getElementsByTagName("td");
          
        //var to stpre each value of array
        var itemArray = '';        

        // Get the ID of the row
        var rowID = row.id;
        var rowIDClean = rowID.replace("row-patient-req-", "");

        // Get the ID of the cell that contain the office name
        var cellOfficeId = cells[4].id
        var officeID = cellOfficeId.replace("address-patient-req-", "");
        // Output the the row
       
        // Get the ID of the cell that contain the select to obtain the status 
        var cellSelectId = cells[6].id
               
        //split the id to get the status 
        var statusSelec = cellSelectId.split("-");

        //get the status from the first array element
        statusSelec = statusSelec[0];

        ///set the Select id
        var idSelect = "action-pract-" + rowIDClean;

        // Get the select element
        var selectElement = document.getElementById(idSelect);
        console.log("Paso aqi")
      
        // Get the select element inside the table cell
        var selectElement = cells[6].querySelector("select");
      
        // Get the selected option
        //var selectedOption = selectElement.options[selectElement.selectedIndex];

        // Get the value of the selected option 
        var selectedValue = selectElement.value;

        itemArray = rowIDClean + "-" + selectedValue + "-" + officeID;

     
        //statusSelec = status selected as parameter  
        //verify if the modified some status
        if (selectedValue != statusSelec) {
            console.log("Paso aqui");
            // Add the selected value to the array
            arrayPatientUpdateNewRequest.push(itemArray);

        }

        //get the input to store all the patient IDs and status to update
        var inputStatusUpdate = document.getElementById("status-pat-req");  

        //get the form that send the http post request to save the patient IDs and status to update
        var formUpdatePatientRequest = document.getElementById("update-req-patient");

        if (arrayPatientUpdateNewRequest.length > 0) {
            btnSaveRequestNewPat.disabled = false;
        }

        //set the values all the patient IDs and status to update
        inputStatusUpdate.value = arrayPatientUpdateNewRequest.join(",");


        // Display the array contents
        console.log("Selected values: ");
        
        console.log(arrayPatientUpdateNewRequest);
      


        //send the form
        formUpdatePatientRequest.submit();

       
    }
}

//#########################continue here ##########################################
//function to get patient id and status to update status to removed of patient request for practictioner
//receive a array in format string with patient id and status
function removePatientToPractitioner(id) {

  

    // Output the the idCleam
   
    var idclean = id.replace("offic-pract-rem-", "");

    // Output the the idCleam

    // Display the array contents
    console.log("Selected values: ", id);

    var idRowClicked = "row-patient-req-rem" + idclean;

    var rowClicked = document.getElementById(idRowClicked);

    //get all the cells
    var cells = rowClicked.getElementsByTagName("td");

    //var to stpre each value of array
    var itemArray = '';
        
    // Get the ID of the cell that contain the office name
    var cellOfficeId = cells[4].id
    var officeID = cellOfficeId.replace("address-patient-req-", "");
   

    itemArray = idclean + "-removed" + "-" + officeID;


    // Add the selected value to the array
    arrayPatientUpdateNewRequest.push(itemArray);

    //get the input to store all the patient IDs and status to update
    var inputStatusUpdate = document.getElementById("status-pat-req-remove");

    //get the form that send the http post request to save the patient IDs and status to update
    var formUpdatePatientRequest = document.getElementById("update-req-patient-remove");

    //set the values all the patient IDs and status to update
    inputStatusUpdate.value = arrayPatientUpdateNewRequest.join(",");


    //send the form
    formUpdatePatientRequest.submit();
}

function cancelPatientRequest(id) {

     
    // Display the array contents
    console.log("Selected values: ", id);   
    var idRowClicked = "row" + id;

    var rowClicked = document.getElementById(idRowClicked);

    //get all the cells
    var cells = rowClicked.getElementsByTagName("td");

    //var to stpre each value of array
    var itemArray = '';

    // Get the ID of the cell that contain the office name
    var cellOfficeId = cells[2].id;
    var cellPatientId = cells[1].id;
    var PatientId = cellPatientId.replace("patient-cancel-", "");
    
    var practID = cells[0].id;
    var practIDClean = practID.replace("patient-req-canc-", "");
    var officeID = cellOfficeId.replace("address-patient-home-", "");

    //idClean = id practictioner
    itemArray = PatientId + "-canceled" + "-" + officeID + "-" + practIDClean;
     
    // Add the selected value to the array
    arrayPatientUpdateNewRequest.push(itemArray);

    //get the input to store all the patient IDs and status to update
    var inputStatusUpdate = document.getElementById("status-pat-req-remove");

    //get the form that send the http post request to save the patient IDs and status to update
    var formUpdatePatientRequest = document.getElementById("update-patient-cancel-req");

    //set the values all the patient IDs and status to update
    inputStatusUpdate.value = arrayPatientUpdateNewRequest.join(",");


    //send the form
    formUpdatePatientRequest.submit();
}

//fuction to enable or disable save button to update a new pract request
function checkSelectedValues(select) {

   
    //set the current value of select selected
    valueSelectNewPatientRequest = select.value;

    console.log("valor select");
    console.log(valueSelectNewPatientRequest)

    var allSelected = true;
    var selectElements = document.getElementsByClassName("select-update-pat");
   
    for (var i = 0; i < selectElements.length; i++) {
     
        if (selectElements[i].value !== "pending") {
            allSelected = false;
            break; // Exit the loop early if the value is not selected
        }
    }

    // Enable or disable the button based on whether the specific value is selected in all select elements
    document.getElementById("btn-save-request-pat").disabled = allSelected;



    
}

//set the id of the button after corfirmation to remove the patient
function setIdPatientDelete(button) {
    // Get the button element
    var buttonDelete = document.getElementById("btn-delete-patient");

    // Set the onclick attribute with the new dynamic value
    buttonDelete.setAttribute("onclick", "removePatientToPractitioner('" + button.id + "')");

    $('#confirmdelete').modal('show');
}

//set the id of the button after corfirmation to cancel the patient request in the home page patient
function setIdRequesttoCancel(button) {
    // Get the button element
    var buttonDelete = document.getElementById("btn-cancel-request");

    // Set the onclick attribute with the new dynamic value
    buttonDelete.setAttribute("onclick", "cancelPatientRequest('" + button.id + "')");

    $('#confirmcancelrequest').modal('show');
}

//function to valifate and add a new address
function addOfficeAddress() {

    //grl all th elements in the form
    var officeName = document.getElementById("officename-add").value;
    var typeOffice = document.getElementById("officeType-add").value;
    var street = document.getElementById("street-address-add").value;
    var city = document.getElementById("city-add").value;
    var province = document.getElementById("province-add").value;
    var zone = document.getElementById("zone-add").value;
    var postalCode = document.getElementById("postalcode-add").value;
    var country = document.getElementById("countryu-add").value;

    //get the element to show the errors
    var formAddress = document.getElementById("frm-add-office-address");

    //set the arry to store all the erros
    var errorMessage = [];


    if (officeName == undefined || officeName == "" || officeName == null) {

        errorMessage.push("Please enter Office name");
    }
    if (typeOffice == "noselected") {

        errorMessage.push("Please enter Office type");
    }
    if (street == undefined || street == "" || street == null) {

        errorMessage.push("Please enter Street");
    }
    if (city == undefined || city == "" || city == null) {

        errorMessage.push("Please enter City");
    }
    if (zone == undefined || zone == "" || zone == null) {

        errorMessage.push("Please enter Zone");
    }
    if (province == "noselected") {

        errorMessage.push("Please enter Province");
    }
    if (postalCode == undefined || postalCode == "" || postalCode == null) {

        errorMessage.push("Please enter Postal Code");
    }
    if (country == undefined || country == "" || country == null) {

        errorMessage.push("Please enter Country");
    }

    console.log("result errors");
    console.log(errorMessage);

    $("#div-messages").empty();

    if (errorMessage.length > 0) {

        errorMessage.forEach(item => {

            var elementP = "<p class='my-0 py-0'>" + item + "</p>";

            $("#div-messages").append(elementP);
            $("#div-messages").show();

        });
    }
    else {
        //send form to server
        formAddress.submit();

    }
     

}

function checkInPatientValidation() {

    //get all the elements in the form
    var firstName = document.getElementById("first-Name").value.trim();
    var lastName = document.getElementById("last-Name").value.trim();

    //get the element to show the errors
    var formCheckInPatient = document.getElementById("frm-check-patient-in");

    //set the arry to store all the erros
    var errorMessage = [];


    if (firstName == undefined || firstName == "" || firstName == null) {

        errorMessage.push("Please enter the patient's first name");
    }
    if (lastName == undefined || lastName == "" || lastName == null) {

        errorMessage.push("Please enter the patient's last name");
    }
    
    console.log("result errors");
    console.log(errorMessage);

    $("#div-check-in-error-messages").empty();

    if (errorMessage.length > 0) {

        errorMessage.forEach(item => {

            var elementP = "<p class='my-0 py-0'>" + item + "</p>";

            $("#div-check-in-error-messages").append(elementP);
            $("#div-check-in-error-messages").show();

        });
    }
    else {
        //send form to server
        formCheckInPatient.submit();
    }


}

function currentPracReviewValidation() {
    var rating = document.getElementById("currentPracRating").value;
    var form = document.getElementById("frm-add-current-prac-review")
    var errorMessage = "";

    if (rating == 0) {
        errorMessage = "Please rate the practitioner";
    }

    console.log("result errors");
    console.log(errorMessage);

    $("div-current-prac-review-error-messages").empty();

    if (errorMessage.length !== "") {
        var elementP = "<p class='my-0 py-0'>" + errorMessage + "</p>";

        $("div-current-prac-review-error-messages").append(elementP);
        $("div-current-prac-review-error-messages").show();
        
    }
    else {
        form.submit();
    }
}

function pastPracReviewValidation() {
    var rating = document.getElementById("pastPracRating").value;
    var errorMessage = "";

    if (rating == 0) {
        errorMessage = "Please rate the practitioner";
    }

    console.log("result errors");
    console.log(errorMessage);

    $("div-past-prac-review-error-messages").empty();

    if (errorMessage.length === "") {
        var elementP = "<p class='my-0 py-0'>" + errorMessage + "</p>";

        $("div-past-prac-review-error-messages").append(elementP);
        $("div-past-prac-review-error-messages").show();
    }
    else {
        document.getElementById("frm-add-past-prac-review").submit();
    }
}

