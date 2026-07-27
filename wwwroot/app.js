document.addEventListener("DOMContentLoaded", async function() {
    const map = L.map('map').setView([-16.0, -49.0], 7);

    L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', {
        minZoom: 0,
        maxZoom: 20,
        attribution: '&copy; OpenStreetMap contributors &copy; CARTO'
    }).addTo(map);

    const subtitle = document.getElementById('ranking-subtitle');
    const chart = document.getElementById('ranking-chart');
    const containerCheckboxes = document.getElementById('lista-checkboxes');
    const btnToggleAll = document.getElementById('btn-toggle-all');

    try {
        const response = await fetch('/api/producoes');
        if (!response.ok) throw new Error(`Erro na API: ${response.status}`);

        const listaCidades = await response.json();
        if (listaCidades.length === 0) {
            subtitle.innerHTML = 'Nenhum dado encontrado.';
            return;
        }

        const todasFrutasSet = new Set();
        listaCidades.forEach(cidade => {
            if (cidade.frutas) {
                Object.keys(cidade.frutas).forEach(f => todasFrutasSet.add(f));
            }
        });
        const listaFrutas = Array.from(todasFrutasSet).sort();
        let frutasSelecionadas = new Set(listaFrutas);

        // Criação dos checkboxes
        containerCheckboxes.innerHTML = '';
        listaFrutas.forEach(fruta => {
            const div = document.createElement('label');
            div.className = 'checkbox-item';
            div.innerHTML = `
                <input type="checkbox" value="${fruta}" checked style="cursor: pointer;">
                <span>${fruta}</span>
            `;
            const chk = div.querySelector('input');
            chk.addEventListener('change', () => {
                if (chk.checked) frutasSelecionadas.add(fruta);
                else frutasSelecionadas.delete(fruta);
                atualizarInterface();
            });
            containerCheckboxes.appendChild(div);
        });

        btnToggleAll.addEventListener('click', () => {
            const chks = containerCheckboxes.querySelectorAll('input');
            const deveMarcar = Array.from(chks).some(c => !c.checked);
            chks.forEach(c => {
                c.checked = deveMarcar;
                if (deveMarcar) frutasSelecionadas.add(c.value);
                else frutasSelecionadas.delete(c.value);
            });
            atualizarInterface();
        });

        const dadosPorCidadeMap = {};
        listaCidades.forEach(item => {
            dadosPorCidadeMap[item.cidade] = item;
        });

        const geojsonResponse = await fetch('https://raw.githubusercontent.com/tbrugz/geodata-br/master/geojson/geojs-52-mun.json');
        const geojsonGoias = await geojsonResponse.json();

        // Variável global para armazenar os totais filtrados atuais de cada cidade
        let totaisFiltradosPorCidade = {};
        let maiorValorGlobal = 1;

        const geoJsonLayer = L.geoJson(geojsonGoias, {
            style: function(feature) {
                return calcularEstiloMunicipio(feature.properties.name);
            },
            onEachFeature: function(feature, layer) {
                configurarPopupEEventos(feature, layer);
            }
        }).addTo(map);

        // Função para calcular a cor com base na escala de produção (Azul gradiente)
        function obterCorPorProducao(quantidade) {
            if (!quantidade || quantidade <= 0) return "#ffffff"; // Branco para sem produção
            
            // Gradiente de intensidade baseado na proporção em relação ao maior produtor atual
            const proporcao = quantidade / maiorValorGlobal;

            if (proporcao > 0.75) return "#08306b"; // Azul escuro profundo
            if (proporcao > 0.50) return "#2171b5"; // Azul intermediário forte
            if (proporcao > 0.25) return "#6baed6"; // Azul médio
            if (proporcao > 0.05) return "#bdd7e7"; // Azul claro
            return "#eff3ff";                       // Azul bem clarinho (produção mínima)
        }

        function calcularEstiloMunicipio(nomeMun) {
            const qtd = totaisFiltradosPorCidade[nomeMun] || 0;
            const cor = obterCorPorProducao(qtd);
            const opacidade = qtd > 0 ? 0.75 : 0.05;

            return {
                color: "black",
                weight: 0.3,
                fillColor: cor,
                fillOpacity: opacidade
            };
        }

        function configurarPopupEEventos(feature, layer) {
            const nomeMun = feature.properties.name;
            atualizarPopupLayer(layer, nomeMun);

            layer.on({
                mouseover: function(e) {
                    e.target.setStyle({ weight: 1.5, fillOpacity: 0.9 });
                },
                mouseout: function(e) {
                    geoJsonLayer.resetStyle(e.target);
                }
            });
        }

        function atualizarPopupLayer(layer, nomeMun) {
            const dadosMun = dadosPorCidadeMap[nomeMun];
            let conteudoPopup = `<div style="font-family: Arial; font-size: 13px; max-height: 200px; overflow-y: auto;">
                <b>${nomeMun}</b><br><hr style="margin:4px 0;">`;

            let totalFiltradoMun = 0;
            let temProducao = false;

            if (dadosMun && dadosMun.frutas) {
                for (const [fruta, qtd] of Object.entries(dadosMun.frutas)) {
                    if (frutasSelecionadas.has(fruta) && qtd > 0) {
                        conteudoPopup += `${fruta}: ${Math.round(qtd).toLocaleString('pt-BR')} t<br>`;
                        totalFiltradoMun += qtd;
                        temProducao = true;
                    }
                }
            }

            if (temProducao) {
                conteudoPopup += `<hr style="margin:4px 0;"><b>Total (Filtrado):</b> ${Math.round(totalFiltradoMun).toLocaleString('pt-BR')} t`;
            } else {
                conteudoPopup += `Sem registro para as frutas selecionadas`;
            }
            conteudoPopup += `</div>`;
            layer.bindPopup(conteudoPopup);
        }

        function atualizarInterface() {
            totaisFiltradosPorCidade = {};

            listaCidades.forEach(item => {
                let somaCidade = 0;
                if (item.frutas) {
                    for (const [fruta, qtd] of Object.entries(item.frutas)) {
                        if (frutasSelecionadas.has(fruta)) {
                            somaCidade += qtd;
                        }
                    }
                }
                if (somaCidade > 0) {
                    totaisFiltradosPorCidade[item.cidade] = somaCidade;
                }
            });

            // Descobre o maior valor atual para calibrar o gradiente de cores dinamicamente
            const valores = Object.values(totaisFiltradosPorCidade);
            maiorValorGlobal = valores.length > 0 ? Math.max(...valores) : 1;

            // Atualiza o estilo de todos os polígonos no mapa com base no novo gradiente
            geoJsonLayer.eachLayer(layer => {
                const nomeMun = layer.feature.properties.name;
                layer.setStyle(calcularEstiloMunicipio(nomeMun));
                atualizarPopupLayer(layer, nomeMun);
            });

            // Atualiza o Ranking Top 10
            const top10 = Object.entries(totaisFiltradosPorCidade)
                .map(([cidade, quantidade]) => ({ cidade, quantidade }))
                .sort((a, b) => b.quantidade - a.quantidade)
                .slice(0, 10);

            subtitle.innerHTML = `Frutas ativas: ${frutasSelecionadas.size} de ${listaFrutas.length}`;

            if (top10.length === 0) {
                chart.innerHTML = '<div class="ranking-empty">Nenhuma cidade encontrada para os filtros selecionados.</div>';
                return;
            }

            const maiorValorRanking = top10[0].quantidade;

            chart.innerHTML = top10.map((item, index) => {
                const percentual = (item.quantidade / maiorValorRanking) * 100;
                return `
                    <div class="ranking-row">
                        <div class="ranking-label" title="${item.cidade}">
                            ${index + 1}. ${item.cidade}
                        </div>
                        <div class="ranking-bar-wrap">
                            <div class="ranking-bar" style="width: ${percentual}%"></div>
                        </div>
                        <div class="ranking-value">
                            ${Math.round(item.quantidade).toLocaleString('pt-BR')} t
                        </div>
                    </div>
                `;
            }).join('');
        }

        atualizarInterface();

    } catch (error) {
        console.error("Erro:", error);
        subtitle.innerHTML = 'Erro ao carregar dados.';
        chart.innerHTML = `<div class="text-danger small">${error.message}</div>`;
    }
});