// Define form element
const form = document.getElementById('add-website-form');

// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
var validator = FormValidation.formValidation(
    form,
    {
        fields: {
            'Websites.Name': {
                validators: {
                    notEmpty: {
                        message: 'نام وبسایت الزامی است'
                    }
                }
            },
            'Websites.Url': {
                validators: {
                    notEmpty: {
                        message: 'آدرس وبسایت الزامی است'
                    },
                    uri: {
                        message: 'آدرس وبسایت معتبر نیست',
                        protocol : "https"
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

// Delete Website
$(".delete-website").on("click", function (e) {

    e.preventDefault();

    let id = $(this).attr("data-id");
    

    Swal.fire({
        title: 'آیا از حذف این وبسایت اطمینان دارید ؟',
        text: "عملیات مورد نظر قابل برگشت نمی باشد!",
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
                url: "/userpanel/websites/",
                data: { Id: id },
                success: function (msg) {
                    if (msg == false) {
                        swal.fire({
                            title: 'خطا',
                            text: "ارسال فعالی با این وبسایت در حال انجام است تا اتمام ارسال قادر به حذف وبسایت نیستید.",
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
                }
            }); 
        }
    })
})

// Bind data on Edit modal

$(".edit-website-btn").on("click", function (e) {

    e.preventDefault();
    let id = $(this).attr('data-id');

    $.ajax({
        type: "POST",
        url: "/userpanel/GetWebsite/",
        data: {Id : id},
        dataType: "json",
        success: function (result) {
            let editModal = $("#edit-website-modal");

            editModal.find('input[name="Id"]').val(result.id);
            editModal.find('input[name="Name"]').val(result.name);
            editModal.find('input[name="Url"]').val(result.url);
            editModal.find('input[name="CustomerKey"]').val(result.customerKey);
            editModal.find('input[name="CustomerSecret"]').val(result.customerSecret);

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

// Edit Website

$(".edit-website-submit").on("click",function (e) {

    var website = $("#editWebsiteForm").serialize();
    var preloader = $(".spinner-container");
    preloader.show();
    $.ajax({
        type: "POST",
        url: "/userpanel/EditWebsite/",
        data: website,
        success: function (result) {

            $("#edit-website-modal .close").click();
            preloader.hide();

            if (result.status == "success") {
                swal.fire({
                    title: 'موفق',
                    text: result.message,
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 3000
                })
            }else {
                swal.fire({
                    title: 'خطا',
                    text: result.message,
                    icon: 'error',
                    showConfirmButton: false,
                    timer: 3000
                })
            }
            setTimeout(() => {
                location.reload();
            },2500)
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

// Add Website
$(form).submit(function (e) {

    e.preventDefault();

    if (validator) {
        validator.validate().then(function (status) {
            console.log('validated!');
            console.log(status);
            if (status == 'Valid') {

                form.submit();

            }
        });
    }

})