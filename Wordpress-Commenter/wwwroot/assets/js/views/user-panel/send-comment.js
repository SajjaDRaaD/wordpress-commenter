// Stepper lement
var element = document.querySelector("#kt_stepper_example_vertical");

// Initialize Stepper
var stepper = new KTStepper(element);

const sendCommentForm = document.getElementById("send-comment-form");

var websiteStepValidation = FormValidation.formValidation(
    sendCommentForm,
    {
        fields: {

            'websiteId': {
                validators: {
                    notEmpty: {
                        message: 'انتخاب وبسایت مقصد الزامی است'
                    }
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

var commentTypeStepValidation = FormValidation.formValidation(
    sendCommentForm,
    {
        fields: {

            'comment-type': {
                validators: {
                    notEmpty: {
                        message: 'انتخاب نوع کامنت الزامی است'
                    }
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

var selectCategoryStepValidation = FormValidation.formValidation(
    sendCommentForm,
    {
        fields: {

            'desCatId': {
                validators: {
                    notEmpty: {
                        message: 'انتخاب دسته بندی مقصد الزامی است'
                    }
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

var commentGroupStepValidation = FormValidation.formValidation(
    sendCommentForm,
    {
        fields: {

            'commentCatId': {
                validators: {
                    notEmpty: {
                        message: 'انتخاب دسته بندی کامنت الزامی است'
                    }
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

//var schudaleStepValidation = FormValidation.formValidation(
//    sendCommentForm,
//    {
//        fields: {

//            'productsPerSendCount': {
//                validators: {
//                    notEmpty: {
//                        message: 'تعداد پست در هر ارسال الزامی است'
//                    }
//                }
//            },
//            'commentsPerProductCount': {
//                validators: {
//                    notEmpty: {
//                        message: 'تعداد کامنت برای هر پست الزامی است'
//                    }
//                }
//            },
//            'sendPeriod': {
//                validators: {
//                    notEmpty: {
//                        message: 'بازه زمانی ارسال الزامی است'
//                    }
//                }
//            },
//            'hourlyMinute': {
//                validators: {
//                    callback: {
//                        message: 'الزامی',
//                        callback: function (input) {
//                            var period = document.getElementById('send-period').value;
//                            return (period !== "hourly") ? true : (input.value !== "");
//                        },
//                    },
//                }
//            },
//            'dailyHour': {
//                validators: {
//                    callback: {
//                        message: 'الزامی',
//                        callback: function (input) {
//                            var period = document.getElementById('send-period').value;
//                            return (period !== "daily") ? true : (input.value !== "");
//                        },
//                    },
//                }
//            },
//            'dailyMinute': {
//                validators: {
//                    callback: {
//                        message: 'الزامی',
//                        callback: function (input) {
//                            var period = document.getElementById('send-period').value;
//                            return (period !== "daily") ? true : (input.value !== "");
//                        },
//                    },
//                }
//            },
//            'weeklyDOW': {
//                validators: {
//                    callback: {
//                        message: 'الزامی',
//                        callback: function (input) {
//                            var period = document.getElementById('send-period').value;
//                            return (period !== "weekly") ? true : (input.value !== "");
//                        },
//                    },
//                }
//            },
//            'weeklyHour': {
//                validators: {
//                    callback: {
//                        message: 'الزامی',
//                        callback: function (input) {
//                            var period = document.getElementById('send-period').value;
//                            return (period !== "weekly") ? true : (input.value !== "");
//                        },
//                    },
//                }
//            },
//            'weeklyMinute': {
//                validators: {
//                    callback: {
//                        message: 'الزامی',
//                        callback: function (input) {
//                            var period = document.getElementById('send-period').value;
//                            return (period !== "weekly") ? true : (input.value !== "");
//                        },
//                    },
//                }
//            },
//        },

//        plugins: {
//            trigger: new FormValidation.plugins.Trigger(),
//            bootstrap: new FormValidation.plugins.Bootstrap5({
//                rowSelector: '.fv-row',
//                eleInvalidClass: '',
//                eleValidClass: ''
//            })
//        }
//    }

//);

//$(sendCommentForm).submit(function (e) {
//    e.preventDefault();

//    if (schudaleStepValidation) {
//        schudaleStepValidation.validate().then(function (status) {
//            if (status == "Valid") {
//                sendCommentForm.submit();
//            }
//        })
//    }
//})

$("#website-step").on("click", function (e) {
    e.preventDefault();

    if (websiteStepValidation) {
        websiteStepValidation.validate().then(function (status) {

            if (status == "Valid") {
                stepper.goNext();
            }

        })
    }
})

$("#comment-type-step").on("click", function (e) {
    e.preventDefault();

    if (commentTypeStepValidation) {
        commentTypeStepValidation.validate().then(function (status) {
            if (status == "Valid") {
                $(this).button('loading');
                GetWordpressCategories(stepper);
            }
        })
    }
    

})

$("#select-category-step").on("click", function (e) {
    e.preventDefault();

    if (selectCategoryStepValidation) {
        selectCategoryStepValidation.validate().then(function (status) {
            if (status == "Valid") {
                $(this).button('loading');
                GetPostIds(stepper);
            }
        })
    } 
})

$("#comment-group-step").on("click", function (e) {
    e.preventDefault();
    if (commentGroupStepValidation) {
        commentGroupStepValidation.validate().then(function (status) {
            if (status == "Valid") {
                stepper.goNext();
                ConfirmSendCommentData();
            }
        })
    }
})



function GetWordpressCategories(stepper) {

    var websiteID = $('#website-select').find(":selected").val();
    var commentType = $('input[name=comment-type]:checked').val()

    var btn = document.getElementById("comment-type-step");

    btn.setAttribute('data-kt-indicator', 'on');
    btn.disabled = true;

    $.ajax({
        type: "POST",
        url: "/userpanel/GetWordpressCategories/",
        data: { websiteId : websiteID, commentType : commentType },
        dataType: "json",
        success: function (result) {
            var categorySelect = $("#destination-category");

            result.forEach((category) => {
                var option = new Option(category.name, category.id);
                categorySelect.append(option);
            });

            var groups = commentType == "blog" ? $("#comment-group-select option.commerce-gp") : $("#comment-group-select option.blog-gp");
            console.log(groups);
            Array.from(groups).forEach((group) => {
                group.remove();
            });

            stepper.goNext();
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
}

function GetPostIds(stepper) {
    var websiteID = $('#website-select').find(":selected").val();
    var commentType = $('input[name=comment-type]:checked').val()
    var catId = $('#destination-category').find(":selected").val();
    var btn = document.getElementById("select-category-step");

    btn.setAttribute('data-kt-indicator', 'on');
    btn.disabled = true;

    $.ajax({
        type: "POST",
        url: "/userpanel/GetWordpressPostsIds/",
        data: { websiteId: websiteID, commentType: commentType, catId: catId },
        dataType: "json",
        success: function (result) {

            $("#posts-id").val(result);
            console.log(result.length);
            $('input[name="productsPerSendCount"]').attr("max", result.length);
            stepper.goNext();
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
}

function ConfirmSendCommentData() {
    var websiteName = $("#website-select option:selected").text();
    var commentType = $("input[name=comment-type]:checked").val();
    var destinationCategory = $("#destination-category option:selected").text();
    var commentGroup = $("#comment-group-select option:selected").text();

    $("#cm-destination-name").text(websiteName);
    $("#cm-comment-type").text(commentType == "commerce" ? "فروشگاهی" : "بلاگ");
    $("#cm-destination-category").text(destinationCategory);
    $("#cm-comment-category").text(commentGroup);
}

$(document).on("change", "#send-period", function () {

    var selected = $("#send-period option:selected").val();

    if ($(".schedule-detail").hasClass("d-flex")) {

        $(".schedule-detail").removeClass("d-flex");
        $(".schedule-detail").addClass("d-none");

    }

    switch (selected) {
        case "hourly":
            $("#hourly").toggleClass("d-flex d-none");
            break;
        case "daily":
            $("#daily").toggleClass("d-flex d-none");
            break;
        case "weekly":
            $("#weekly").toggleClass("d-flex d-none");
            break;
    }
})