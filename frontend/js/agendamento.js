// Estado central do agendamento
const estado = {
  servicos: [],
  data: null,
  horario: null,
};

// Seletores utilitários
const servicoCards = () => Array.from(document.querySelectorAll('[data-servico]'));
const horarioBtns = () => Array.from(document.querySelectorAll('.horario-btn:not(.indisponivel)'));

// Formata moeda BRL simples
function formatarMoeda(valor) {
  return `R$ ${valor.toFixed(2).replace('.', ',')}`;
}

// Formata data para BR (DD/MM/AAAA)
function formatarDataBR(valor) {
  if (!valor) return '—';
  const [ano, mes, dia] = valor.split('-');
  return `${dia}/${mes}/${ano}`;
}

// Valida data (existente + não passada + limite máximo)
function dataValida(valor) {
  if (!valor) return false;

  const [ano, mes, dia] = valor.split('-').map(Number);

  // Limite máximo
  if (ano > 2099) return false;

  const data = new Date(ano, mes - 1, dia);
  const hoje = new Date();
  hoje.setHours(0, 0, 0, 0);

  return (
    data.getFullYear() === ano &&
    data.getMonth() === mes - 1 &&
    data.getDate() === dia &&
    data >= hoje
  );
}

// Atualiza total de serviços
function atualizarTotal() {
  const total = estado.servicos.reduce((acc, servico) => acc + servico.preco, 0);
  const totalEl = document.getElementById('total-servicos');
  if (totalEl) totalEl.textContent = formatarMoeda(total);

  const totalResumo = document.getElementById('res-total');
  if (totalResumo) totalResumo.textContent = formatarMoeda(total);
}

// Preenche resumo final
function atualizarResumo() {
  const nome = document.getElementById('nome').value.trim();
  const telefone = document.getElementById('telefone').value.trim();
  const email = document.getElementById('email').value.trim();
  const data = document.getElementById('data').value;

  document.getElementById('res-servicos').textContent =
    estado.servicos.length ? estado.servicos.map(s => s.nome).join(', ') : '—';
  document.getElementById('res-data').textContent = formatarDataBR(data);
  document.getElementById('res-horario').textContent = estado.horario || '—';
  document.getElementById('res-nome').textContent = nome || '—';
  document.getElementById('res-telefone').textContent = telefone || '—';
  document.getElementById('res-email').textContent = email || '—';

  atualizarTotal();
}

// Helpers de erro
function limparErro(inputId, errorId) {
  const input = document.getElementById(inputId);
  const error = document.getElementById(errorId);
  if (!input || !error) return;
  input.classList.remove('input-error');
  error.style.display = 'none';
}

function mostrarErro(inputId, errorId) {
  const input = document.getElementById(inputId);
  const error = document.getElementById(errorId);
  if (!input || !error) return;
  input.classList.add('input-error');
  error.style.display = 'block';
}

// === Controle de TAB (apenas etapa ativa) ===
function focaveisNoPainel(panel) {
  return Array.from(
    panel.querySelectorAll('a, button, input, select, textarea, [tabindex]')
  ).filter(el => !el.classList.contains('indisponivel'));
}

function desabilitarTab(panel) {
  focaveisNoPainel(panel).forEach(el => {
    if (!el.hasAttribute('data-tabindex')) {
      el.setAttribute('data-tabindex', el.getAttribute('tabindex') ?? '');
    }
    el.setAttribute('tabindex', '-1');
  });
}

function habilitarTab(panel) {
  focaveisNoPainel(panel).forEach(el => {
    const anterior = el.getAttribute('data-tabindex');
    if (anterior === '') {
      el.removeAttribute('tabindex');
    } else if (anterior != null) {
      el.setAttribute('tabindex', anterior);
    } else {
      el.removeAttribute('tabindex');
    }
    el.removeAttribute('data-tabindex');
  });
}

function focarPrimeiro(panel) {
  const primeiro = panel.querySelector('input, select, textarea, button, a');
  if (primeiro) primeiro.focus();
}

