$(document).ready(function () {
    $('form').on('submit', iconLoad);

    function iconLoad() {
        $('#btnEntrar').attr('disabled', 'disable');

        $('#mensagem').html(`<span class="spinner-border text-white" role="status"></span> </div>`);
    }
});