$("#validateLink").on("click", function (e) {

    e.preventDefault();
    var category = $('input[name="digikalaCategoryLink"]').val();

    if (category != "") {

        $.ajax({
            type: "POST",
            url: `https://api.digikala.com/v1/product/`,
            success: function (result) {
                console.log(result);
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

    } else {
        swal.fire({
            icon: 'warning',
            title: 'نام دسته بندی را وارد کنید',
            showConfirmButton: false,
            timer: 1500
        })
    }


})