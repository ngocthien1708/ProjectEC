var cart = {
    init: function () {
        cart.regEvents();
    },

    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });

        $('#btnPayment').off('click').on('click', function () {
            var value = $('#btnPayment').val();
            if (value == "no") {
                window.alert("Phải đăng nhập trước khi đặt hàng");
                window.location.href = "/dang-nhap";
            }
            else {
                window.location.href = "/thanh-toan";
            }
        });

        $('#btnUpdate').off('click').on('click', function () {
            var listSanPham = $('.txtQuantity');
            var cartList = [];
            $.each(listSanPham, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                });
            });
            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == false) {
                        if (res.val == 1) {
                            window.alert("không được nhập nhỏ hơn 1");
                        }
                        else {
                            window.alert("Số lượng tối đa cho 1 sản phẩm là 3");
                        }
                    }
                    else
                    {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: {idsp:$(this).data('id')},
                url: '/Cart/DeleteSP',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });
    }
}

cart.init();