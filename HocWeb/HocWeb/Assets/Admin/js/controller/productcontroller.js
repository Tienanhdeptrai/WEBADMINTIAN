var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('.btn-productactive').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Product/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Còn hàng');
                    }
                    else {
                        btn.text('Hết hàng');
                    }

                }
            });
        });
    }
}
product.init();