var datatable;

$(document).ready(function () {
    loaddatatable();
});

function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "Teacher/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "dob", "width": "10%" },
            { "data": "gender", "width": "10%" },
            { "data": "mobile", "width": "10%" },
            { "data": "email", "width": "10%" },
            { "data": "address", "width": "10%" },
            { "data": "password", "width": "10%" },
            { "data": "role", "width": "10%" },
            {
                "data": "teacherId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-primary" href="/Teacher/SaveOrUpdate?teacherId=${data}">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger" onclick="Delete('/Teacher/Delete?teacherId=${data}')">
                                <i class="fas fa-trash"></i>
                            </a>
                        </div>
                    `;
                }
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
