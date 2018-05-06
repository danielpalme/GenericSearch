$(function () {
    $(".dateSelect").on("change", function (event) {
        var selectBox = $(this);
        var dateInput = selectBox.parent().parent().find("input:eq(1)");

        if (selectBox.val() === '5') {
            $(dateInput).css('display', 'inline');
        }
        else {
            $(dateInput).css('display', 'none');
            $(dateInput).val('');
        }
    })
});