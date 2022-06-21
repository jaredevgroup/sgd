const recuperarContraseñaPage = {
    init: function () {
        recuperarContraseñaPage.initEvents();
    },
    initEvents: function () {
        $('#frm-recuperar-contraseña').validate(recuperarContraseñaPage.options.validate);
    },
    options: {
        validate: {
            rules: {
                correo: {
                    required: true,
                    email: true
                }
            },
            messages: {
                correo: {
                    required: "Ingrese correo electrónico",
                    email: "Correo electrónico incorrecto"
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                element.attr('data-toggle', 'tooltip');
                element.attr('title', error.text());
                element.tooltip();
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
                $(element).tooltip('dispose');
            }
        }
    }
}