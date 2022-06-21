const entidadDeportivaMantenimientoPage = {
    init: function () {
        entidadDeportivaMantenimientoPage.initEvents();
        entidadDeportivaMantenimientoPage.methods.obtenerEntidadDeportiva();
    },
    initEvents: function () {
        let { idFrmRegistro, idBtnCancelar, validate } = entidadDeportivaMantenimientoPage.options;
        let { btnCancelarClick } = entidadDeportivaMantenimientoPage.events;
        $(idFrmRegistro).validate(validate);
        $(idBtnCancelar).click(btnCancelarClick);
    },
    default: {
        entidadDeportiva: null
    },
    options: {
        idFrmRegistro: '#frm-registro',
        idBtnCancelar: '#btn-cancelar',
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
        dataTableUsuario: {
            processing: true,
            serverSide: true,
            ajax: {
                url: `${urlBaseWebApi}/api/usuario/buscar-usuario`,
                data: function (params, settings) {
                    let [columnIndex, sortOrder] = $(settings.nTable).DataTable().order()[0];
                    let sortName = $(settings.nTable).DataTable().column(columnIndex).dataSrc();

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
        }
    },
    events: {
        btnCancelarClick: function (e) {
            entidadDeportivaMantenimientoPage.methods.validarCancelar();
        }
    },
    methods: {
        obtenerEntidadDeportiva: function () {
            if (entidadDeportivaId == null) {
                funciones.limpiarPreloader();
                return;
            }
            let url = `${urlBaseWebApi}/api/entidaddeportiva/obtener-entidaddeportiva/${entidadDeportivaId}`;
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
            funciones.limpiarPreloader();
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
    }
}