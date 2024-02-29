// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let arrayIdOffice = [];

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
