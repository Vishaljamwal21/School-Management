var datatable;
$(document).ready(function () {
    loaddatatable();
})
function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "Class/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [          
            { "data": "className", "width": "50%" },
            {
                "data": "classId",
                "render": function (data) {
                    return `
                           <div class="text-center">
                                <a class="btn btn-primary" href="/Class/SaveOrUpdate?classId=${data}"><i class="fas fa-edit"></i></a>
                                <a class="btn btn-danger" onclick="Delete('/Class/Delete?classId=${data}')"><i class="fas fa-trash"></i></a>
                            </div>
                            `;
                }
            }
        ]
    })
}
function Delete(url) {
    swal({
        text: "Delete Class",
        title: "Do you want to delete the information",
        buttons: true,
        icon: "warning",
        dangermodel: true
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                        toastr.success(data.message); // Corrected from messege to message
                    }
                    else {
                        toastr.error(data.message); // Corrected from messege to message
                    }
                }
            })
        }
    })
}
