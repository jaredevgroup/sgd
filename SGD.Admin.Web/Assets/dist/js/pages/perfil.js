const perfilPage = {
    init: function () {
        perfilPage.initEvents();
        funciones.limpiarPreloader();
    },
    initEvents: function () {
        $(perfilPage.options.idBtnBuscar).click(perfilPage.events.btnBuscarClick);
        $(perfilPage.options.idTblResultados)
            .DataTable(perfilPage.options.dataTable)
            .on("draw", perfilPage.events.tblResultadosOnDraw)
            .on("preXhr", perfilPage.events.tblResultadosOnPreXhr)
            .on("xhr", perfilPage.events.tblResultadosOnXhr);
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
                url: `${urlBaseWebApi}/api/perfilsistema/buscar-perfilsistema`,
                data: function (params) {
                    var [columnIndex, sortOrder] = $(perfilPage.options.idTblResultados).DataTable().order()[0];
                    var sortName = $(perfilPage.options.idTblResultados).DataTable().column(columnIndex).dataSrc();
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
                { data: 'PerfilId', title: 'Opciones', orderable: false, render: (data, type, row) => `${(row.FlagActivo ? `<a href="${urlBase}perfil/editar/${data}" class="btn btn-info btn-xs" data-toggle="tooltip" title="Ir a editar"><i class="fa fa-pen"></i></a>&nbsp;` : "")}<button type="button" class="btn btn-${(row.FlagActivo ? "danger" : "success")} btn-xs btn-confirmar-cambiar-flagactivo" data-item-id="${data}" data-item-flagactivo="${row.FlagActivo}" data-toggle="tooltip" title="${(row.FlagActivo ? "Inactivar" : "Activar")}"><i class="fa fa-${(row.FlagActivo ? "ban" : "check")}"></i></button>&nbsp;<button type="button" class="btn btn-primary btn-xs btn-editar-lista-permisos" data-item-id="${data}" data-toggle="tooltip" title="Asignar Permisos"><i class="fa fa-list"></i></button>` }
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
                perfilPage.methods.guardarAsignacionListaMenu();
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
                perfilPage.methods.obtenerListaPermisos();
            },
            onHide: function (e) {
                perfilPage.default.perfilIdSeleccionado = null;
            },
            animateIn: 'zoomInDown',
            animateOut: 'zoomOutUp'
        }
    },
    events: {
        tblResultadosOnDraw: function (e) {
            $(e.currentTarget).find('thead tr').addClass('bg-secondary')
            $(e.currentTarget).find('.btn-confirmar-cambiar-flagactivo').click(perfilPage.events.btnConfirmarCambiarFlagActivoClick);
            $(e.currentTarget).find('.btn-editar-lista-permisos').click(perfilPage.events.btnEditarListaPermisosClick);
            $('[data-toggle="tooltip"]').tooltip();
        },
        tblResultadosOnPreXhr: function (e) {
            $(perfilPage.options.idBtnBuscar).prop('disabled', true);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Open();
        },
        tblResultadosOnXhr: function (e) {
            $(perfilPage.options.idBtnBuscar).prop('disabled', false);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Close();
        },
        btnBuscarClick: function (e) {
            perfilPage.methods.buscar();
        },
        btnConfirmarCambiarFlagActivoClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            let flagActivo = eval($(e.currentTarget).attr("data-item-flagactivo"));
            perfilPage.methods.confirmarCambiarFlagActivo(id, flagActivo);
        },
        btnEditarListaPermisosClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            perfilPage.methods.mostrarModalEditarListaPermisos(id);
        },
        chkMenuChecked: function (e) {
            funciones.treeCheckBoxChecked(e.currentTarget);
        }
    },
    methods: {
        buscar: function () {
            $(perfilPage.options.idTblResultados).DataTable().ajax.reload();
        },
        confirmarCambiarFlagActivo: function (id, flagActivo) {
            let title = "Confirmar";
            let text = `¿Desea ${(flagActivo ? "inactivar" : "activar")} el registro?`;
            perfilPage.options.cambiarFlagActivoSwal.title = title;
            perfilPage.options.cambiarFlagActivoSwal.text = text;
            Swal.fire(perfilPage.options.cambiarFlagActivoSwal)
                .then(perfilPage.methods.cambiarFlagActivo.bind({ id, flagActivo }));
        },
        cambiarFlagActivo: function (result) {
            if (!result.isConfirmed) return;
            let url = `${urlBaseWebApi}/api/perfilsistema/cambiar-flagactivo-perfilsistema/${this.id}/${this.flagActivo}`;
            let init = {};
            init.method = 'PUT';
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(perfilPage.methods.finalizarCambiarFlagActivo);
        },
        finalizarCambiarFlagActivo: function (data) {
            let title = data.status == "success" ? data.result ? "Se modificó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            perfilPage.options.defaultSwal.title = title;
            perfilPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', perfilPage.options.defaultSwal);
            $(perfilPage.options.idBtnBuscar).trigger('click');
        },
        mostrarModalEditarListaPermisos: function (id) {
            perfilPage.default.perfilIdSeleccionado = id;
            bootbox.dialog(perfilPage.options.editarListaPermisosSwal);
            
        },
        obtenerListaPermisos: function () {
            const { perfilIdSeleccionado } = perfilPage.default;
            let urlListaMenu = `${urlBaseWebApi}/api/menusistema/listar-menusistema-por-flagactivo-menuidpadre/true/true`;
            let urlListaMenuAsignado = `${urlBaseWebApi}/api/menusistema/listar-menusistema-por-flagactivo-perfilid/${perfilIdSeleccionado}/true`;
            let init = {};
            init.credentials = 'include';
            funciones.loading('#modal-editar-lista-permisos .modal-content').Open();

            Promise.all([
                fetch(urlListaMenu, init),
                fetch(urlListaMenuAsignado, init)
            ])
                .then(funciones.managedResponseAllPromisesToJson)
                .then(perfilPage.methods.cargarListaPermisos);
        },
        cargarListaPermisos: function ([dataListaMenu, dataListaMenuAsignado]) {
            let itemList = dataListaMenu.status == "success" ? dataListaMenu.result || [] : [];
            let itemListAsignado = (dataListaMenuAsignado.status == "success" ? dataListaMenuAsignado.result || [] : []).filter(x => x.FlagActivo);
            let itemListAsignadoIds = itemListAsignado.map(x => x.MenuId);
            let menuHtml = perfilPage.methods.renderMenuToHtml(null, itemList);
            $('#div-lista-menu-content').html(menuHtml);
            Array.from($('input[type="checkbox"][name="menu"]')).forEach(x => $(x).prop("checked", itemListAsignadoIds.some(y => y == x.value)));
            $(':input[id^="chk-menu-"]').change(perfilPage.events.chkMenuChecked);
            $('#frm-editar-lista-permisos').validate(perfilPage.options.validate);
            funciones.loading('#modal-editar-lista-permisos .modal-content').Close();
        },
        renderMenuToHtml: function (item, listaSubMenu) {
            let html = '';
            let htmlList = '';

            if (listaSubMenu != null) {
                for (let x of listaSubMenu) {
                    let xHtml = perfilPage.methods.renderMenuToHtml(x, x.ListaSubMenu);

                    htmlList += xHtml;
                }
            }

            if (item != null) {
                html = `
                    <div class="icheck-primary">
                        <input type="checkbox" name="menu" id="chk-menu-${item.MenuId}" value="${item.MenuId}">
                        <label for="chk-menu-${item.MenuId}">
                            ${item.Nombre}
                        </label>
                        ${htmlList}
                    </div>
                `;
            } else {
                html = htmlList;
            }

            return html;
        },
        guardarAsignacionListaMenu: function () {
            let listaMenuSistema = Array.from($('input[type="checkbox"][name="menu"]')).map(x => Object.assign({}, { MenuId: x.value, FlagActivo: x.checked }));
            let body = {
                PerfilId: perfilPage.default.perfilIdSeleccionado,
                ListaMenuSistema: listaMenuSistema
            }
            let url = `${urlBaseWebApi}/api/perfilsistema/guardar-lista-menusistema-por-perfil`;
            let init = {};
            init.method = 'POST';
            init.credentials = 'include';
            init.body = JSON.stringify(body);
            init.headers = {};
            init.headers['Content-Type'] = "application/json";
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(perfilPage.methods.finalizarGuardarAsignacionListaMenu);
        },
        finalizarGuardarAsignacionListaMenu: function (data) {
            let title = data.status == "success" ? data.result ? "Se asignó la lista de permisos correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            perfilPage.options.defaultSwal.title = title;
            perfilPage.options.defaultSwal.icon = icon;
            $(document).Toasts('create', perfilPage.options.defaultSwal);
            $('#modal-editar-lista-permisos').modal('hide');
        }
    }
}