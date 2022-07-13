const usuarioPage = {
    init: function () {
        usuarioPage.initEvents();
        funciones.limpiarPreloader();
    },
    initEvents: function () {
        $(usuarioPage.options.idBtnBuscar).click(usuarioPage.events.btnBuscarClick);
        $(usuarioPage.options.idTblResultados)
            .DataTable(usuarioPage.options.dataTable)
            .on("draw", usuarioPage.events.tblResultadosOnDraw)
            .on("preXhr", usuarioPage.events.tblResultadosOnPreXhr)
            .on("xhr", usuarioPage.events.tblResultadosOnXhr);
    },
    default: {
        usuarioIdSeleccionado: null
    },
    options: {
        idTblResultados: '#tbl-resultados',
        idBtnBuscar: '#btn-buscar',
        dataTable: {
            processing: true,
            serverSide: true,
            ajax: {
                url: `${urlBaseWebApi}/api/usuariosistema/buscar-usuariosistema`,
                data: function (params) {
                    var [columnIndex, sortOrder] = $(usuarioPage.options.idTblResultados).DataTable().order()[0];
                    var sortName = $(usuarioPage.options.idTblResultados).DataTable().column(columnIndex).dataSrc();
                    params.nombre = () => $('#txt-filtro-nombre').val();
                    params.correo = () => $('#txt-filtro-correo').val();
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
                { data: 'UsuarioId', title: 'Opciones', orderable: false, render: (data, type, row) => `${(row.FlagActivo ? `<a href="${urlBase}usuario/editar/${data}" class="btn btn-info btn-xs" data-toggle="tooltip" title="Ir a editar"><i class="fa fa-pen"></i></a>&nbsp;` : "")}<button type="button" class="btn btn-${(row.FlagActivo ? "danger" : "success")} btn-xs btn-confirmar-cambiar-flagactivo" data-item-id="${data}" data-item-flagactivo="${row.FlagActivo}" data-toggle="tooltip" title="${(row.FlagActivo ? "Inactivar" : "Activar")}"><i class="fa fa-${(row.FlagActivo ? "ban" : "check")}"></i></button>&nbsp;<button type="button" class="btn btn-primary btn-xs btn-editar-lista-perfiles" data-item-id="${data}" data-toggle="tooltip" title="Asignar Perfiles"><i class="fa fa-list"></i></button>` }
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
        validate: {
            rules: {
                'perfil': {
                    //required: true,
                    minlength: 1
                }
            },
            messages: {
                'perfil': {
                    //required: "Marque mínimo una opción",
                    minlength: "Debe marcar mínimo una opción"
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
                usuarioPage.methods.guardarAsignacionListaPerfil();
            }
        },
        cambiarFlagActivoSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
        },
        defaultSwal: {
            class: 'bg-success',
            autohide: true,
            delay: 3000,
            close: false
        },
        editarListaPerfilesSwal: {
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
        tblResultadosOnDraw: function (e) {
            $(e.currentTarget).find('thead tr').addClass('bg-secondary')
            $(e.currentTarget).find('.btn-confirmar-cambiar-flagactivo').click(usuarioPage.events.btnConfirmarCambiarFlagActivoClick);
            $(e.currentTarget).find('.btn-editar-lista-perfiles').click(usuarioPage.events.btnEditarListaPerfilesClick);
            $('[data-toggle="tooltip"]').tooltip();
        },
        tblResultadosOnPreXhr: function (e) {
            $(usuarioPage.options.idBtnBuscar).prop('disabled', true);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Open();
        },
        tblResultadosOnXhr: function (e) {
            $(usuarioPage.options.idBtnBuscar).prop('disabled', false);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Close();
        },
        btnBuscarClick: function (e) {
            usuarioPage.methods.buscar();
        },
        btnConfirmarCambiarFlagActivoClick: function (e) {
            let id = $(e.currentTarget).attr("data-item-id");
            let flagActivo = eval($(e.currentTarget).attr("data-item-flagactivo"));
            usuarioPage.methods.confirmarCambiarFlagActivo(id, flagActivo);
        },
        btnEditarListaPerfilesClick: function (e) {
            let id = $(e.currentTarget).attr("data-item-id");
            usuarioPage.methods.mostrarModalEditarListaPerfiles(id);
        },
        //chkPerfilChecked: function (e) {
        //    funciones.treeCheckBoxChecked(e.currentTarget);
        //}
    },
    methods: {
        buscar: function () {
            $(usuarioPage.options.idTblResultados).DataTable().ajax.reload();
        },
        confirmarCambiarFlagActivo: function (id, flagActivo) {
            let title = "Confirmar";
            let text = `¿Desea ${(flagActivo ? "inactivar" : "activar")} el registro?`;
            usuarioPage.options.cambiarFlagActivoSwal.title = title;
            usuarioPage.options.cambiarFlagActivoSwal.text = text;
            Swal.fire(usuarioPage.options.cambiarFlagActivoSwal)
                .then(usuarioPage.methods.cambiarFlagActivo.bind({ id, flagActivo }));
        },
        cambiarFlagActivo: function (result) {
            if (!result.isConfirmed) return;
            let url = `${urlBaseWebApi}/api/usuariosistema/cambiar-flagactivo-usuariosistema/${this.id}/${this.flagActivo}`;
            let init = {};
            init.method = 'PUT';
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(usuarioPage.methods.finalizarCambiarFlagActivo);
        },
        finalizarCambiarFlagActivo: function (data) {
            let title = data.status == "success" ? data.result ? "Se modificó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            usuarioPage.options.defaultSwal.title = title;
            usuarioPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', usuarioPage.options.defaultSwal);
            $(usuarioPage.options.idBtnBuscar).trigger('click');
        },
        mostrarModalEditarListaPerfiles: function (id) {
            usuarioPage.default.usuarioIdSeleccionado = id;
            bootbox.dialog(usuarioPage.options.editarListaPerfilesSwal);

        },
        obtenerListaPerfiles: function () {
            const { usuarioIdSeleccionado } = usuarioPage.default;
            let urlListaPerfil = `${urlBaseWebApi}/api/perfilsistema/listar-perfilsistema-por-flagactivo`;
            let urlListaPerfilAsignado = `${urlBaseWebApi}/api/perfilsistema/listar-perfilsistema-por-flagactivo-usuarioid/${usuarioIdSeleccionado}/true`;
            let init = {};
            init.credentials = 'include';
            funciones.loading('#modal-editar-lista-perfiles .modal-content').Open();

            Promise.all([
                fetch(urlListaPerfil, init),
                fetch(urlListaPerfilAsignado, init)
            ])
                .then(funciones.managedResponseAllPromisesToJson)
                .then(usuarioPage.methods.cargarListaPerfiles);
        },
        cargarListaPerfiles: function ([dataListaPerfil, dataListaPerfilAsignado]) {
            let itemList = dataListaPerfil.status == "success" ? dataListaPerfil.result || [] : [];
            let itemListAsignado = (dataListaPerfilAsignado.status == "success" ? dataListaPerfilAsignado.result || [] : []).filter(x => x.FlagActivo);
            let itemListAsignadoIds = itemListAsignado.map(x => x.PerfilId);
            let perfilHtml = funciones.renderiCheckBoxToHtml(null, itemList, null, 'perfil', 'PerfilId', 'Nombre');
            //let perfilHtml = usuarioPage.methods.renderPerfilToHtml(null, itemList);
            $('#div-lista-perfil-content').html(perfilHtml);
            Array.from($('input[type="checkbox"][name="perfil"]')).forEach(x => $(x).prop("checked", itemListAsignadoIds.some(y => y == x.value)));
            //$(':input[id^="chk-perfil-"]').change(usuarioPage.events.chkPerfilChecked);
            $('#frm-editar-lista-perfiles').validate(usuarioPage.options.validate);
            funciones.loading('#modal-editar-lista-perfiles .modal-content').Close();
        },
        //renderPerfilToHtml: function (item, listaSubPerfil) {
        //    let html = '';
        //    let htmlList = '';

        //    if (listaSubPerfil != null) {
        //        for (let x of listaSubPerfil) {
        //            let xHtml = usuarioPage.methods.renderPerfilToHtml(x, x.ListaSubPerfil);

        //            htmlList += xHtml;
        //        }
        //    }

        //    if (item != null) {
        //        html = `
        //            <div class="icheck-primary">
        //                <input type="checkbox" name="perfil" id="chk-perfil-${item.PerfilId}" value="${item.PerfilId}">
        //                <label for="chk-perfil-${item.PerfilId}">
        //                    ${item.Nombre}
        //                </label>
        //                ${htmlList}
        //            </div>
        //        `;
        //    } else {
        //        html = htmlList;
        //    }

        //    return html;
        //},
        guardarAsignacionListaPerfil: function () {
            let listaPerfilSistema = Array.from($('input[type="checkbox"][name="perfil"]')).map(x => Object.assign({}, { PerfilId: x.value, FlagActivo: x.checked }));
            let body = {
                UsuarioId: usuarioPage.default.usuarioIdSeleccionado,
                ListaPerfilSistema: listaPerfilSistema
            }
            let url = `${urlBaseWebApi}/api/usuariosistema/guardar-lista-perfilsistema-por-usuario`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(usuarioPage.methods.finalizarGuardarAsignacionListaPerfil);
        },
        finalizarGuardarAsignacionListaPerfil: function (data) {
            let title = data.status == "success" ? data.result ? "Se asignó la lista de perfiles correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            usuarioPage.options.defaultSwal.title = title;
            usuarioPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', usuarioPage.options.defaultSwal);
            $('#modal-editar-lista-perfiles').modal('hide');
        }
    }
}