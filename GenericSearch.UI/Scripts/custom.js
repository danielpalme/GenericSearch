$(function () {
    $(".dateSelect").on("change", function (event) {
        var selectBox = $(this);
        var dateInput = selectBox.parent().parent().find("input:last-child");

        if (selectBox.val() == 'InRange') {
            $(dateInput).css('display', 'inline');
        }
        else {
            $(dateInput).css('display', 'none');
            $(dateInput).val('');
        }
    })
});