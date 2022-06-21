const funciones = {
    managedResponseFetchToJson: function (response) {
        if (!(response.ok && response.status == 200)) return null;
        return response.json();
    },
    managedResponseAllPromisesToJson: function (responses) {
        return Promise.all(responses.map(function (r) {
            if (!(r.ok && r.status == 200)) return null;
            return r.json();
        }));
    },
    loading: function (selector, options) {
        let optionsDefault = {
            selectorMain: '.overlay-wrapper',
            selectorOverlay: '.overlay',
            html: '<div class="overlay-wrapper"><div class="overlay"><i class="fas fa-3x fa-sync-alt fa-spin"></i><div class="text-bold pt-2">Cargando...</div></div></div>'
        };

        options = { ...optionsDefault, ...options };

        let _open = function () {
            let elements = Array.from($(selector));
            for (let element of elements) {
                let parentElement = $(element).parent();
                let contentElement = $(element);
                let overlayElement = $(options.html).append(contentElement);
                parentElement.addClass('modal-open');
                parentElement.append(overlayElement);
            }
        };

        let _close = function () {
            let elements = Array.from($(selector)).filter(e => $(e).closest(options.selectorMain).length > 0);
            for (let element of elements) {
                let parentElement = $(element).closest(options.selectorMain);
                parentElement.find(options.selectorOverlay).remove();
                let contentElement = $(element);
                parentElement.parent().append(contentElement);
                parentElement.parent().removeClass('modal-open');
                parentElement.remove();
            }
        };

        return {
            Open: _open,
            Close: _close
        };
    },
    limpiarPreloader: function () {
        $('.preloader').css('height', 0);
        setTimeout(function () {
            $('.preloader').children().hide();
        }, 200);
    },
    treeCheckBoxChecked: function (el) {
        let checked = el.checked;
        $(el).parent().find('input[type="checkbox"]').prop('checked', checked);

        let cantidadHermanos = $(el).parent().parent().find('> div[class^="icheck-"] > input[type="checkbox"]').length;
        let cantidadHermanosCheckeados = $(el).parent().parent().find('> div[class^="icheck-"] > input[type="checkbox"]:checked').length;

        let checkedParent = cantidadHermanosCheckeados > 0;
        $(el).parent().parent().find('> input[type="checkbox"]').prop('checked', checkedParent);
    },
    cerrarSesion: function () {
        document.cookie = `currentSesion=;expires=${(new Date()).toGMTString()};domain=${domainCookie};`;
        location.reload();
    }
}