$(document).ready(function () {
    $('.game-card').on('click', passDataModal);
    $('#btnInfoUser').on('click', modalUser);

    function modalUser() {
        $.ajax({
            url: '/Home/InfoUser',
            type: 'GET',
            success: function (html) {
                $('#modalContent').html(html);
                $('#modalInfo').modal('show');
            },
            error: function () {
                $('#modalContent').html(`
                    <div class="modal-body text-center text-danger">
                        Erro ao carregar. Tente novamente.
                    </div>`
                );
                $('#modalInfo').modal('show');
            }
        });
    }

    function passDataModal() {
        var gameName = $(this).data('game');
        var positionGame = $(this).data('position');
        var img = $(this).data('img');
        var note = $(this).data('note');
        var finalNote = $(this).data('final-note');
        var indicador = $(this).data('indicador');

        $('#modalContent').html(`
            <div class="modal-body d-flex justify-content-center align-items-center" style="min-height: 200px">
                <span class="text-white">Carregando... </span>
                <span class="spinner-border text-white" role="status"></span>
            </div>
        `);

        $('#modalInfo').modal('show');

        $.ajax({
            url: '/Home/Edit',
            type: "Get",
            success: function (html) {
                $('#modalContent').html(html);

                $('#gameName').text(gameName);
                $('#gameFinalNote').text(`TOTAL DE NOTA ATRIBUITA: ${finalNote}`);
                $('#gameIndicador').text(`JOGADOR QUE INDICOU: ${indicador}`);
                $('#positionGame').val(positionGame);
                $('#gameImg').attr('src', img);
                $('#note').val(note);

                $('#modalInfo').modal('show');
            },
            error: function () {
                $('#modalContent').html(`
                    <div class="modal-body text-center text-danger">
                        Erro ao carregar. Tente novamente.
                         <button type="button" class="btn btn-outline-info" data-bs-dismiss="modal"> Fechar </button>
                    </div>
                `);
            }
        });
    }

    $('#modalInfo').on('hidden.bs.modal', () => {
        $(document.activeElement).blur();
        $('body').removeClass('modal-open');
        $('body').css('overflow', '');
        $('body').css('padding-right', '');
        $('.modal-backdrop').remove();
    });
});