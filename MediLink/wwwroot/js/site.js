// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let arrayIdOffice = [];
let arrayIdLanguage = [];

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
            console.log("paso")
            console.log(filterTime)
            if (filterTime > 0 ) {
                console.log("paso 1")
                if (parseInt(txtValueTime) <= parseInt(filterTime) && txtValue.toUpperCase().indexOf(filter) > -1) {

                    tr[i].style.display = "";
                } else {

                    tr[i].style.display = "none";
                }

            } else {
                console.log("paso 2")
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

    //set the list of address id in the input
    if (arrayIdOffice.length > 0) {
        inputListOffices.value = arrayIdOffice.join(',');
    } else {
        inputListOffices.value = "";
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

    console.log("array languages")
    console.log(arrayIdLanguage);
   

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
