var adver = {
    init: function () {
        adver.regEvents();
    },

    regEvents: function () {
        $('#EndDate').change(function () {
            $.ajax({
                url: '/Advertisment/FindTotalMoney',
                dataType: 'json',
                data: {
                    EndDate: $(this).val(),
                    StartDate: $('#StartDate').val(),
                    IdLocation: $('#LocationAD').val()
                },
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        $('#TotalPrice').val(res.val+" VNĐ");
                    }
                }
            });
        })
    }
}
adver.init();

            //$.ajax({
            //    url: '/User/CheckUsername',
            //    dataType: 'json',
            //    data: {
            //        username: $(this).val()
            //    },
            //    type: 'POST',
            //    success: function (res) {
            //        if (res.status == false) {
            //            document.getElementById("UsernameMessage").innerHTML = "Tài khoản đã tồn tại";
            //            $('#username').val("");
            //            $('#username').focus();
            //        }
            //        else {
            //            document.getElementById("UsernameMessage").innerHTML = "";
            //        }
            //    }
            //});