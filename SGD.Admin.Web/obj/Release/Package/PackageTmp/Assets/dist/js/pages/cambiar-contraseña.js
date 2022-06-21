const cambiarContraseñaPage = {
    init: function () {
        cambiarContraseñaPage.initEvents();
    },
    initEvents: function () {
        $('#frm-cambiar-contraseña').validate(cambiarContraseñaPage.options.validate);
    },
    options: {
        validate: {
            rules: {
                contraseña: { required: true },
                "contraseña-confirmar": {
                    required: true,
                    equalTo: '#txt-contraseña'
                }
            },
            messages: {
                contraseña: { required: "Ingrese contraseña" },
                "contraseña-confirmar": {
                    required: "Confirme contraseña",
                    equalTo: "Las contraseñas no coinciden"
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