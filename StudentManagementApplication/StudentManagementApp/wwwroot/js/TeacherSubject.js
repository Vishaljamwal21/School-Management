var datatable;

$(document).ready(function () {
    loaddatatable();
});

function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "/TeacherSubject/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "teacherName", "width": "25%" },
            { "data": "className", "width": "25%" },
            { "data": "subjectName", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-primary" href="/TeacherSubject/SaveOrUpdate?Id=1">
                             <i class="fas fa-edit"></i>
                            </a>

                            <a class="btn btn-danger" onclick="Delete('/TeacherSubject/Delete?Id=${data}')">
                                <i class="fas fa-trash"></i>
                            </a>
                        </div>
                    `;
                },
                "width": "25%"
            }
        ]
    });
}


function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this data!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
