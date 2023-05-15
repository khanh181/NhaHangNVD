$(function () {
    $('nav ul li a p').each(function (i) {
        var name = $(this).text().trim() // This is your rel value
        if (name.length > 20) {
            $(this).text(name.substring(0, 20) + "...")
        }
    });
    $('[data-toggle="tooltip"]').tooltip()
});

function toastCanhbao(mess) {
    $.toast({
        text: mess,
        showHideTransition: 'plain',
        position: 'top-right',
        icon: 'warning'
    })
}
function toastThanhCong(mess) {
    $.toast({
        text: mess,
        showHideTransition: 'slide',
        position: 'top-right',
        icon: 'success'
    })
}
function toastThatBai(mess) {
    $.toast({
        text: mess,
        showHideTransition: 'fade',
        position: 'top-right',
        icon: 'error'
    })
}


function openModalCommom(idModal) {
    $("#" + idModal).modal({
        backdrop: "static", //remove ability to close modal with click
        keyboard: false, //remove option to close with keyboard
        show: true //Display loader!
    });
}

function closeModalCommom(idModal) {
    $("#" + idModal).modal("hide");
}

function showModalConfirm(action, title, content) {
    $("#modal-confirm .modal-title").text(title)
    $("#modal-confirm .modal-body>p").text(content)
    $("#modal-confirm .modal-footer #btn-xacnhan").attr("onclick", action)
    $("#modal-confirm").modal('show');
}