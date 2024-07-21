var datatable;

$(document).ready(function () {
    loadDatatable();
});

function loadDatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "/StudentAttendance/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "className", "width": "15%" },
            { "data": "subjectName", "width": "15%" },
            { "data": "rollNo", "width": "15%" },
            { "data": "status", "width": "15%" },
            { "data": "date", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-primary" href="/StudentAttendance/SaveOrUpdate?id=${data}">
                                <i class="fas fa-edit"></i>
                            </a>
                             <a class="btn btn-danger" onclick="Delete('/StudentAttendance/Delete?id=${data}')">
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