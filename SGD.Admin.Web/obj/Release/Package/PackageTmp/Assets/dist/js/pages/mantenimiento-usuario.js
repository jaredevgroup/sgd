const usuarioMantenimientoPage = {
    init: function () {
        usuarioMantenimientoPage.initEvents();
        usuarioMantenimientoPage.methods.obtenerUsuario();
    },
    initEvents: function () {
        let { idFrmRegistro, idBtnCancelar, validate } = usuarioMantenimientoPage.options;
        let { btnCancelarClick } = usuarioMantenimientoPage.events;
        $(idFrmRegistro).validate(validate);
        $(idBtnCancelar).click(btnCancelarClick);
    },
    default: {
        usuario: null
    },
    options: {
        idFrmRegistro: '#frm-registro',
        idBtnCancelar: '#btn-cancelar',
        validate: {
            rules: {
                id: {
                    required: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/usuariosistema/existe-id-usuariosistema`,
                        data: {
                            tipoMantenimiento: () => tipoMantenimiento,
                            usuarioId: () => $('#txt-id').val()
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        dataFilter: function (responseText) {
                            let responseJson = JSON.parse(responseText);
                            let jsonString = JSON.stringify(responseJson.status != 'success' ? true : !responseJson.result);
                            return jsonString;
                        }
                    }
                },
                correo: {
                    required: true,
                    email: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/usuariosistema/existe-correo-usuariosistema`,
                        data: {
                            correo: () => $('#txt-correo').val(),
                            usuarioId: () => usuarioId,
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        dataFilter: function (responseText) {
                            let responseJson = JSON.parse(responseText);
                            let jsonString = JSON.stringify(responseJson.status != 'success' ? true : !responseJson.result);
                            return jsonString;
                        }
                    }
                }
            },
            messages: {
                id: {
                    required: "Ingrese id",
                    remote: "Id ya existe"
                },
                correo: {
                    required: "Ingrese correo",
                    email: "El correo ingresado no es válido",
                    remote: "Correo ya existe"
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                $(element).tooltip('dispose');
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
            submitHandler: function (form, e) {
                e.preventDefault();
                usuarioMantenimientoPage.methods.guardarUsuario();
            }
        },
        cargarUsuarioSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Si, de acuerdo',
            icon: 'warning',
            title: 'Advertencia',
            html: 'No existe<br/>¿Desea regresar a la lista de usuarios?'
        },
        confirmarCancelarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
            title: 'Confirmar',
            html: 'Existe una modificación<br/>¿Desea regresar a la lista de usuarios?'
        },
        finalizarGuardarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Regresar a la lista de usuarios',
        }
    },
    events: {
        btnCancelarClick: function (e) {
            usuarioMantenimientoPage.methods.validarCancelar();
        }
    },
    methods: {
        obtenerUsuario: function () {
            if (usuarioId == null) {
                funciones.limpiarPreloader();
                return;
            }
            let url = `${urlBaseWebApi}/api/usuariosistema/obtener-usuariosistema/${usuarioId}`;
            let init = {};
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(usuarioMantenimientoPage.methods.cargarUsuario);
        },
        cargarUsuario: function (data) {
            let { status, result } = data;
            if (status != "success") return;
            if (result == null) {
                funciones.limpiarPreloader();
                Swal.fire(usuarioMantenimientoPage.options.cargarUsuarioSwal)
                    .then(usuarioMantenimientoPage.methods.redireccionarListaUsuarios);
                return;
            }
            usuarioMantenimientoPage.default.usuario = { ...result };
            $("#txt-nombre").val(result.Nombre);
            funciones.limpiarPreloader();
        },
        guardarUsuario: function () {
            let id = $('#txt-id').val();
            let contraseña = $('#txt-contraseña').val() || '';
            let correo = $('#txt-correo').val();
            let body = {
                UsuarioId: id,
                Contraseña: contraseña,
                Correo: correo,
            };
            let url = `${urlBaseWebApi}/api/usuariosistema/guardar-usuariosistema/${tipoMantenimiento}`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(usuarioMantenimientoPage.methods.finalizarGuardar);
        },
        finalizarGuardar: function (data) {
            let title = data.status == "success" ? data.result ? "Se guardó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            usuarioMantenimientoPage.options.finalizarGuardarSwal.title = title;
            usuarioMantenimientoPage.options.finalizarGuardarSwal.icon = icon;
            Swal.fire(usuarioMantenimientoPage.options.finalizarGuardarSwal)
                .then(usuarioMantenimientoPage.methods.redireccionarListaUsuarios);
        },
        redireccionarListaUsuarios: function (result) {
            if (!result.isConfirmed) return;
            location.href = `${urlBase}usuario`;
            return false;
        },
        validarCancelar: function () {
            let { Nombre } = usuarioMantenimientoPage.default.usuario || {};
            let nombre = $('#txt-nombre').val();
            let nombreCambiado = nombre != (Nombre || '');
            let huboCambio = nombreCambiado;
            if (huboCambio) usuarioMantenimientoPage.methods.confirmarCancelar();
            else location.href = `${urlBase}usuario`;
        },
        confirmarCancelar: function () {
            Swal.fire(usuarioMantenimientoPage.options.confirmarCancelarSwal)
                .then(usuarioMantenimientoPage.methods.redireccionarListaUsuarios);
        },
    }
}