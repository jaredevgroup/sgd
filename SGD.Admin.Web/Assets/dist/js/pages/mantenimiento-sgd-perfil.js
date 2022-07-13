const perfilSGDMantenimientoPage = {
    init: function () {
        perfilSGDMantenimientoPage.initEvents();
        perfilSGDMantenimientoPage.methods.obtenerPerfil();
    },
    initEvents: function () {
        let { idFrmRegistro, idBtnCancelar, validate } = perfilSGDMantenimientoPage.options;
        let { btnCancelarClick } = perfilSGDMantenimientoPage.events;
        $(idFrmRegistro).validate(validate);
        $(idBtnCancelar).click(btnCancelarClick);
    },
    default: {
        perfil: null
    },
    options: {
        idFrmRegistro: '#frm-registro',
        idBtnCancelar: '#btn-cancelar',
        validate: {
            rules: {
                nombre: {
                    required: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/perfil/existe-nombre-perfil`,
                        data: {
                            perfilId: () => perfilId,
                            nombre: () => $("#txt-nombre").val()
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
                nombre: {
                    required: "Ingrese nombre",
                    remote: "Nombre ya existe"
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
                perfilSGDMantenimientoPage.methods.guardarPerfil();
            }
        },
        cargarPerfilSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Si, de acuerdo',
            icon: 'warning',
            title: 'Advertencia',
            html: 'No existe<br/>¿Desea regresar a la lista de perfiles?'
        },
        confirmarCancelarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
            title: 'Confirmar',
            html: 'Existe una modificación<br/>¿Desea regresar a la lista de perfiles?'
        },
        finalizarGuardarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Regresar a la lista de perfiles',
        }
    },
    events: {
        btnCancelarClick: function (e) {
            perfilSGDMantenimientoPage.methods.validarCancelar();
        }
    },
    methods: {
        obtenerPerfil: function () {
            if (perfilId == null) {
                funciones.limpiarPreloader();
                return;
            }
            let url = `${urlBaseWebApi}/api/perfil/obtener-perfil/${perfilId}`;
            let init = {};
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(perfilSGDMantenimientoPage.methods.cargarPerfil);
        },
        cargarPerfil: function (data) {
            let { status, result } = data;
            if (status != "success") return;
            if (result == null) {
                funciones.limpiarPreloader();
                Swal.fire(perfilSGDMantenimientoPage.options.cargarPerfilSwal)
                    .then(perfilSGDMantenimientoPage.methods.redireccionarListaPerfiles);
                return;
            }
            perfilSGDMantenimientoPage.default.perfil = { ...result };
            $("#txt-nombre").val(result.Nombre);
            funciones.limpiarPreloader();
        },
        guardarPerfil: function () {
            let nombre = $('#txt-nombre').val();
            let body = {
                PerfilId: perfilId,
                Nombre: nombre
            };
            let url = `${urlBaseWebApi}/api/perfil/guardar-perfil`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(perfilSGDMantenimientoPage.methods.finalizarGuardar);
        },
        finalizarGuardar: function (data) {
            let title = data.status == "success" ? data.result ? "Se guardó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            perfilSGDMantenimientoPage.options.finalizarGuardarSwal.title = title;
            perfilSGDMantenimientoPage.options.finalizarGuardarSwal.icon = icon;
            Swal.fire(perfilSGDMantenimientoPage.options.finalizarGuardarSwal)
                .then(perfilSGDMantenimientoPage.methods.redireccionarListaPerfiles);
        },
        redireccionarListaPerfiles: function (result) {
            if (!result.isConfirmed) return;
            location.href = `${urlBase}sgd-perfil`;
            return false;
        },
        validarCancelar: function () {
            let { Nombre } = perfilSGDMantenimientoPage.default.perfil || {};
            let nombre = $('#txt-nombre').val();
            let nombreCambiado = nombre != (Nombre || '');
            let huboCambio = nombreCambiado;
            if (huboCambio) perfilSGDMantenimientoPage.methods.confirmarCancelar();
            else location.href = `${urlBase}sgd-perfil`;
        },
        confirmarCancelar: function () {
            Swal.fire(perfilSGDMantenimientoPage.options.confirmarCancelarSwal)
                .then(perfilSGDMantenimientoPage.methods.redireccionarListaPerfiles);
        },
    }
}