// Inicialização
document.addEventListener('DOMContentLoaded', () => {
  const tel = document.getElementById('telefone');
  const data = document.getElementById('data');
  const hoje = new Date().toISOString().split('T')[0];

  // Data mínima: hoje
  if (data) data.min = hoje;

  // Máscara de telefone
  if (tel) {
    tel.addEventListener('input', () => {
      let v = tel.value.replace(/\D/g, '').slice(0, 11);
      if (v.length <= 10) {
        v = v.replace(/^(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
      } else {
        v = v.replace(/^(\d{2})(\d{5})(\d{0,4})/, '($1) $2-$3');
      }
      tel.value = v;
    });
  }

  // Remove erro ao digitar
  ['nome', 'telefone', 'email', 'obs', 'data'].forEach(id => {
    const el = document.getElementById(id);
    if (!el) return;
    el.addEventListener('keydown', () => {
      const errId = `${id === 'telefone' ? 'tel' : id}-err`;
      limparErro(id, errId);
    });
  });

  // Clique em serviços
  servicoCards().forEach(card => {
    card.addEventListener('click', () => toggleServico(card));
  });

  // Clique em horário
  horarioBtns().forEach(btn => {
    btn.addEventListener('click', () => selecionarHorario(btn));
  });

  // Navegação de steps
  document.querySelectorAll('[data-step]').forEach(btn => {
    btn.addEventListener('click', () => {
      const step = Number(btn.dataset.step);
      irParaStep(step);
    });
  });

  // Confirmação final
  const confirmar = document.getElementById('btn-confirmar');
  if (confirmar) {
    confirmar.addEventListener('click', confirmarAgendamento);
  }

  // Inicial: só etapa 1 com TAB
  document.querySelectorAll('.step-panel').forEach(panel => {
    panel.id === 'panel-1' ? habilitarTab(panel) : desabilitarTab(panel);
  });
  focarPrimeiro(document.getElementById('panel-1'));
});

// Seleciona/retira serviço (multi-select)
function toggleServico(card) {
  const nome = card.dataset.nome;
  const preco = Number(card.dataset.preco);

  const jaSelecionado = estado.servicos.some(s => s.nome === nome);

  if (jaSelecionado) {
    estado.servicos = estado.servicos.filter(s => s.nome !== nome);
    card.classList.remove('selected');
  } else {
    estado.servicos.push({ nome, preco });
    card.classList.add('selected');
  }

  atualizarTotal();
  document.getElementById('servico-err').style.display = 'none';
}

// Seleciona horário único
function selecionarHorario(btn) {
  horarioBtns().forEach(b => b.classList.remove('selected'));
  btn.classList.add('selected');
  estado.horario = btn.dataset.horario;
  document.getElementById('horario-err').style.display = 'none';
}

// Validação etapa 1
function validarStep1() {
  const nome = document.getElementById('nome').value.trim();
  const telefone = document.getElementById('telefone').value.trim();
  const email = document.getElementById('email').value.trim();
  const emailInput = document.getElementById('email');

  let valido = true;

  limparErro('nome', 'nome-err');
  limparErro('telefone', 'tel-err');
  limparErro('email', 'email-err');

  if (!nome) {
    mostrarErro('nome', 'nome-err');
    valido = false;
  }

  if (!telefone || telefone.length < 14) {
    mostrarErro('telefone', 'tel-err');
    valido = false;
  }

  if (email && !emailInput.checkValidity()) {
    mostrarErro('email', 'email-err');
    valido = false;
  }

  return valido;
}

// Validação etapa 2
function validarStep2() {
  const dataValor = document.getElementById('data').value;
  let valido = true;

  if (!dataValida(dataValor)) {
    mostrarErro('data', 'data-err');
    valido = false;
  } else {
    limparErro('data', 'data-err');
  }

  if (!estado.horario) {
    document.getElementById('horario-err').style.display = 'block';
    valido = false;
  } else {
    document.getElementById('horario-err').style.display = 'none';
  }

  return valido;
}

// Validação etapa 3
function validarStep3() {
  if (!estado.servicos.length) {
    document.getElementById('servico-err').style.display = 'block';
    return false;
  }
  document.getElementById('servico-err').style.display = 'none';
  return true;
}

// Navegação entre steps
function irParaStep(step) {
  if (step === 2 && !validarStep1()) return;
  if (step === 3 && !validarStep2()) return;
  if (step === 4 && !validarStep3()) return;

  if (step === 4) atualizarResumo();

  document.querySelectorAll('.step-panel').forEach(p => p.classList.remove('active'));
  const painelAtivo = document.getElementById(`panel-${step}`);
  painelAtivo.classList.add('active');

  // Atualiza TAB: só o painel ativo
  document.querySelectorAll('.step-panel').forEach(panel => {
    panel.id === `panel-${step}` ? habilitarTab(panel) : desabilitarTab(panel);
  });

  // Indicadores
  for (let i = 1; i <= 4; i++) {
    const stepEl = document.getElementById(`step-ind-${i}`);
    stepEl.classList.remove('active', 'done');
    if (i < step) stepEl.classList.add('done');
    if (i === step) stepEl.classList.add('active');
  }

  for (let i = 1; i <= 3; i++) {
    const line = document.getElementById(`line-${i}`);
    if (line) line.classList.toggle('done', i < step);
  }

  focarPrimeiro(painelAtivo);
  window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Confirmação final (mock)
function confirmarAgendamento() {
  const nome = document.getElementById('nome').value.trim();

  // TODO: substituir pelo fetch para POST /agendamento quando o back-end estiver pronto

  document.querySelectorAll('.step-panel').forEach(p => p.classList.remove('active'));
  document.querySelector('.steps').style.display = 'none';
  document.querySelector('.page-title').style.display = 'none';
  document.querySelector('.page-tag').style.display = 'none';

  document.getElementById('suc-nome').textContent = nome;
  document.getElementById('sucesso').classList.add('active');

  window.scrollTo({ top: 0, behavior: 'smooth' });
}
