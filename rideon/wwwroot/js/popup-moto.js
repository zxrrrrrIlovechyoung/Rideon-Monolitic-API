(function () {
    const popup = document.getElementById('popupMoto');
    const btnAddMoto = document.getElementById('btnAddMoto');
    const buscador = document.getElementById('buscadorMoto');
    const filtroTipo = document.getElementById('filtroTipo');
    const resultados = document.getElementById('resultadosBusqueda');
    const idHidden = document.getElementById('idMotoSeleccionada');
    const modeloInput = document.getElementById('modeloMoto');
    const cilindrajeInput = document.getElementById('cilindrajeMoto');
    const imgPreview = document.getElementById('imgPreviewMoto');
    const formAdd = document.getElementById('formAddMoto');

    function attachOpen() {
        if (btnAddMoto) btnAddMoto.addEventListener('click', () => abrirPopup());
        document.querySelectorAll('#btnAddMoto').forEach(b => b.addEventListener('click', () => abrirPopup()));
    }

    window.abrirPopup = function () {
        if (!popup) return;
        popup.style.display = 'flex';
        popup.setAttribute('aria-hidden', 'false');
        if (buscador) buscador.focus();
        cargarResultados();
    };

    window.cerrarPopup = function () {
        if (!popup) return;
        popup.style.display = 'none';
        popup.setAttribute('aria-hidden', 'true');
        resultados.innerHTML = '';
        idHidden.value = '';
        modeloInput.value = '';
        cilindrajeInput.value = '';
        imgPreview.src = '/images/default-moto.png';
        buscador.value = '';
        filtroTipo.value = '';
    };

    window.cargarResultados = function () {
        if (!resultados) return;
        const q = encodeURIComponent(buscador?.value ?? '');
        const tipo = filtroTipo?.value ?? '';
        const url = `/Usuario/BuscarMotos?q=${q}&tipo=${tipo}`;

        fetch(url)
            .then(res => res.ok ? res.json() : Promise.reject('Network error'))
            .then(data => {
                resultados.innerHTML = '';
                if (!data || data.length === 0) {
                    resultados.innerHTML = '<div class="p-2">No se encontraron motos</div>';
                    return;
                }
                data.forEach(moto => {
                    const item = document.createElement('div');
                    item.className = 'moto-item';
                    const marcaModelo = document.createElement('div');
                    marcaModelo.innerHTML = `<strong>${escapeHtml(moto.marca)} ${escapeHtml(moto.strModelo)}</strong><br><small>${escapeHtml(moto.strCilindraje)} cc · ${escapeHtml(moto.tipoMoto)}</small>`;
                    const img = document.createElement('img');
                    img.className = 'mini';
                    img.src = moto.imgFoto || '/images/default-moto.png';
                    img.alt = moto.strModelo;
                    const tooltip = document.createElement('div');
                    tooltip.className = 'tooltip-preview';
                    tooltip.innerHTML = `<img src="${moto.imgFoto || '/images/default-moto.png'}" style="width:100%;height:120px;object-fit:cover;border-radius:6px;" /><p style="margin:6px 0 0 0;"><strong>${escapeHtml(moto.marca)} ${escapeHtml(moto.strModelo)}</strong></p><p style="margin:0;">Cilindraje: ${escapeHtml(moto.strCilindraje)}</p>`;
                    item.appendChild(img);
                    item.appendChild(marcaModelo);
                    item.appendChild(tooltip);

                    item.addEventListener('click', () => {
                        document.querySelectorAll('.moto-item.seleccionada').forEach(e => e.classList.remove('seleccionada'));
                        item.classList.add('seleccionada');
                        idHidden.value = moto.idMoto;
                        modeloInput.value = `${moto.marca} ${moto.strModelo}`;
                        cilindrajeInput.value = moto.strCilindraje;
                        imgPreview.src = moto.imgFoto || '/images/default-moto.png';
                    });

                    resultados.appendChild(item);
                });
            })
            .catch(err => {
                console.error(err);
                resultados.innerHTML = '<div class="p-2 text-danger">Error cargando resultados</div>';
            });
    };

    function escapeHtml(text) {
        if (!text && text !== 0) return '';
        return String(text).replace(/&/g, "&amp;").replace(/</g, "&lt;")
            .replace(/>/g, "&gt;").replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    if (formAdd) {
        formAdd.addEventListener('submit', e => {
            if (!idHidden.value) {
                e.preventDefault();
                alert('Debes seleccionar una moto del catálogo antes de guardar.');
                return false;
            }
            return true;
        });
    }

    if (buscador) buscador.addEventListener('input', debounce(cargarResultados, 300));
    if (filtroTipo) filtroTipo.addEventListener('change', cargarResultados);

    function debounce(fn, wait) {
        let t;
        return function () {
            clearTimeout(t);
            t = setTimeout(() => fn.apply(this, arguments), wait);
        };
    }

    attachOpen();
    document.addEventListener('DOMContentLoaded', () => {
        const btnMotoOptions = document.getElementById('btnMotoOptions');
        const motoOptionsMenu = document.getElementById('motoOptionsMenu');
        const btnEliminarMoto = document.getElementById('btnEliminarMoto');
        const popupEliminar = document.getElementById('popupEliminarMoto');
        const closeDeleteBtn = document.getElementById('closeDeleteMoto');
        const cancelDeleteBtn = document.getElementById('cancelDeleteMoto');
        const confirmDeleteBtn = document.getElementById('confirmDeleteMoto');
        if (btnMotoOptions && motoOptionsMenu) {
            btnMotoOptions.addEventListener('click', e => {
                e.stopPropagation(); 
                motoOptionsMenu.style.display = motoOptionsMenu.style.display === 'block' ? 'none' : 'block';
            });
            document.addEventListener('click', () => {
                if (motoOptionsMenu) motoOptionsMenu.style.display = 'none';
            });
        }

        // Abrir popup eliminar
        if (btnEliminarMoto && popupEliminar) {
            btnEliminarMoto.addEventListener('click', e => {
                e.stopPropagation();
                popupEliminar.style.display = 'flex';
                motoOptionsMenu.style.display = 'none';
            });
        }

        // Cerrar popup
        [closeDeleteBtn, cancelDeleteBtn].forEach(btn => {
            if (btn) btn.addEventListener('click', () => popupEliminar.style.display = 'none');
        });

        // Click fuera del contenido
        if (popupEliminar) {
            popupEliminar.addEventListener('click', e => {
                if (e.target === popupEliminar) popupEliminar.style.display = 'none';
            });
        }

        // Confirmar eliminar
        if (confirmDeleteBtn) {
            confirmDeleteBtn.addEventListener('click', () => {
                popupEliminar.style.display = 'none';
            });
        }
    });

})();
