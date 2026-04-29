$(document).ready(function () {
    $('#btnSalvar').on('click', saveDates);
    $('#closeModal').on('click', closeModal);
    $('#close').on('click', closeModal);

    function closeModal() {
        $('#modalInfo').modal('hide');
        $(document.activeElement).blur();
    };

    $('#note').on('input', function () {
        let v = this.value;

        v = v.replace(/[^0-9,]/g, '');

        let num = parseFloat(v.replace(',', '.'));

        if (num > 10) {
            v = '10';
        }

        this.value = v;
    });

    function saveDates() {
        var formData = $('#formNota').serialize();

        $('#botao').html(`<span class="spinner-border text-white" role="status"></span>`);

        $.ajax({
            url: '/Home/Edit',
            type: 'Post',
            data: formData,
            success: function () {
                $('#messagem').html(`<div class="col-12 p-2 bg-success text-white rounded" style="text-align: center; font-size: 17px;">
                    Nota salva com sucesso!
                    </div>`);

                $('#botao').html(`<button class="btn btn-outline-success" id="btnSalvar">Salvar</button>`);

                setTimeout(function () {
                    closeModal();
                }, 1500);
            },
            error: function () {
                $('#messagem').html(`
                  <div class="col-12 p-2 bg-danger text-white rounded" style="text-align: center; font-size: 17px;">
                    Erro ao salvar nota!</div>
                `);
            }
        });
    };
});