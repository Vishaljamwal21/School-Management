var datatable;

$(document).ready(function () {
    loaddatatable();
});

function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "/Student/GetAll",  // Ensure this matches your route
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            {
                "data": "dob",
                "width": "15%",
                "render": function (data) {
                    return data ? moment(data).format('YYYY-MM-DD') : "N/A";
                }
            },
            { "data": "gender", "width": "10%" },
            { "data": "mobile", "width": "15%" },
            { "data": "rollNo", "width": "10%" },
            { "data": "address", "width": "15%" },
            { "data": "className", "width": "10%" },  // Assuming Class has a ClassName property
            {
                "data": "studentId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-primary" href="/Student/SaveOrUpdate?Id=${data}">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger" onclick="Delete('/Student/Delete?Id=${data}')">
                                <i class="fas fa-trash"></i>
                            </a>
                        </div>
                    `;
                },
                "width": "10%"
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
