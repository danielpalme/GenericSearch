$(function () {
    $(".date").datepicker();
    $("#tabs").tabs({ selected: $("#tabs").data('selectedindex') });

    $(".dateSelect").on("change", function (event) {
        var selectBox = $(this);
        var dateInput = selectBox.next().next()[0];
        if (selectBox.val() == 'InRange') {
            $(dateInput).css('display', 'inline-block');
        }
        else {
            $(dateInput).css('display', 'none');
            $(dateInput).val('');
        }
    })
});