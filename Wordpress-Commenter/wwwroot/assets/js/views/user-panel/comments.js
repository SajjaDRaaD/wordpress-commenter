"use strict";

// Class definition
var ReviewsDataTable = function () {
    // Shared variables
    var table;
    var datatable;

    // Private functions
    var initDatatable = function () {

        datatable = $(table).DataTable({
            language: {
                url : "/assets/language/data-tables-fa.json"
            },
            "info": false,
            "ordering": false,
        });
    }

    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="review-search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('#reviews-tbl');

            if (!table) {
                return;
            }

            initDatatable();
            handleSearchDatatable();
        }
    };
}();

// Class definition
var CommentsDataTable = function () {
    // Shared variables
    var table;
    var datatable;

    // Private functions
    var initDatatable = function () {

        datatable = $(table).DataTable({
            language: {
                url: "/assets/language/data-tables-fa.json"
            },
            "info": false,
            "ordering": false,
        });
    }

    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="comment-search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('#comments-tbl');

            if (!table) {
                return;
            }

            initDatatable();
            handleSearchDatatable();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    ReviewsDataTable.init();
    CommentsDataTable.init();
});

const reviewForm = document.getElementById("reviewForm");
const commentForm = document.getElementById("commentForm");

var addReviewValidation = FormValidation.formValidation(
    reviewForm,
    {
        fields: {

            'Review.Author': {
                validators: {
                    notEmpty: {
                        message: 'نام نویسنده الزامی است'
                    }
                }
            },
            'Review.CommentGroupId': {
                validators: {
                    notEmpty: {
                        message: 'انتخاب گروه الزامی است'
                    },
                }
            },
            'Review.Body': {
                validators: {
                    notEmpty: {
                        message: 'متن کامنت الزامی است'
                    },
                }
            },
            'Review.Rating': {
                validators: {
                    notEmpty: {
                        message: 'امتیاز محصول الزامی است'
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

var addCommentValidation = FormValidation.formValidation(
    commentForm,
    {
        fields: {

            'Comment.Author': {
                validators: {
                    notEmpty: {
                        message: 'نام نویسنده الزامی است'
                    }
                }
            },
            'Comment.CommentGroupId': {
                validators: {
                    notEmpty: {
                        message: 'انتخاب گروه الزامی است'
                    },
                }
            },
            'Comment.Body': {
                validators: {
                    notEmpty: {
                        message: 'متن کامنت الزامی است'
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

$(commentForm).submit(function (e) {
    e.preventDefault();

    if (addCommentValidation) {
        addCommentValidation.validate().then(function (status) {
            if (status == "Valid") {
                commentForm.submit();
            }
        })
    }
})

$(reviewForm).submit(function (e) {
    e.preventDefault();

    if (addReviewValidation) {
        addReviewValidation.validate().then(function (status) {
            if (status == "Valid") {
                reviewForm.submit();
            }
        })
    }
})

$(".delete-review-btn").on("click", function (e) {

    e.preventDefault();

    let id = $(this).attr("data-id");


    Swal.fire({
        title: 'آیا از حذف این نظر اطمینان دارید ؟',
        text: "این عملیات قابل برگشت نیست!",
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
                url: "/userpanel/deletereview/",
                data: { Id: id },
                dataType: "json",
                success: function (msg) {
                    location.reload();
                },
                error: function (req, status, error) {
                    swal.fire({
                        title: 'خطا',
                        text: "جهت بررسی خطا با پشتیبانی در ارتباط باشید!",
                        icon: 'error',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'متوجه شدم'
                    })
                }
            });
        }
    })

})

$(".delete-comment-btn").on("click", function (e) {

    e.preventDefault();

    let id = $(this).attr("data-id");


    Swal.fire({
        title: 'آیا از حذف این نظر اطمینان دارید ؟',
        text: "این عملیات قابل برگشت نیست!",
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
                url: "/userpanel/deletecomment/",
                data: { Id: id },
                dataType: "json",
                success: function (msg) {
                    location.reload();
                },
                error: function (req, status, error) {
                    swal.fire({
                        title: 'خطا',
                        text: "جهت بررسی خطا با پشتیبانی در ارتباط باشید!",
                        icon: 'error',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'متوجه شدم'
                    })
                }
            });
        }
    })

})

$(".preview-btn").on("click", function (e) {

    e.preventDefault();

    let mode = $(this).attr("data-mode");
    let id = $(this).attr("data-id");
    let url = mode == "review" ? "/userpanel/getreview" : "/userpanel/getcomment";

    $.ajax({
        type: "post",
        url: url,
        data: { Id: id },
        dataType: "json",
        success: function (result) {

            let stars = $(".rating-label");
            $('.comment-title').text(result.author)
            $('.comment-body').text(result.body)


            if (typeof result.rating !== 'undefined') {
                $('.rating').show();
                for (let i = 0; i < result.rating; i++) {
                    $(stars[i]).addClass("checked");
                }
            } else {
                $('.rating').hide();
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
        }
    });


})