const entidadDeportivaMantenimientoPage = {
    init: function () {
        entidadDeportivaMantenimientoPage.initEvents();
        entidadDeportivaMantenimientoPage.methods.obtenerListas();
    },
    initEvents: function () {
        let { idFrmRegistro, idBtnCancelar, idBtnBuscarUsuario, idBtnNuevoUsuario, idTblResultadosUsuario, validate, dataTableUsuario } = entidadDeportivaMantenimientoPage.options;
        let { btnCancelarClick, btnBuscarUsuarioClick, btnNuevoUsuarioClick, tblResultadosUsuarioOnDraw, tblResultadosUsuarioOnPreXhr, tblResultadosUsuarioOnXhr } = entidadDeportivaMantenimientoPage.events;
        $(idFrmRegistro).validate(validate);
        $(idBtnCancelar).click(btnCancelarClick);
        $(idBtnNuevoUsuario).click(btnNuevoUsuarioClick);
        $(idBtnBuscarUsuario).click(btnBuscarUsuarioClick);
        $(idTblResultadosUsuario).DataTable(dataTableUsuario)
            .on("draw", tblResultadosUsuarioOnDraw)
            .on("preXhr", tblResultadosUsuarioOnPreXhr)
            .on("xhr", tblResultadosUsuarioOnXhr);
    },
    default: {
        entidadDeportiva: null,
        perfilIdSeleccionado: null,
        usuarioIdSeleccionado: null
    },
    options: {
        idFrmRegistro: '#frm-registro',
        idBtnCancelar: '#btn-cancelar, #btn-cancelar-menu, #btn-cancelar-perfil',
        idBtnNuevoUsuario: '#btn-nuevo-usuario',
        idBtnBuscarUsuario: '#btn-buscar-usuario',
        idTblResultadosUsuario: '#tbl-resultados-usuario',
        validate: {
            rules: {
                codigo: {
                    required: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/entidaddeportiva/existe-codigo-entidaddeportiva`,
                        data: {
                            codigo: () => $('#txt-codigo').val(),
                            entidadDeportivaId: () => entidadDeportivaId
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
                nombre: {
                    required: true
                }
            },
            messages: {
                codigo: {
                    required: "Ingrese código",
                    remote: "Código ya existe"
                },
                nombre: {
                    required: "Ingrese nombre",
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
                entidadDeportivaMantenimientoPage.methods.guardarEntidadDeportiva();
            }
        },
        validateMenu: {
            rules: {
                'menu-entidaddeportiva': {
                    required: 1
                }
            },
            messages: {
                'menu-entidaddeportiva': {
                    required: "Debe marcar mínimo una opción"
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
                if (element.type == 'checkbox') {
                    $(element).parent().addClass('is-invalid');
                } else {
                    $(element).addClass('is-invalid');
                }
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
                $(element).tooltip('dispose');
            },
            submitHandler: function (form, e) {
                e.preventDefault();
                entidadDeportivaMantenimientoPage.methods.guardarAsignacionListaMenuEntidadDeportiva();
            }
        },
        validatePerfil: {
            rules: {
                'perfil-entidaddeportiva': {
                    required: 1
                }
            },
            messages: {
                'perfil-entidaddeportiva': {
                    required: "Debe marcar mínimo una opción"
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
                if (element.type == 'checkbox') {
                    $(element).parent().addClass('is-invalid');
                } else {
                    $(element).addClass('is-invalid');
                }
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
                $(element).tooltip('dispose');
            },
            submitHandler: function (form, e) {
                e.preventDefault();
                entidadDeportivaMantenimientoPage.methods.guardarAsignacionListaPerfilEntidadDeportiva();
            }
        },
        validatePerfilMenu: {
            rules: {
                'perfilmenu-entidaddeportiva': {
                    required: true,
                    //minlength: 1
                }
            },
            messages: {
                'perfilmenu-entidaddeportiva': {
                    required: "Marque mínimo una opción",
                    //minlength: "Debe marcar mínimo una opción"
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
                if (element.type == 'checkbox') {
                    $(element).parent().addClass('is-invalid');
                } else {
                    $(element).addClass('is-invalid');
                }
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
                $(element).tooltip('dispose');
            },
            submitHandler: function (form, e) {
                e.preventDefault();
                entidadDeportivaMantenimientoPage.methods.guardarAsignacionListaMenu();
            }
        },
        validateUsuarioPerfil: {
            rules: {
                'usuarioperfil': {
                    required: true,
                    //minlength: 1
                }
            },
            messages: {
                'usuarioperfil': {
                    required: "Marque mínimo una opción",
                    //minlength: "Debe marcar mínimo una opción"
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
                if (element.type == 'checkbox') {
                    $(element).parent().addClass('is-invalid');
                } else {
                    $(element).addClass('is-invalid');
                }
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
                $(element).tooltip('dispose');
            },
            submitHandler: function (form, e) {
                e.preventDefault();
                entidadDeportivaMantenimientoPage.methods.guardarAsignacionListaPerfilUsuario();
            }
        },
        validateUsuario: {
            rules: {
                "usuario-id": {
                    required: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/usuario/existe-id-usuario`,
                        data: {
                            entidadDeportivaId: () => entidadDeportivaId,
                            tipoMantenimiento: () => entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado == null ? 1 : 2,
                            usuarioId: () => $('#txt-usuario-id').val()
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
                "usuario-correo": {
                    required: true,
                    email: true,
                    remote: {
                        url: `${urlBaseWebApi}/api/usuario/existe-correo-usuario`,
                        data: {
                            entidadDeportivaId: () => entidadDeportivaId,
                            correo: () => $('#txt-correo').val(),
                            usuarioId: () => entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado,
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
                "usuario-id": {
                    required: "Ingrese id",
                    remote: "Id ya existe"
                },
                "usuario-correo": {
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
                entidadDeportivaMantenimientoPage.methods.guardarUsuario();
            }
        },
        dataTableUsuario: {
            processing: true,
            serverSide: true,
            ajax: {
                url: `${urlBaseWebApi}/api/usuario/buscar-usuario`,
                data: function (params, settings) {
                    let [columnIndex, sortOrder] = $(settings.nTable).DataTable().order()[0];
                    let sortName = $(settings.nTable).DataTable().column(columnIndex).dataSrc();

                    params.entidadDeportivaId = () => entidadDeportivaId;
                    params.nombre = () => $('#txt-filtro-usuario-nombre').val();
                    params.correo = () => $('#txt-filtro-usuario-correo').val();
                    params.pageNumber = (params.start / params.length) + 1;
                    params.pageSize = params.length;
                    params.sortName = sortName;
                    params.sortOrder = sortOrder;
                },
                xhrFields: {
                    withCredentials: true
                },
                dataFilter: function (responseText) {
                    let responseJson = JSON.parse(responseText);
                    let jsonString = JSON.stringify(responseJson.result);
                    return jsonString;
                },
            },
            columns: [
                { data: 'UsuarioId', title: 'Nombre', orderable: true },
                { data: 'Correo', title: 'Correo', orderable: true },
                { data: 'FlagActivo', title: 'Estado', orderable: true, render: (data) => data ? "Activo" : "Inactivo" },
                { data: 'FlagCambioContraseña', title: 'Cambió contraseña', orderable: true, render: (data) => data ? "Sí" : "No" },
                { data: 'UsuarioId', title: 'Opciones', orderable: false, render: (data, type, row) => `${(row.FlagActivo ? `<button class="btn btn-info btn-xs btn-editar-usuario" data-item-id="${data}" data-toggle="tooltip" title="Ir a editar"><i class="fa fa-pen"></i></button>&nbsp;` : "")}<button type="button" class="btn btn-${(row.FlagActivo ? "danger" : "success")} btn-xs btn-confirmar-cambiar-flagactivo-usuario" data-item-id="${data}" data-item-flagactivo="${row.FlagActivo}" data-toggle="tooltip" title="${(row.FlagActivo ? "Inactivar" : "Activar")}"><i class="fa fa-${(row.FlagActivo ? "ban" : "check")}"></i></button>&nbsp;<button type="button" class="btn btn-primary btn-xs btn-editar-lista-perfiles-usuario" data-item-id="${data}" data-toggle="tooltip" title="Asignar Perfiles"><i class="fa fa-list"></i></button>` }
            ],
            createdRow: function (row, data, dataIndex) {
                if (!data.FlagActivo) $(row).addClass('bg-danger disabled');
            },
            paging: true,
            lengthChange: false,
            searching: false,
            ordering: true,
            info: true,
            autoWidth: false,
            responsive: true,
            language: {
                paginate: {
                    first: 'Primero',
                    next: 'Siguiente',
                    previous: 'Anterior',
                    last: 'Último'
                },
                info: 'Mostrando del _START_ al _END_ de _TOTAL_ registros',
                infoEmpty: '',
                emptyTable: 'No existe información'
            }
        },
        cargarEntidadDeportivaSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Si, de acuerdo',
            icon: 'warning',
            title: 'Advertencia',
            html: 'No existe<br/>¿Desea regresar a la lista de entidades deportivas?'
        },
        confirmarCancelarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
            title: 'Confirmar',
            html: 'Existe una modificación<br/>¿Desea regresar a la lista de entidades deportivas?'
        },
        finalizarGuardarSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: 'Regresar a la lista de entidades deportivas',
        },
        defaultSwal: {
            class: 'bg-success',
            autohide: true,
            delay: 3000,
            close: false
        },
        editarListaPermisosSwal: {
            title: "Asignar Menús",
            message:
                `<form id="frm-editar-lista-permisos" autocomplete="off">
                    <div class="row">
                        <div id="div-lista-menu-content" class="col-sm-12"></div>
                    </div>
                </form>`,
            closeButton: true,
            buttons: {
                guardar: {
                    label: 'Guardar',
                    className: 'btn-success',
                    callback: function () {
                        $('#frm-editar-lista-permisos').trigger("submit");
                        return false;
                    },

                },
                cancelar: {
                    label: 'Cancelar',
                    classname: 'btn-danger',
                }
            },
            onShow: function (e) {
                $(e.currentTarget).attr("id", "modal-editar-lista-permisos");
                entidadDeportivaMantenimientoPage.methods.obtenerListaPermisos();
            },
            onHide: function (e) {
                entidadDeportivaMantenimientoPage.default.perfilIdSeleccionado = null;
            },
            animateIn: 'zoomInDown',
            animateOut: 'zoomOutUp'
        },
        mantenimientoUsuarioSwal: {
            title: "",
            message:
                `<form id="frm-registro-usuario" novalidate autocomplete="off">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="txt-usuario-id">ID</label>
                                <input type="text" name="usuario-id" id="txt-usuario-id" class="form-control" placeholder="ID">
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="txt-usuario-contraseña">Contraseña</label>
                                <input type="password" name="usuario-contraseña" id="txt-usuario-contraseña" class="form-control" placeholder="Contraseña">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="form-group">
                                <label for="txt-usuario-correo">Correo</label>
                                <input type="text" name="usuario-correo" id="txt-usuario-correo" class="form-control" placeholder="Correo">
                            </div>
                        </div>
                    </div>
                </form>`,
            closeButton: true,
            buttons: {
                guardar: {
                    label: 'Guardar',
                    className: 'btn-success',
                    callback: function () {
                        $('#frm-registro-usuario').trigger("submit");
                        return false;
                    },

                },
                cancelar: {
                    label: 'Cancelar',
                    classname: 'btn-danger',
                }
            },
            onShow: function (e) {
                $(e.currentTarget).attr("id", "modal-mantenimiento-usuario");
                $("#frm-registro-usuario").validate(entidadDeportivaMantenimientoPage.options.validateUsuario);
                let { usuarioIdSeleccionado } = entidadDeportivaMantenimientoPage.default;
                if (usuarioIdSeleccionado != null) {
                    $("#txt-usuario-id").prop("readonly", true).parent().parent().attr("class", "col-sm-12");
                    $("#txt-usuario-contraseña").parent().parent().remove();
                    $("#txt-usuario-correo").parent().parent().attr("class", "col-sm-12");
                    entidadDeportivaMantenimientoPage.methods.obtenerUsuario();
                }
                //$(e.currentTarget).attr("id", "modal-editar-lista-permisos");
                //entidadDeportivaMantenimientoPage.methods.obtenerListaPermisos();
            },
            onHide: function (e) {
                entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado = null;
            },
            animateIn: 'zoomInDown',
            animateOut: 'zoomOutUp'
        },
        cambiarFlagActivoUsuarioSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
        },
        editarListaPerfilesUsuarioSwal: {
            title: "Asignar Menús",
            message:
                `<form id="frm-editar-lista-perfiles" autocomplete="off">
                    <div class="row">
                        <div id="div-lista-perfil-content" class="col-sm-12"></div>
                    </div>
                </form>`,
            closeButton: true,
            buttons: {
                guardar: {
                    label: 'Guardar',
                    className: 'btn-success',
                    callback: function () {
                        $('#frm-editar-lista-perfiles').trigger("submit");
                        return false;
                    },

                },
                cancelar: {
                    label: 'Cancelar',
                    classname: 'btn-danger',
                }
            },
            onShow: function (e) {
                $(e.currentTarget).attr("id", "modal-editar-lista-perfiles");
                entidadDeportivaMantenimientoPage.methods.obtenerListaPerfilesUsuario();
            },
            onHide: function (e) {
                entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado = null;
            },
            animateIn: 'zoomInDown',
            animateOut: 'zoomOutUp'
        }
    },
    events: {
        btnCancelarClick: function (e) {
            entidadDeportivaMantenimientoPage.methods.validarCancelar();
        },
        chkMenuEntidadDeportivaChecked: function (e) {
            funciones.treeCheckBoxChecked(e.currentTarget);
        },
        chkPerfilEntidadDeportivaChecked: function (e) {
            let el = e.currentTarget;
            entidadDeportivaMantenimientoPage.methods.cambiarVisibilidadAsignarMenuAlPerfil(el);
        },
        chkMenuChecked: function (e) {
            funciones.treeCheckBoxChecked(e.currentTarget);
        },
        btnEditarListaPermisosClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            entidadDeportivaMantenimientoPage.methods.mostrarModalEditarListaPermisos(id);
        },
        tblResultadosUsuarioOnDraw: function (e) {
            $(e.currentTarget).find('thead tr').addClass('bg-secondary')
            $(e.currentTarget).find('.btn-editar-usuario').click(entidadDeportivaMantenimientoPage.events.btnEditarUsuarioClick);
            $(e.currentTarget).find('.btn-confirmar-cambiar-flagactivo-usuario').click(entidadDeportivaMantenimientoPage.events.btnConfirmarCambiarFlagActivoUsuarioClick);
            $(e.currentTarget).find('.btn-editar-lista-perfiles-usuario').click(entidadDeportivaMantenimientoPage.events.btnEditarListaPerfilesUsuarioClick);
            $('[data-toggle="tooltip"]').tooltip();
        },
        tblResultadosUsuarioOnPreXhr: function (e) {
            $(entidadDeportivaMantenimientoPage.options.idBtnBuscarUsuario).prop('disabled', true);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Open();
        },
        tblResultadosUsuarioOnXhr: function (e) {
            $(entidadDeportivaMantenimientoPage.options.idBtnBuscarUsuario).prop('disabled', false);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Close();
        },
        btnNuevoUsuarioClick: function (e) {
            entidadDeportivaMantenimientoPage.methods.mostrarModalMantenimientoUsuario();
        },
        btnBuscarUsuarioClick: function (e) {
            entidadDeportivaMantenimientoPage.methods.buscarUsuario();
        },
        btnEditarUsuarioClick: function (e) {
            let id = $(e.currentTarget).attr("data-item-id");
            entidadDeportivaMantenimientoPage.methods.mostrarModalMantenimientoUsuario(id);
        },
        btnConfirmarCambiarFlagActivoUsuarioClick: function (e) {
            let id = $(e.currentTarget).attr("data-item-id");
            let flagActivo = eval($(e.currentTarget).attr("data-item-flagactivo"));
            entidadDeportivaMantenimientoPage.methods.confirmarCambiarFlagActivoUsuario(id, flagActivo);
        },
        btnEditarListaPerfilesUsuarioClick: function (e) {
            let id = $(e.currentTarget).attr("data-item-id");
            entidadDeportivaMantenimientoPage.methods.mostrarModalEditarListaPerfilesUsuario(id);
        }
    },
    methods: {
        obtenerListas: function () {
            let urlListaMenu = `${urlBaseWebApi}/api/menu/listar-menu-por-flagactivo-menuidpadre/true/true`;
            let urlListaPerfil = `${urlBaseWebApi}/api/perfil/listar-perfil-por-flagactivo/true`;
            let init = {};
            init.credentials = 'include';

            Promise.all([
                fetch(urlListaMenu, init),
                fetch(urlListaPerfil, init),
            ])
                .then(funciones.managedResponseAllPromisesToJson)
                .then(entidadDeportivaMantenimientoPage.methods.cargarListas);
        },
        cargarListas: function ([dataListaMenu, dataListaPerfil]) {
            entidadDeportivaMantenimientoPage.methods.cargarListaMenu(dataListaMenu);
            entidadDeportivaMantenimientoPage.methods.cargarListaPerfil(dataListaPerfil);
            if (entidadDeportivaId == null) funciones.limpiarPreloader();
            else entidadDeportivaMantenimientoPage.methods.obtenerEntidadDeportiva();
        },
        cargarListaMenu: function (data) {
            let itemList = data.status == "success" ? data.result || [] : [];
            let menuHtml = funciones.renderiCheckBoxToHtml(null, itemList, 'ListaSubMenu', 'menu-entidaddeportiva', 'MenuId', 'Nombre');
            $('#div-registro-menu-lista-menu-content').html(menuHtml);
            $(':input[id^="chk-menu-entidaddeportiva-"]').change(entidadDeportivaMantenimientoPage.events.chkMenuEntidadDeportivaChecked);
            $('#frm-registro-menu').validate(entidadDeportivaMantenimientoPage.options.validateMenu);
        },
        cargarListaPerfil: function (data) {
            let itemList = data.status == "success" ? data.result || [] : [];
            let botones = [
                `<button type="button" class="btn btn-primary btn-xs btn-editar-lista-permisos" data-toggle="tooltip" title="Asignar Permisos"><i class="fa fa-list"></i></button>`
            ];
            let perfilHtml = funciones.renderiCheckBoxToHtml(null, itemList, null, 'perfil-entidaddeportiva', 'PerfilId', 'Nombre', botones);
            $('#div-registro-perfil-lista-perfil-content').html(perfilHtml);
            $(':input[id^="chk-perfil-entidaddeportiva-"]').change(entidadDeportivaMantenimientoPage.events.chkPerfilEntidadDeportivaChecked);
            $('.btn-editar-lista-permisos').click(entidadDeportivaMantenimientoPage.events.btnEditarListaPermisosClick);
            $('#frm-registro-perfil').validate(entidadDeportivaMantenimientoPage.options.validatePerfil);
            $('[data-toggle="tooltip"]').tooltip();
        },
        cambiarVisibilidadAsignarMenuAlPerfil(el) {
            let { checked } = el;
            if (checked) $(el).parent().find(".btn-editar-lista-permisos").removeClass("d-none");
            else $(el).parent().find(".btn-editar-lista-permisos").addClass("d-none");
        },
        obtenerEntidadDeportiva: function () {
            if (entidadDeportivaId == null) {
                funciones.limpiarPreloader();
                return;
            }
            let url = `${urlBaseWebApi}/api/entidaddeportiva/obtener-entidaddeportiva/${entidadDeportivaId}/true/true/false`;
            let init = {};
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.cargarEntidadDeportiva);
        },
        cargarEntidadDeportiva: function (data) {
            let { status, result } = data;
            if (status != "success") return;
            if (result == null) {
                funciones.limpiarPreloader();
                Swal.fire(entidadDeportivaMantenimientoPage.options.cargarEntidadDeportivaSwal)
                    .then(entidadDeportivaMantenimientoPage.methods.redireccionarListaEntidadDeportivas);
                return;
            }
            entidadDeportivaMantenimientoPage.default.entidadDeportiva = { ...result };
            $("#txt-codigo").val(result.Codigo);
            $("#txt-nombre").val(result.Nombre);
            entidadDeportivaMantenimientoPage.methods.cargarEntidadDeportivaLista(status, result.ListaMenu, "menu-entidaddeportiva", "MenuId", "FlagActivo");
            entidadDeportivaMantenimientoPage.methods.cargarEntidadDeportivaLista(status, result.ListaPerfil, "perfil-entidaddeportiva", "PerfilId", "FlagActivo");
            funciones.limpiarPreloader();
        },
        cargarEntidadDeportivaLista(status, lista, inputName, campoId, campoFlagActivo) {
            let itemListAsignado = (status == "success" ? lista || [] : []).filter(x => x[campoFlagActivo]);
            let itemListAsignadoIds = itemListAsignado.map(x => x[campoId]);
            Array.from($(`input[type="checkbox"][name="${inputName}"]`)).forEach(x => $(x).prop("checked", itemListAsignadoIds.some(y => y == x.value)).trigger("change"));
        },
        guardarEntidadDeportiva: function () {
            let codigo = $('#txt-codigo').val();
            let nombre = $('#txt-nombre').val();
            let body = {
                EntidadDeportivaId: entidadDeportivaId,
                Codigo: codigo,
                Nombre: nombre,
            };
            let url = `${urlBaseWebApi}/api/entidaddeportiva/guardar-entidaddeportiva`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarGuardar);
        },
        finalizarGuardar: function (data) {
            let title = data.status == "success" ? data.result ? "Se guardó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.finalizarGuardarSwal.title = title;
            entidadDeportivaMantenimientoPage.options.finalizarGuardarSwal.icon = icon;
            Swal.fire(entidadDeportivaMantenimientoPage.options.finalizarGuardarSwal)
                .then(entidadDeportivaMantenimientoPage.methods.redireccionarListaEntidadDeportivas);
        },
        redireccionarListaEntidadDeportivas: function (result) {
            if (!result.isConfirmed) return;
            location.href = `${urlBase}entidad-deportiva`;
            return false;
        },
        validarCancelar: function () {
            let { Nombre } = entidadDeportivaMantenimientoPage.default.entidadDeportiva || {};
            let nombre = $('#txt-nombre').val();
            let nombreCambiado = nombre != (Nombre || '');
            let huboCambio = nombreCambiado;
            if (huboCambio) entidadDeportivaMantenimientoPage.methods.confirmarCancelar();
            else location.href = `${urlBase}entidad-deportiva`;
        },
        confirmarCancelar: function () {
            Swal.fire(entidadDeportivaMantenimientoPage.options.confirmarCancelarSwal)
                .then(entidadDeportivaMantenimientoPage.methods.redireccionarListaEntidadDeportivas);
        },
        guardarAsignacionListaMenuEntidadDeportiva: function () {
            let listaMenu = Array.from($('input[type="checkbox"][name="menu-entidaddeportiva"]')).map(x => Object.assign({}, { MenuId: x.value, FlagActivo: x.checked }));
            let body = {
                EntidadDeportivaId: entidadDeportivaId,
                ListaMenu: listaMenu
            }
            let url = `${urlBaseWebApi}/api/entidaddeportiva/guardar-lista-menu-por-entidaddeportiva`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarGuardarAsignacionListaMenuEntidadDeportiva);
        },
        finalizarGuardarAsignacionListaMenuEntidadDeportiva: function (data) {
            let title = data.status == "success" ? data.result ? "Se asignó la lista de menús correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.defaultSwal.title = title;
            entidadDeportivaMantenimientoPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaMantenimientoPage.options.defaultSwal);
            //entidadDeportivaMantenimientoPage.methods.obtenerEntidadDeportiva();
        },
        guardarAsignacionListaPerfilEntidadDeportiva: function () {
            let listaPerfil = Array.from($('input[type="checkbox"][name="perfil-entidaddeportiva"]')).map(x => Object.assign({}, { PerfilId: x.value, FlagActivo: x.checked }));
            let body = {
                EntidadDeportivaId: entidadDeportivaId,
                ListaPerfil: listaPerfil
            }
            let url = `${urlBaseWebApi}/api/entidaddeportiva/guardar-lista-perfil-por-entidaddeportiva`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarGuardarAsignacionListaPerfilEntidadDeportiva);
        },
        finalizarGuardarAsignacionListaPerfilEntidadDeportiva: function (data) {
            let title = data.status == "success" ? data.result ? "Se asignó la lista de permisos correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.defaultSwal.title = title;
            entidadDeportivaMantenimientoPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaMantenimientoPage.options.defaultSwal);
            //entidadDeportivaMantenimientoPage.methods.obtenerEntidadDeportiva();
        },
        mostrarModalEditarListaPermisos: function (id) {
            entidadDeportivaMantenimientoPage.default.perfilIdSeleccionado = id;
            bootbox.dialog(entidadDeportivaMantenimientoPage.options.editarListaPermisosSwal);
        },
        obtenerListaPermisos: function () {
            const { perfilIdSeleccionado } = entidadDeportivaMantenimientoPage.default;
            let urlListaMenu = `${urlBaseWebApi}/api/menu/listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/${entidadDeportivaId}/true/true`;
            let urlListaMenuAsignado = `${urlBaseWebApi}/api/menu/listar-menu-por-flagactivo-entidaddeportivaperfil/${entidadDeportivaId}/${perfilIdSeleccionado}/true`;
            let init = {};
            init.credentials = 'include';
            funciones.loading('#modal-editar-lista-permisos .modal-content').Open();

            Promise.all([
                fetch(urlListaMenu, init),
                fetch(urlListaMenuAsignado, init)
            ])
                .then(funciones.managedResponseAllPromisesToJson)
                .then(entidadDeportivaMantenimientoPage.methods.cargarListaPermisos);
        },
        cargarListaPermisos: function ([dataListaMenu, dataListaMenuAsignado]) {
            let itemList = dataListaMenu.status == "success" ? dataListaMenu.result || [] : [];
            let itemListAsignado = (dataListaMenuAsignado.status == "success" ? dataListaMenuAsignado.result || [] : []).filter(x => x.FlagActivo);
            let itemListAsignadoIds = itemListAsignado.map(x => x.MenuId);
            let menuHtml = funciones.renderiCheckBoxToHtml(null, itemList, 'ListaSubMenu', 'perfilmenu-entidaddeportiva', 'MenuId', 'Nombre');
            $('#div-lista-menu-content').html(menuHtml);
            Array.from($('input[type="checkbox"][name="perfilmenu-entidaddeportiva"]')).forEach(x => $(x).prop("checked", itemListAsignadoIds.some(y => y == x.value)));
            $(':input[id^="chk-perfilmenu-entidaddeportiva-"]').change(entidadDeportivaMantenimientoPage.events.chkMenuChecked);
            $('#frm-editar-lista-permisos').validate(entidadDeportivaMantenimientoPage.options.validatePerfilMenu);
            funciones.loading('#modal-editar-lista-permisos .modal-content').Close();
        },
        guardarAsignacionListaMenu: function () {
            let listaMenu = Array.from($('input[type="checkbox"][name="perfilmenu-entidaddeportiva"]')).map(x => Object.assign({}, { MenuId: x.value, FlagActivo: x.checked }));
            let body = {
                EntidadDeportivaId: entidadDeportivaId,
                ListaPerfil: [
                    {
                        PerfilId: entidadDeportivaMantenimientoPage.default.perfilIdSeleccionado,
                        ListaMenu: listaMenu
                    }
                ]
            };
            let url = `${urlBaseWebApi}/api/entidaddeportiva/guardar-lista-perfilmenu-por-entidaddeportiva`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarGuardarAsignacionListaMenu);
        },
        finalizarGuardarAsignacionListaMenu: function (data) {
            let title = data.status == "success" ? data.result ? "Se asignó la lista de permisos correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.defaultSwal.title = title;
            entidadDeportivaMantenimientoPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaMantenimientoPage.options.defaultSwal);
            $('#modal-editar-lista-permisos').modal('hide');
        },
        mostrarModalMantenimientoUsuario: function (id) {
            id = id == undefined ? null : id;
            entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado = id;
            entidadDeportivaMantenimientoPage.options.mantenimientoUsuarioSwal.title = id == null ? "Nuevo Usuario" : "Editar Usuario";
            bootbox.dialog(entidadDeportivaMantenimientoPage.options.mantenimientoUsuarioSwal);
        },
        guardarUsuario: function () {
            let tipoMantenimientoUsuario = entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado == null ? 1 : 2;
            let id = $('#txt-usuario-id').val();
            let contraseña = $('#txt-usuario-contraseña').val() || '';
            let correo = $('#txt-usuario-correo').val();
            let body = {
                EntidadDeportivaId: entidadDeportivaId,
                UsuarioId: id,
                Contraseña: contraseña,
                Correo: correo,
            };
            let url = `${urlBaseWebApi}/api/usuario/guardar-usuario/${tipoMantenimientoUsuario}`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarGuardar);
        },
        finalizarGuardar: function (data) {
            let title = data.status == "success" ? data.result ? "Se guardó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.defaultSwal.title = title;
            entidadDeportivaMantenimientoPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaMantenimientoPage.options.defaultSwal);
            $(entidadDeportivaMantenimientoPage.options.idBtnBuscarUsuario).trigger('click');
            $('#modal-mantenimiento-usuario').modal('hide');
        },
        buscarUsuario: function () {
            $(entidadDeportivaMantenimientoPage.options.idTblResultadosUsuario).DataTable().ajax.reload();
        },
        obtenerUsuario: function () {
            let { usuarioIdSeleccionado } = entidadDeportivaMantenimientoPage.default;
            if (usuarioIdSeleccionado == null) {
                //funciones.limpiarPreloader();
                return;
            }
            let url = `${urlBaseWebApi}/api/usuario/obtener-usuario/${entidadDeportivaId}/${usuarioIdSeleccionado}`;
            let init = {};
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.cargarUsuario);
        },
        cargarUsuario: function (data) {
            let { status, result } = data;
            if (status != "success") return;
            if (result == null) {
                //funciones.limpiarPreloader();
                //Swal.fire(entidadDeportivaMantenimientoPage.options.cargarUsuarioSwal)
                //    .then(entidadDeportivaMantenimientoPage.methods.redireccionarListaUsuarios);
                $('#modal-mantenimiento-usuario').modal('hide');
                return;
            }
            //entidadDeportivaMantenimientoPage.default.usuario = { ...result };
            $("#txt-usuario-id").val(result.UsuarioId);
            $("#txt-usuario-correo").val(result.Correo);
            //funciones.limpiarPreloader();
        },
        confirmarCambiarFlagActivoUsuario: function (id, flagActivo) {
            let title = "Confirmar";
            let text = `¿Desea ${(flagActivo ? "inactivar" : "activar")} el registro?`;
            entidadDeportivaMantenimientoPage.options.cambiarFlagActivoUsuarioSwal.title = title;
            entidadDeportivaMantenimientoPage.options.cambiarFlagActivoUsuarioSwal.text = text;
            Swal.fire(entidadDeportivaMantenimientoPage.options.cambiarFlagActivoUsuarioSwal)
                .then(entidadDeportivaMantenimientoPage.methods.cambiarFlagActivoUsuario.bind({ id, flagActivo }));
        },
        cambiarFlagActivoUsuario: function (result) {
            if (!result.isConfirmed) return;
            let url = `${urlBaseWebApi}/api/usuario/cambiar-flagactivo-usuario/${entidadDeportivaId}/${this.id}/${this.flagActivo}`;
            let init = {};
            init.method = 'PUT';
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarCambiarFlagActivoUsuario);
        },
        finalizarCambiarFlagActivoUsuario: function (data) {
            let title = data.status == "success" ? data.result ? "Se modificó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.defaultSwal.title = title;
            entidadDeportivaMantenimientoPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaMantenimientoPage.options.defaultSwal);
            $(entidadDeportivaMantenimientoPage.options.idBtnBuscarUsuario).trigger('click');
        },
        mostrarModalEditarListaPerfilesUsuario: function (id) {
            entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado = id;
            bootbox.dialog(entidadDeportivaMantenimientoPage.options.editarListaPerfilesUsuarioSwal);

        },
        obtenerListaPerfilesUsuario: function () {
            const { usuarioIdSeleccionado } = entidadDeportivaMantenimientoPage.default;
            let urlListaPerfil = `${urlBaseWebApi}/api/perfil/listar-perfil-por-flagactivo-entidaddeportiva/${entidadDeportivaId}/true`;
            let urlListaPerfilAsignado = `${urlBaseWebApi}/api/perfil/listar-perfil-por-flagactivo-usuario/${entidadDeportivaId}/${usuarioIdSeleccionado}/true`;
            let init = {};
            init.credentials = 'include';
            funciones.loading('#modal-editar-lista-perfiles .modal-content').Open();

            Promise.all([
                fetch(urlListaPerfil, init),
                fetch(urlListaPerfilAsignado, init)
            ])
                .then(funciones.managedResponseAllPromisesToJson)
                .then(entidadDeportivaMantenimientoPage.methods.cargarListaPerfilesUsuario);
        },
        cargarListaPerfilesUsuario: function ([dataListaPerfil, dataListaPerfilAsignado]) {
            let itemList = dataListaPerfil.status == "success" ? dataListaPerfil.result || [] : [];
            let itemListAsignado = (dataListaPerfilAsignado.status == "success" ? dataListaPerfilAsignado.result || [] : []).filter(x => x.FlagActivo);
            let itemListAsignadoIds = itemListAsignado.map(x => x.PerfilId);
            let perfilHtml = funciones.renderiCheckBoxToHtml(null, itemList, null, 'usuarioperfil', 'PerfilId', 'Nombre');
            $('#div-lista-perfil-content').html(perfilHtml);
            Array.from($('input[type="checkbox"][name="usuarioperfil"]')).forEach(x => $(x).prop("checked", itemListAsignadoIds.some(y => y == x.value)));
            //$(':input[id^="chk-usuarioperfil-"]').change(entidadDeportivaMantenimientoPage.events.chkPerfilChecked);
            $('#frm-editar-lista-perfiles').validate(entidadDeportivaMantenimientoPage.options.validateUsuarioPerfil);
            funciones.loading('#modal-editar-lista-perfiles .modal-content').Close();
        },
        guardarAsignacionListaPerfilUsuario: function () {
            let listaPerfil= Array.from($('input[type="checkbox"][name="usuarioperfil"]')).map(x => Object.assign({}, { PerfilId: x.value, FlagActivo: x.checked }));
            let body = {
                EntidadDeportivaId: entidadDeportivaId,
                UsuarioId: entidadDeportivaMantenimientoPage.default.usuarioIdSeleccionado,
                ListaPerfil: listaPerfil
            }
            let url = `${urlBaseWebApi}/api/usuario/guardar-lista-perfil-por-usuario`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaMantenimientoPage.methods.finalizarGuardarAsignacionListaPerfilUsuario);
        },
        finalizarGuardarAsignacionListaPerfilUsuario: function (data) {
            let title = data.status == "success" ? data.result ? "Se asignó la lista de perfiles correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaMantenimientoPage.options.defaultSwal.title = title;
            entidadDeportivaMantenimientoPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaMantenimientoPage.options.defaultSwal);
            $('#modal-editar-lista-perfiles').modal('hide');
        }
    }
}