function SetCost() {
    var data2 = parseInt($('#tsserver_time_payment').val());
    var data1 = parseInt($("input#slots").val());
    var money = UserMoney;
    var data = data1 * 2 * data2;
    if (data > money) {
        var lacks = "Нехватает: " + (data - money).toString() + "руб.";
    } else {
        var lacks = "";
    }

    $('span.cost').text(data);
    $('span.lacks').text(lacks);
};

    $('#dns_input').bind('change', function (event) {
        var dns = $('#dns_input').val();
        if ((/^[A-Za-z0-9_-]+$/.test(dns)) && dns != '') {
            $.ajax({
                type: 'POST',
                url: "/cabinet/free_dns",
                data: { dns: dns },
                success: function (data) {
                    if (data == "false") {
                        $('#dns_status').text('Домен занят');
                        $('#dns_status').attr('style', 'color: red');
                    } else {
                        $('#dns_status').text('Домен свободен');
                        $('#dns_status').attr('style', 'color: green');
                    }
                },
                error: function (data) {

                }

            });
        } else if (dns == '') {
            $('#dns_status').text('Введите домен(необязательно)');
        } else {
            $('#dns_status').text('Недопустимый домен');
            $('#dns_status').attr('style', 'color: red');
        }
    });

    $('input#slots').bind('keyup focusout', function (event) {
        SetCost();
    });

    $('#tsserver_time_payment').bind('click', function (event) {
        SetCost();
    });
