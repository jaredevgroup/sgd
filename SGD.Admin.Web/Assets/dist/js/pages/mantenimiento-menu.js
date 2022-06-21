const menuMantenimientoPage = {
    init: function () {
        menuMantenimientoPage.initEvents();
        menuMantenimientoPage.methods.obtenerDataDefault();
    },
    initEvents: function () {
        let { idFrmRegistro, idBtnCancelar, validate } = menuMantenimientoPage.options;
        let { btnCancelarClick } = menuMantenimientoPage.events;
        $(idFrmRegistro).validate(validate);
        $(idBtnCancelar).click(btnCancelarClick);
        $('#txt-icono').change(menuMantenimientoPage.events.txtIconoChange);
    },
    default: {
        menu: null,
        listaMenu: []
    },
    options: {
        idFrmRegistro: '#frm-registro',
        idBtnCancelar: '#btn-cancelar',
        validate: {
            rules: {
                nombre: {
                    required: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/menusistema/existe-nombre-menusistema`,
                        data: {
                            menuId: () => menuId,
                            nombre: () => $("#txt-nombre").val(),
                            menuIdPadre: () => menuIdPadre
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
                menuMantenimientoPage.methods.guardarMenu();
            }
        },
        select2: {
            theme: 'bootstrap4'
        },
        cargarMenuSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Si, de acuerdo',
            icon: 'warning',
            title: 'Advertencia',
            html: 'No existe<br/>¿Desea regresar a la lista de menús?'
        },
        confirmarCancelarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
            title: 'Confirmar',
            html: 'Existe una modificación<br/>¿Desea regresar a la lista de menús?'
        },
        finalizarGuardarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Regresar a la lista de menús',
        }
    },
    events: {
        btnCancelarClick: function (e) {
            menuMantenimientoPage.methods.validarCancelar();
        },
        txtIconoChange: function (e) {
            let iconoClass = $(e.currentTarget).val();
            console.log(iconoClass);
            $('#spn-icono').removeAttr('class').addClass(iconoClass);
        }
    },
    methods: {
        obtenerDataDefault: function () {
            let urlListaMenu = `${urlBaseWebApi}/api/menusistema/listar-menusistema-por-flagactivo/true`;
            let init = {};
            init.credentials = 'include';
            Promise.all([
                fetch(urlListaMenu, init)
            ])
                .then(funciones.managedResponseAllPromisesToJson)
                .then(menuMantenimientoPage.methods.guardarDataDefault)
        },
        guardarDataDefault: function ([dataListaMenu]) {
            resultListaMenu = dataListaMenu.status == 'success' ? (dataListaMenu.result || []) : [];
            menuMantenimientoPage.default.listaMenu = resultListaMenu.map(x => Object.assign({}, { ...x }));

            let selectMenuIdPadre = { ...menuMantenimientoPage.options.select2 };
            selectMenuIdPadre.data = resultListaMenu.map(x => Object.assign({ id: x.MenuId, text: x.Nombre }, { ...x }));
            $('#cmb-menuidpadre').select2(selectMenuIdPadre);
            $('#cmb-menuidpadre').val(menuIdPadre).trigger('change');

            menuMantenimientoPage.methods.obtenerMenu();
        },
        obtenerMenu: function () {
            if (menuId == null) {
                funciones.limpiarPreloader();
                return;
            }
            let url = `${urlBaseWebApi}/api/menusistema/obtener-menusistema/${menuId}`;
            let init = {};
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(menuMantenimientoPage.methods.cargarMenu);
        },
        cargarMenu: function (data) {
            let { status, result } = data;
            if (status != "success") return;
            if (result == null) {
                funciones.limpiarPreloader();
                Swal.fire(menuMantenimientoPage.options.cargarMenuSwal)
                    .then(menuMantenimientoPage.methods.redireccionarListaMenus);
                return;
            }
            menuMantenimientoPage.default.menu = { ...result };
            $("#txt-nombre").val(result.Nombre);
            $("#txt-url").val(result.Url);
            $("#txt-icono").val(result.Icono);
            funciones.limpiarPreloader();
        },
        guardarMenu: function () {
            let nombre = $('#txt-nombre').val();
            let urlMenu = $('#txt-url').val();
            let icono = $('#txt-icono').val();
            let body = {
                MenuId: menuId,
                Nombre: nombre,
                Url: urlMenu,
                Icono: icono,
                MenuIdPadre: menuIdPadre
            };
            let url = `${urlBaseWebApi}/api/menusistema/guardar-menusistema`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(menuMantenimientoPage.methods.finalizarGuardar);
        },
        finalizarGuardar: function (data) {
            let title = data.status == "success" ? data.result ? "Se guardó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            menuMantenimientoPage.options.finalizarGuardarSwal.title = title;
            menuMantenimientoPage.options.finalizarGuardarSwal.icon = icon;
            Swal.fire(menuMantenimientoPage.options.finalizarGuardarSwal)
                .then(menuMantenimientoPage.methods.redireccionarListaMenus);
        },
        redireccionarListaMenus: function (result) {
            if (!result.isConfirmed) return;
            location.href = `${urlBase}menu`;
            return false;
        },
        validarCancelar: function () {
            let { Nombre } = menuMantenimientoPage.default.menu || {};
            let nombre = $('#txt-nombre').val();
            let nombreCambiado = nombre != (Nombre || '');
            let huboCambio = nombreCambiado;
            if (huboCambio) menuMantenimientoPage.methods.confirmarCancelar();
            else location.href = `${urlBase}menu`;
        },
        confirmarCancelar: function () {
            Swal.fire(menuMantenimientoPage.options.confirmarCancelarSwal)
                .then(menuMantenimientoPage.methods.redireccionarListaMenus);
        },
    }
}