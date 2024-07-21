var datatable;
$(document).ready(function () {
    loaddatatable();
});

function loaddatatable() {
    datatable = $("#tabledata").DataTable({
        "ajax": {
            "url": "/Exam/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "className", "width": "20%" },
            { "data": "subjectName", "width": "20%" },
            { "data": "studentName", "width": "20%" },
            { "data": "totalMarks", "width": "10%" },
            { "data": "outOfMarks", "width": "10%" },
            {
                "data": "examId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-primary" href="/Exam/SaveOrUpdate?examId=${data}"><i class="fas fa-edit"></i></a>
                            <a class="btn btn-danger" onclick="Delete('/Exam/Delete?examId=${data}')"><i class="fas fa-trash"></i></a>
                        </div>
                    `;
                }, "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        text: "Delete Exam",
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
