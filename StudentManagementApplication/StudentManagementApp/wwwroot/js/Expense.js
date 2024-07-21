var datatable;
$(document).ready(function () {
    loaddatatable();
});

function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "/Expense/GetAll", // Update URL to Expense controller
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "className", "width": "20%" }, // Update data properties
            { "data": "subjectName", "width": "20%" },
            { "data": "chargeAmount", "width": "20%" }, // Change to chargeAmount for Expense
            {
                "data": "expenseId", // Change to expenseId for Expense
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-primary" href="/Expense/SaveOrUpdate?expenseId=${data}"><i class="fas fa-edit"></i></a>
                            <a class="btn btn-danger" onclick="Delete('/Expense/Delete?expenseId=${data}')"><i class="fas fa-trash"></i></a>
                        </div>
                    `;
                }, "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        text: "Delete Expense", // Update message for Expense
        title: "Do you want to delete the information?",
        buttons: true,
        icon: "warning",
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
