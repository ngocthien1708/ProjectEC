var shop = {
    init: function () {
        shop.regEvents();
    },

    regEvents: function () {
        $('.btn-hidden').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { idsp: $(this).data('id') },
                url: '/ShopManager/HidingProduct',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/quan-ly-shop";
                    }
                }
            });
        });
    }
}

shop.init();