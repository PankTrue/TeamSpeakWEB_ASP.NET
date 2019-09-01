$(document).ready({

    if((UserAutoExtension) && (($('#toggle_event_editing button').hasClass('locked_active')) || (('#toggle_event_editing button').hasClass('locked_active')))) {
    $('#toggle_event_editing button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
    $('#toggle_event_editing button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
}


$('#toggle_event_editing button').click(function () {
    if ($(this).hasClass('locked_active') || $(this).hasClass('unlocked_inactive')) {
        $.ajax({
            type: 'POST',
            url: "/cabinet/edit_auto_extension",
            data: { auto_extension: true }
        });
        $('#toggle_event_editing button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    } else {
        /* code to do when locking */
        $.ajax({
            type: 'POST',
            url: "/cabinet/edit_auto_extension",
            data: { auto_extension: false }
        });
        $('#toggle_event_editing button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }
})

});


    function SetCost() {
        var data2 = parseInt($('#tsserver_time_payment').val());
        var data1 = parseInt($("#tsserver_slots").val());
        var money = UserMoney;
        var data = data1 * 2 * data2;
        if(data>money){
            var lacks = "Нехватает: " + (data-money).toString() + "руб.";
        }else{
          var lacks = "";
        }

        $('span.cost').text(data);
        $('span.lacks').text(lacks);
}


    $('#dns_input').bind('change', function (event) {
      var dns = $('#dns_input').val();
        if ((/^[A-Za-z0-9_-]+$/.test(dns)) && dns != '' ) {
        $.ajax({
            type: 'POST',
            url: "/cabinet/free_dns",
            data: { dns: dns },
            success: function (data) {
                if ((data.status) == true) {
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
    }else if (dns == ''){
        $('#dns_status').text('Введите домен(необязательно)');
    }else{
        $('#dns_status').text('Недопустимый домен');
    $('#dns_status').attr('style','color: red');
}
});



    $('#tsserver_time_payment').bind('click', function(event){
        SetCost();
    });

    $("#tsserver_slots").bind("keyup focusout", function(event){
      var data = parseInt($("#tsserver_slots").val());
    if (data>512)
      {
        $('#tsserver_slots').val(512);
    data = 512;
    SetCost();
  }

  if (data>=10)
      {
        var selector = $("[data-slider]");
    selector.simpleSlider("setValue", data);
    SetCost();

  }

});

    $("#tsserver_slots").bind("focusout", function(event){
      var data = parseInt($("#tsserver_slots").val());
      if (data<10)
      {
        $('#tsserver_slots').val(10);
    var selector = $("[data-slider]");
    selector.simpleSlider("setValue", 10);
    SetCost();

  }
});

    $("[data-slider]").bind("slider:ready slider:changed", function (event, data) {
        $('#tsserver_slots').val(data.value.toFixed(0));
    SetCost();


  });
