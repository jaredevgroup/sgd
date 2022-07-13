const perfilSGDPage = {
    init: function () {
        perfilSGDPage.initEvents();
        funciones.limpiarPreloader();
    },
    initEvents: function () {
        $(perfilSGDPage.options.idBtnBuscar).click(perfilSGDPage.events.btnBuscarClick);
        $(perfilSGDPage.options.idTblResultados)
            .DataTable(perfilSGDPage.options.dataTable)
            .on("draw", perfilSGDPage.events.tblResultadosOnDraw)
            .on("preXhr", perfilSGDPage.events.tblResultadosOnPreXhr)
            .on("xhr", perfilSGDPage.events.tblResultadosOnXhr);
    },
    default: {
        perfilIdSeleccionado: null
    },
    options: {
        idTblResultados: '#tbl-resultados',
        idBtnBuscar: '#btn-buscar',
        dataTable: {
            processing: true,
            serverSide: true,
            ajax: {
                url: `${urlBaseWebApi}/api/perfil/buscar-perfil`,
                data: function (params) {
                    var [columnIndex, sortOrder] = $(perfilSGDPage.options.idTblResultados).DataTable().order()[0];
                    var sortName = $(perfilSGDPage.options.idTblResultados).DataTable().column(columnIndex).dataSrc();
                    params.nombre = () => $('#txt-filtro-nombre').val();
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
                { data: 'Nombre', title: 'Nombre', orderable: true },
                { data: 'FlagActivo', title: 'Estado', orderable: true, render: (data) => data ? "Activo" : "Inactivo" },
                { data: 'PerfilId', title: 'Opciones', orderable: false, render: (data, type, row) => `${(row.FlagActivo ? `<a href="${urlBase}sgd-perfil/editar/${data}" class="btn btn-info btn-xs" data-toggle="tooltip" title="Ir a editar"><i class="fa fa-pen"></i></a>&nbsp;` : "")}<button type="button" class="btn btn-${(row.FlagActivo ? "danger" : "success")} btn-xs btn-confirmar-cambiar-flagactivo" data-item-id="${data}" data-item-flagactivo="${row.FlagActivo}" data-toggle="tooltip" title="${(row.FlagActivo ? "Inactivar" : "Activar")}"><i class="fa fa-${(row.FlagActivo ? "ban" : "check")}"></i></button>` }
                //{ data: 'PerfilId', title: 'Opciones', orderable: false, render: (data, type, row) => `${(row.FlagActivo ? `<a href="${urlBase}sgd-perfil/editar/${data}" class="btn btn-info btn-xs" data-toggle="tooltip" title="Ir a editar"><i class="fa fa-pen"></i></a>&nbsp;` : "")}<button type="button" class="btn btn-${(row.FlagActivo ? "danger" : "success")} btn-xs btn-confirmar-cambiar-flagactivo" data-item-id="${data}" data-item-flagactivo="${row.FlagActivo}" data-toggle="tooltip" title="${(row.FlagActivo ? "Inactivar" : "Activar")}"><i class="fa fa-${(row.FlagActivo ? "ban" : "check")}"></i></button>&nbsp;<button type="button" class="btn btn-primary btn-xs btn-editar-lista-permisos" data-item-id="${data}" data-toggle="tooltip" title="Asignar Permisos"><i class="fa fa-list"></i></button>` }
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
                'menu': {
                    //required: true,
                    minlength: 1
                }
            },
            messages: {
                'menu': {
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
                perfilSGDPage.methods.guardarAsignacionListaMenu();
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
        //editarListaPermisosSwal: {
        //    title: "Asignar Menús",
        //    message:
        //        `<form id="frm-editar-lista-permisos" autocomplete="off">
        //            <div class="row">
        //                <div id="div-lista-menu-content" class="col-sm-12"></div>
        //            </div>
        //        </form>`,
        //    closeButton: true,
        //    buttons: {
        //        guardar: {
        //            label: 'Guardar',
        //            className: 'btn-success',
        //            callback: function () {
        //                $('#frm-editar-lista-permisos').trigger("submit");
        //                return false;
        //            },

        //        },
        //        cancelar: {
        //            label: 'Cancelar',
        //            classname: 'btn-danger',
        //        }
        //    },
        //    onShow: function (e) {
        //        $(e.currentTarget).attr("id", "modal-editar-lista-permisos");
        //        perfilSGDPage.methods.obtenerListaPermisos();
        //    },
        //    onHide: function (e) {
        //        perfilSGDPage.default.perfilIdSeleccionado = null;
        //    },
        //    animateIn: 'zoomInDown',
        //    animateOut: 'zoomOutUp'
        //}
    },
    events: {
        tblResultadosOnDraw: function (e) {
            $(e.currentTarget).find('thead tr').addClass('bg-secondary')
            $(e.currentTarget).find('.btn-confirmar-cambiar-flagactivo').click(perfilSGDPage.events.btnConfirmarCambiarFlagActivoClick);
            //$(e.currentTarget).find('.btn-editar-lista-permisos').click(perfilSGDPage.events.btnEditarListaPermisosClick);
            $('[data-toggle="tooltip"]').tooltip();
        },
        tblResultadosOnPreXhr: function (e) {
            $(perfilSGDPage.options.idBtnBuscar).prop('disabled', true);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Open();
        },
        tblResultadosOnXhr: function (e) {
            $(perfilSGDPage.options.idBtnBuscar).prop('disabled', false);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Close();
        },
        btnBuscarClick: function (e) {
            perfilSGDPage.methods.buscar();
        },
        btnConfirmarCambiarFlagActivoClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            let flagActivo = eval($(e.currentTarget).attr("data-item-flagactivo"));
            perfilSGDPage.methods.confirmarCambiarFlagActivo(id, flagActivo);
        },
        //btnEditarListaPermisosClick: function (e) {
        //    let id = parseInt($(e.currentTarget).attr("data-item-id"));
        //    perfilSGDPage.methods.mostrarModalEditarListaPermisos(id);
        //},
        chkMenuChecked: function (e) {
            funciones.treeCheckBoxChecked(e.currentTarget);
        }
    },
    methods: {
        buscar: function () {
            $(perfilSGDPage.options.idTblResultados).DataTable().ajax.reload();
        },
        confirmarCambiarFlagActivo: function (id, flagActivo) {
            let title = "Confirmar";
            let text = `¿Desea ${(flagActivo ? "inactivar" : "activar")} el registro?`;
            perfilSGDPage.options.cambiarFlagActivoSwal.title = title;
            perfilSGDPage.options.cambiarFlagActivoSwal.text = text;
            Swal.fire(perfilSGDPage.options.cambiarFlagActivoSwal)
                .then(perfilSGDPage.methods.cambiarFlagActivo.bind({ id, flagActivo }));
        },
        cambiarFlagActivo: function (result) {
            if (!result.isConfirmed) return;
            let url = `${urlBaseWebApi}/api/perfil/cambiar-flagactivo-perfil/${this.id}/${this.flagActivo}`;
            let init = {};
            init.method = 'PUT';
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(perfilSGDPage.methods.finalizarCambiarFlagActivo);
        },
        finalizarCambiarFlagActivo: function (data) {
            let title = data.status == "success" ? data.result ? "Se modificó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            perfilSGDPage.options.defaultSwal.title = title;
            perfilSGDPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', perfilSGDPage.options.defaultSwal);
            $(perfilSGDPage.options.idBtnBuscar).trigger('click');
        },
        //mostrarModalEditarListaPermisos: function (id) {
        //    perfilSGDPage.default.perfilIdSeleccionado = id;
        //    bootbox.dialog(perfilSGDPage.options.editarListaPermisosSwal);
        //},
        //obtenerListaPermisos: function () {
        //    const { perfilIdSeleccionado } = perfilSGDPage.default;
        //    let urlListaMenu = `${urlBaseWebApi}/api/menu/listar-menu-por-flagactivo-menuidpadre/true/true`;
        //    let urlListaMenuAsignado = `${urlBaseWebApi}/api/menu/listar-menu-por-flagactivo-perfilid/${perfilIdSeleccionado}/true`;
        //    let init = {};
        //    init.credentials = 'include';
        //    funciones.loading('#modal-editar-lista-permisos .modal-content').Open();

        //    Promise.all([
        //        fetch(urlListaMenu, init),
        //        fetch(urlListaMenuAsignado, init)
        //    ])
        //        .then(funciones.managedResponseAllPromisesToJson)
        //        .then(perfilSGDPage.methods.cargarListaPermisos);
        //},
        //cargarListaPermisos: function ([dataListaMenu, dataListaMenuAsignado]) {
        //    let itemList = dataListaMenu.status == "success" ? dataListaMenu.result || [] : [];
        //    let itemListAsignado = (dataListaMenuAsignado.status == "success" ? dataListaMenuAsignado.result || [] : []).filter(x => x.FlagActivo);
        //    let itemListAsignadoIds = itemListAsignado.map(x => x.MenuId);
        //    let menuHtml = funciones.renderiCheckBoxToHtml(null, itemList, 'ListaSubMenu', 'menu', 'MenuId', 'Nombre');
        //    $('#div-lista-menu-content').html(menuHtml);
        //    Array.from($('input[type="checkbox"][name="menu"]')).forEach(x => $(x).prop("checked", itemListAsignadoIds.some(y => y == x.value)));
        //    $(':input[id^="chk-menu-"]').change(perfilSGDPage.events.chkMenuChecked);
        //    $('#frm-editar-lista-permisos').validate(perfilSGDPage.options.validate);
        //    funciones.loading('#modal-editar-lista-permisos .modal-content').Close();
        //},
        //guardarAsignacionListaMenu: function () {
        //    let listaMenu = Array.from($('input[type="checkbox"][name="menu"]')).map(x => Object.assign({}, { MenuId: x.value, FlagActivo: x.checked }));
        //    let body = {
        //        PerfilId: perfilSGDPage.default.perfilIdSeleccionado,
        //        ListaMenu: listaMenu
        //    }
        //    let url = `${urlBaseWebApi}/api/perfil/guardar-lista-menu-por-perfil`;
        //    let init = {};
        //    init.method = 'POST';
        //    init.credentials = 'include';
        //    init.body = JSON.stringify(body);
        //    init.headers = {};
        //    init.headers['Content-Type'] = "application/json";
        //    fetch(url, init)
        //        .then(funciones.managedResponseFetchToJson)
        //        .then(perfilSGDPage.methods.finalizarGuardarAsignacionListaMenu);
        //},
        //finalizarGuardarAsignacionListaMenu: function (data) {
        //    let title = data.status == "success" ? data.result ? "Se asignó la lista de permisos correctamente" : data.message : "Ocurrió un error";
        //    let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
        //    perfilSGDPage.options.defaultSwal.title = title;
        //    perfilSGDPage.options.defaultSwal.icon = icon;
        //    $(document).Toasts('create', perfilSGDPage.options.defaultSwal);
        //    $('#modal-editar-lista-permisos').modal('hide');
        //}
    }
}