


$(document).ready({

if((@ViewBag.current_user.auto_extension) && (($('#toggle_event_editing button').hasClass('locked_active')) || (('#toggle_event_editing button').hasClass('locked_active')))) {
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
