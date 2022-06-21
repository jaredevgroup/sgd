const loginPage = {
    init: function () {
        loginPage.methods.cargarLogin();
        loginPage.initEvents();
    },
    initEvents: function () {
        $('#frm-login').validate(loginPage.options.validate);
    },
    options: {
        validate: {
            rules: {
                usuario: { required: true },
                contraseña: { required: true }
            },
            messages: {
                usuario: { required: "Ingrese usuario" },
                contraseña: { required: "Ingrese contraseña" }
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
            },
            submitHandler: function (form) {
                loginPage.methods.guardarLogin();
                form.submit();
            }
        }
    },
    methods: {
        cargarLogin: function () {
            let flgRecordar = eval(localStorage.getItem('login-recordar') || 'false');

            if (!flgRecordar) return;

            let usuario = localStorage.getItem('login-usuario') || '';
            let contraseña = localStorage.getItem('login-contraseña') || '';

            $('#chk-recordar').prop('checked', flgRecordar);
            $('#txt-usuario').val(usuario);
            $('#txt-contraseña').val(atob(contraseña));
        },
        guardarLogin: function () {
            let flgRecordar = $('#chk-recordar').prop('checked');

            if (!flgRecordar) {
                localStorage.removeItem('login-recordar');
                localStorage.removeItem('login-usuario');
                localStorage.removeItem('login-contraseña');
                return;
            }

            let usuario = $('#txt-usuario').val();
            let contraseña = btoa($('#txt-contraseña').val());

            localStorage.setItem('login-recordar', flgRecordar);
            localStorage.setItem('login-usuario', usuario);
            localStorage.setItem('login-contraseña', contraseña);
        }
    }
}