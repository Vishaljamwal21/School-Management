var datatable;
$(document).ready(function () {
    loaddatatable();
})
function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "Subject/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "className", "width": "25%" },
            { "data": "subjectName", "width": "25%" },
            {
                "data": "subjectId",
                "render": function (data) {
                    return `
                             <div class="text-center">
                            <a class="btn btn-primary" href="/Subject/SaveOrUpdate?subjectId=${data}"><i class="fas fa-edit"></i></a>
                            <a class="btn btn-danger" onclick="Delete('/Subject/Delete?subjectId=${data}')"><i class="fas fa-trash"></i></a>
                        </div>
                            `;
                }
            }
        ]
    })
}
function Delete(url) {
    swal({
        text: "Delete Subject",
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
                        toastr.success(data.message); 
                    }
                    else {
                        toastr.error(data.message); 
                    }
                }
            })
        }
    })
}
