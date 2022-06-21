const entidadDeportivaPage = {
    init: function () {
        entidadDeportivaPage.initEvents();
        funciones.limpiarPreloader();
    },
    initEvents: function () {
        let options = { ...entidadDeportivaPage.options };
        let events = { ...entidadDeportivaPage.events };
        $(options.idBtnBuscar).click(events.btnBuscarClick);
        $(options.idTblResultados)
            .DataTable(options.dataTable)
            .on("draw", events.tblResultadosOnDraw)
            .on("preXhr", events.tblResultadosOnPreXhr)
            .on("xhr", events.tblResultadosOnXhr);
    },
    options: {
        idTblResultados: '#tbl-resultados',
        idBtnBuscar: '#btn-buscar',
        dataTable: {
            processing: true,
            serverSide: true,
            ajax: {
                url: `${urlBaseWebApi}/api/entidaddeportiva/buscar-entidaddeportiva`,
                data: function (params, settings) {
                    let [columnIndex, sortOrder] = $(settings.nTable).DataTable().order()[0];
                    let sortName = $(settings.nTable).DataTable().column(columnIndex).dataSrc();

                    params.codigo = () => $('#txt-filtro-codigo').val();
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
                { data: 'Codigo', title: 'Código' },
                { data: 'Nombre', title: 'Nombre' },
                { data: 'FlagActivo', title: 'Estado', render: (data) => data ? "Activo" : "Inactivo" },
                {
                    data: 'EntidadDeportivaId', title: 'Opciones', orderable: false, render: (data, type, row, context) => `${(row.FlagActivo ? `<a href="${urlBase}entidad-deportiva/editar/${data}${(row.EntidadDeportivaIdPadre == null ? "" : `/${row.EntidadDeportivaIdPadre}`)}" class="btn btn-info btn-xs" data-toggle="tooltip" title="Ir a editar"><i class="fa fa-pen"></i></a>&nbsp;` : "")}<button type="button" class="btn btn-${(row.FlagActivo ? "danger" : "success")} btn-xs btn-confirmar-cambiar-flagactivo" data-item-id="${data}" data-item-flagactivo="${row.FlagActivo}" data-toggle="tooltip" title="${(row.FlagActivo ? "Inactivar" : "Activar")}"><i class="fa fa-${(row.FlagActivo ? "ban" : "check")}"></i></button>`
                }
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
        cambiarFlagActivoSwal: {
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Si, de acuerdo',
            cancelButtonText: 'Cancelar',
            icon: 'warning',
        },
        finalizarCambiarFlagActivoSwal: {
            class: 'bg-success',
            autohide: true,
            delay: 3000,
            close: false
        }
    },
    events: {
        tblResultadosOnDraw: function (e) {
            let events = { ...entidadDeportivaPage.events };
            $(e.currentTarget).find('thead tr').addClass('bg-secondary');
            $(e.currentTarget).find('.btn-confirmar-cambiar-flagactivo').click(events.btnConfirmarCambiarFlagActivoClick);
            $(e.currentTarget).find('.btn-details').unbind("click");
            $(e.currentTarget).find('.btn-details').click(events.btnDetailsClick);
            $(e.currentTarget).find('.btn-cambiar-orden-arriba').unbind("click");
            $(e.currentTarget).find('.btn-cambiar-orden-arriba').click(events.btnCambiarOrdenArribaClick);
            $(e.currentTarget).find('.btn-cambiar-orden-abajo').unbind("click");
            $(e.currentTarget).find('.btn-cambiar-orden-abajo').click(events.btnCambiarOrdenAbajoClick);
            $('[data-toggle="tooltip"]').tooltip();
        },
        tblResultadosOnPreXhr: function (e) {
            if ($(e.currentTarget.id == entidadDeportivaPage.options.idTblResultados)) $(entidadDeportivaPage.options.idBtnBuscar).prop('disabled', true);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Open();
        },
        tblResultadosOnXhr: function (e) {
            if ($(e.currentTarget.id == entidadDeportivaPage.options.idTblResultados)) $(entidadDeportivaPage.options.idBtnBuscar).prop('disabled', false);
            funciones.loading(`#${e.currentTarget.parentElement.parentElement.parentElement.id}`).Close();
        },
        btnBuscarClick: function (e) {
            entidadDeportivaPage.methods.buscar();
        },
        btnDetailsClick: function (e) {
            let row = $(e.currentTarget).closest('tr');
            let nombre = $(e.currentTarget).attr("data-item-nombre");
            let entidadDeportivaId = $(e.currentTarget).attr("data-item-id");
            let cantidadColumnas = $(row).closest('table').find('thead tr th').length;

            let flagAbierto = $(row).next().find('table').length > 0;

            const options = { ...entidadDeportivaPage.options };

            if (!flagAbierto) {
                let rowNext = $(`
                <tr>
                    <td colspan="${cantidadColumnas}">
                        <div class="card card-secondary">
                            <div class="card-header">
                                <h3 class="card-title text-bold">Resultados del Menú ${nombre}</h3>
                            </div>
                            <div class="card-body">
                                <a href="${urlBase}entidadDeportiva/nuevo/${entidadDeportivaId}" class="btn btn-primary">Nuevo</a>
                                <table id="tbl-resultados-${entidadDeportivaId}" data-item-id="${entidadDeportivaId}" class="table table-striped table-bordered table-sm"></table>
                            </div>
                        </div>
                    </td>
                </tr>
                `);
                $(e.currentTarget).find('i').removeClass('fa-plus').addClass('fa-minus');
                let events = { ...entidadDeportivaPage.events };
                rowNext.find(`${options.idTblResultados}-${entidadDeportivaId}`).DataTable(options.dataTable)
                    .on("draw", events.tblResultadosOnDraw)
                    .on("preXhr", events.tblResultadosOnPreXhr)
                    .on("xhr", events.tblResultadosOnXhr);
                $(row).after(rowNext);
            }
            else {
                $(e.currentTarget).find('i').removeClass('fa-minus').addClass('fa-plus');
                $(row).next().remove();
            }
        },
        btnConfirmarCambiarFlagActivoClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            let flagActivo = eval($(e.currentTarget).attr("data-item-flagactivo"));
            entidadDeportivaPage.methods.confirmarCambiarFlagActivo(id, flagActivo);
        },
        btnCambiarOrdenArribaClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            let orden = eval($(e.currentTarget).attr("data-item-orden"));
            let nuevoOrden = orden - 1;
            entidadDeportivaPage.methods.cambiarOrden({ id, orden: nuevoOrden });
        },
        btnCambiarOrdenAbajoClick: function (e) {
            let id = parseInt($(e.currentTarget).attr("data-item-id"));
            let orden = eval($(e.currentTarget).attr("data-item-orden"));
            let nuevoOrden = orden + 1;
            entidadDeportivaPage.methods.cambiarOrden({ id, orden: nuevoOrden });
        }
    },
    methods: {
        buscar: function () {
            $(entidadDeportivaPage.options.idTblResultados).DataTable().ajax.reload();
        },
        confirmarCambiarFlagActivo: function (id, flagActivo) {
            let title = "Confirmar";
            let text = `¿Desea ${(flagActivo ? "inactivar" : "activar")} el registro?`;
            entidadDeportivaPage.options.cambiarFlagActivoSwal.title = title;
            entidadDeportivaPage.options.cambiarFlagActivoSwal.text = text;
            Swal.fire(entidadDeportivaPage.options.cambiarFlagActivoSwal)
                .then(entidadDeportivaPage.methods.cambiarFlagActivo.bind({ id, flagActivo }));
        },
        cambiarFlagActivo: function (result) {
            if (!result.isConfirmed) return;
            let url = `${urlBaseWebApi}/api/entidaddeportiva/cambiar-flagactivo-entidaddeportiva/${this.id}/${this.flagActivo}`;
            let init = {};
            init.method = 'PUT';
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaPage.methods.finalizarCambiarFlagActivo);
        },
        finalizarCambiarFlagActivo: function (data) {
            let title = data.status == "success" ? data.result ? "Se modificó el registro correctamente" : data.message : "Ocurrió un error";
            let icon = data.status == "success" ? data.result ? "success" : "error" : data.status;
            entidadDeportivaPage.options.finalizarCambiarFlagActivoSwal.title = title;
            entidadDeportivaPage.options.finalizarCambiarFlagActivoSwal.icon = icon;
            $(document).Toasts('create', entidadDeportivaPage.options.finalizarCambiarFlagActivoSwal);
            $(entidadDeportivaPage.options.idBtnBuscar).trigger('click');
        },
        cambiarOrden: function (data) {
            let url = `${urlBaseWebApi}/api/entidaddeportiva/cambiar-orden-entidaddeportiva/${data.id}/${data.orden}`;
            let init = {};
            init.method = 'PUT';
            init.credentials = 'include';
            fetch(url, init)
                .then(funciones.managedResponseFetchToJson)
                .then(entidadDeportivaPage.methods.finalizarCambiarOrden);
        },
        finalizarCambiarOrden: function (data) {
            $(entidadDeportivaPage.options.idBtnBuscar).trigger('click');
        }
    }
}