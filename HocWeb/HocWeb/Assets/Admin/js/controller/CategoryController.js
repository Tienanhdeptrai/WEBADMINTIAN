var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Category/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đang hoạt động');
                    }
                    else {
                        btn.text('Đang khoá');
                    }

                }
            });
        });
    }
}
user.init();
var user1 = {
    init: function () {
        user1.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active1').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Category/ChangeShowOnHome",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.ShowOnHome == true) {
                        btn.text('Show');
                    }
                    else {
                        btn.text('None');
                    }

                }
            });
        });
    }
}
user1.init();

