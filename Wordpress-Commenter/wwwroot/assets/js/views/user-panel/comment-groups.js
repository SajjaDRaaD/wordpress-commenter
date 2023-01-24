"use strict";


// Add Form Validation
const AddGroupform = document.getElementById('add-comment-group-form');
var AddFormValidator = FormValidation.formValidation(
    AddGroupform,
    {
        fields: {
            'CommentGroup.Name': {
                validators: {
                    notEmpty: {
                        message: 'نام گروه الزامی است'
                    }
                }
            },
            'CommentGroup.Type': {
                validators: {
                    notEmpty: {
                        message: 'نوع گروه الزامی است'
                    },
                }
            },
        },

        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({
                rowSelector: '.fv-row',
                eleInvalidClass: '',
                eleValidClass: ''
            })
        }
    }

);

// Edit Form Validation
const EditGroupform = document.getElementById('editGroupForm');
var EditFormValidator = FormValidation.formValidation(
    EditGroupform,
    {
        fields: {
            'Name': {
                validators: {
                    notEmpty: {
                        message: 'نام گروه الزامی است'
                    }
                }
            },
            'Type': {
                validators: {
                    notEmpty: {
                        message: 'نوع گروه الزامی است'
                    },
                }
            },
        },

        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({
                rowSelector: '.fv-row',
                eleInvalidClass: '',
                eleValidClass: ''
            })
        }
    }

);

// Add Group
$(AddGroupform).submit(function (e) {
    e.preventDefault();

    if (AddFormValidator) {
        AddFormValidator.validate().then(function (status) {
            if (status == "Valid") {
                AddGroupform.submit();
            }
        })
    }

})

// Delete Group
$(".delete-group").on("click", function (e) {

    e.preventDefault();

    let id = $(this).attr("data-id");


    Swal.fire({
        title: 'آیا از حذف این گروه اطمینان دارید ؟',
        text: "تمامی کامنت های زیر مجموعه این گروه حذف خواهد شد!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'انصراف',
        confirmButtonText: 'بله، حذف شود'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: "/userpanel/commentgroups/",
                data: { Id: id },
                success: function (msg) {
                    if (msg == false) {
                        swal.fire({
                            title: 'خطا',
                            text: "ارسال فعالی با این گروه در حال انجام است. تا اتمام ارسال قادر به حذف گروه نمی باشید.",
                            icon: 'error',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'متوجه شدم'
                        })
                    } else {
                        location.reload();
                    }
                },
                error: function (req, status, error) {
                    swal.fire({
                        title: 'خطا',
                        text: "جهت بررسی خطا با پشتیبانی در ارتباط باشید!",
                        icon: 'error',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'متوجه شدم'
                    })
                    console.log(req, status, error)
                }
            });
        }
    })
})

// Bind data on Edit modal
$(".edit-group-btn").on("click", function (e) {

    e.preventDefault();
    let id = $(this).attr('data-id');

    $.ajax({
        type: "POST",
        url: "/userpanel/GetCommentGroup/",
        data: { Id: id },
        dataType: "json",
        success: function (result) {
            let editModal = $("#edit-comment-group-modal");

            editModal.find('input[name="Id"]').val(result.id);
            editModal.find('input[name="Name"]').val(result.name);
            editModal.find(`select[name="Type"] option[value="${result.type}"]`).attr("selected", "selected");
            editModal.find('textarea[name="Description"]').val(result.description);

        },
        error: function (error) {
            swal.fire({
                title: 'خطا',
                text: "جهت بررسی خطا با پشتیبانی در ارتباط باشید!",
                icon: 'error',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'متوجه شدم'
            })
        }
    })

})

// Edit Group
$(".edit-group-submit").on("click",function (e) {

    e.preventDefault();

    if (EditFormValidator) {
        EditFormValidator.validate().then(function (status) {
            console.log(status);
            if (status == "Valid") {
                var group = $("#editGroupForm").serialize();

                $.ajax({
                    type: "POST",
                    url: "/userpanel/EditCommentGroup/",
                    data: group,
                    success: function (result) {
                        location.reload();
                    },
                    error: function (error) {
                        swal.fire({
                            title: 'خطا',
                            text: "جهت بررسی خطا با پشتیبانی در ارتباط باشید!",
                            icon: 'error',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'متوجه شدم'
                        })
                    }
                })
            }
        });
    }

})

// Class definition
var CommentGroupDataTable = function () {
    // Shared variables
    var table;
    var datatable;

    // Private functions
    var initDatatable = function () {

        datatable = $(table).DataTable({
            language: {
                url: "/assets/language/data-tables-fa.json",
            },
            info: false,
            ordering : false,
        });
    }


    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('#comment-groups-tbl');
            initDatatable();
            handleSearchDatatable();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    CommentGroupDataTable.init();
});




