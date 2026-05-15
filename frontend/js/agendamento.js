// Estado central do agendamento
const estado = {
  servicos: [],
  data: null,
  horario: null,
};

// Janela máxima permitida para agendamento: hoje até 14 dias à frente.
const DIAS_MAXIMOS_AGENDAMENTO = 14;

// Cache dos elementos usados com mais frequência na tela.
const dom = {
  form: null,
  tel: null,
  data: null,
  nome: null,
  email: null,
  obs: null,
  confirmar: null,
  sucesso: null,
  steps: null,
  stepTitle: null,
  stepTag: null,
  servicoErr: null,
  horarioErr: null,
  dataErr: null,
  nomeErr: null,
  telErr: null,
  emailErr: null,
  totalServicos: null,
  resServicos: null,
  resData: null,
  resHorario: null,
  resNome: null,
  resTelefone: null,
  resEmail: null,
  resTotal: null,
  sucNome: null,
  panel1: null,
  panels: [],
  stepIndicators: [],
  stepLines: [],
};

// Seletores utilitários para reduzir repetição de querySelector.
const servicoCards = () => Array.from(document.querySelectorAll('[data-servico]'));
const horarioBtns = () => Array.from(document.querySelectorAll('.horario-btn:not(.indisponivel)'));

// Formata valores no padrão de moeda brasileira.
function formatarMoeda(valor) {
  return `R$ ${valor.toFixed(2).replace('.', ',')}`;
}

// Converte a data do input ISO para o formato BR.
function formatarDataBR(valor) {
  if (!valor) return '—';
  const [ano, mes, dia] = valor.split('-');
  return `${dia}/${mes}/${ano}`;
}

// Helpers pequenos para escrever texto e controlar visibilidade de erros.
function setTexto(el, valor) {
  if (el) el.textContent = valor;
}

function setVisibilidade(el, visivel) {
  if (el) el.style.display = visivel ? 'block' : 'none';
}

// Valida data dentro da janela permitida e garante que a data exista de fato.
function dataValida(valor) {
  if (!valor) return false;

  const [ano, mes, dia] = valor.split('-').map(Number);

  const data = new Date(ano, mes - 1, dia);
  const hoje = new Date();
  hoje.setHours(0, 0, 0, 0);

  const dataMaxima = new Date(hoje);
  dataMaxima.setDate(dataMaxima.getDate() + DIAS_MAXIMOS_AGENDAMENTO);

  return (
    data.getFullYear() === ano &&
    data.getMonth() === mes - 1 &&
    data.getDate() === dia &&
    data >= hoje &&
    data <= dataMaxima
  );
}

// Recalcula o total de serviços sempre que a seleção muda.
function atualizarTotal() {
  const total = estado.servicos.reduce((acc, servico) => acc + servico.preco, 0);
  setTexto(dom.totalServicos, formatarMoeda(total));
  setTexto(dom.resTotal, formatarMoeda(total));
}

// Monta o resumo final com os dados já preenchidos nas etapas anteriores.
function atualizarResumo() {
  const nome = dom.nome.value.trim();
  const telefone = dom.tel.value.trim();
  const email = dom.email.value.trim();
  const data = dom.data.value;

  setTexto(dom.resServicos, estado.servicos.length ? estado.servicos.map(s => s.nome).join(', ') : '—');
  setTexto(dom.resData, formatarDataBR(data));
  setTexto(dom.resHorario, estado.horario || '—');
  setTexto(dom.resNome, nome || '—');
  setTexto(dom.resTelefone, telefone || '—');
  setTexto(dom.resEmail, email || '—');

  atualizarTotal();
}

// Helpers para aplicar e remover estado de erro dos campos.
function limparErroDom(inputEl, errorEl) {
  if (!inputEl || !errorEl) return;
  inputEl.classList.remove('input-error');
  errorEl.style.display = 'none';
}

function mostrarErroDom(inputEl, errorEl) {
  if (!inputEl || !errorEl) return;
  inputEl.classList.add('input-error');
  errorEl.style.display = 'block';
}

// Garante que a navegação por TAB fique restrita ao painel ativo.
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

// Inicialização geral da tela.
document.addEventListener('DOMContentLoaded', () => {
  dom.form = document.getElementById('agendamento-form');
  dom.tel = document.getElementById('telefone');
  dom.data = document.getElementById('data');
  dom.nome = document.getElementById('nome');
  dom.email = document.getElementById('email');
  dom.obs = document.getElementById('obs');
  dom.confirmar = document.getElementById('btn-confirmar');
  dom.sucesso = document.getElementById('sucesso');
  dom.steps = document.querySelector('.steps');
  dom.stepTitle = document.querySelector('.page-title');
  dom.stepTag = document.querySelector('.page-tag');
  dom.servicoErr = document.getElementById('servico-err');
  dom.horarioErr = document.getElementById('horario-err');
  dom.dataErr = document.getElementById('data-err');
  dom.nomeErr = document.getElementById('nome-err');
  dom.telErr = document.getElementById('tel-err');
  dom.emailErr = document.getElementById('email-err');
  dom.totalServicos = document.getElementById('total-servicos');
  dom.resServicos = document.getElementById('res-servicos');
  dom.resData = document.getElementById('res-data');
  dom.resHorario = document.getElementById('res-horario');
  dom.resNome = document.getElementById('res-nome');
  dom.resTelefone = document.getElementById('res-telefone');
  dom.resEmail = document.getElementById('res-email');
  dom.resTotal = document.getElementById('res-total');
  dom.sucNome = document.getElementById('suc-nome');
  dom.panel1 = document.getElementById('panel-1');
  dom.panels = Array.from(document.querySelectorAll('.step-panel'));
  dom.stepIndicators = Array.from(document.querySelectorAll('.step'));
  dom.stepLines = [
    document.getElementById('line-1'),
    document.getElementById('line-2'),
    document.getElementById('line-3'),
  ];

  const hoje = new Date();
  hoje.setHours(0, 0, 0, 0);

  const dataMaxima = new Date(hoje);
  dataMaxima.setDate(dataMaxima.getDate() + DIAS_MAXIMOS_AGENDAMENTO);

  const hojeStr = hoje.toISOString().split('T')[0];
  const dataMaximaStr = dataMaxima.toISOString().split('T')[0];

  // Configura a janela permitida no próprio input para reforçar a validação.
  if (dom.data) {
    dom.data.min = hojeStr;
    dom.data.max = dataMaximaStr;
  }

  // Máscara simples de telefone para melhorar a digitação.
  if (dom.tel) {
    dom.tel.addEventListener('input', () => {
      let v = dom.tel.value.replace(/\D/g, '').slice(0, 11);
      if (v.length <= 10) {
        v = v.replace(/^(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
      } else {
        v = v.replace(/^(\d{2})(\d{5})(\d{0,4})/, '($1) $2-$3');
      }
      dom.tel.value = v;
    });
  }

  // Remove mensagens de erro assim que o usuário volta a editar o campo.
  [
    ['nome', dom.nome, dom.nomeErr],
    ['telefone', dom.tel, dom.telErr],
    ['email', dom.email, dom.emailErr],
    ['obs', dom.obs, null],
    ['data', dom.data, dom.dataErr],
  ].forEach(([id, inputEl, errorEl]) => {
    if (!inputEl) return;
    inputEl.addEventListener('keydown', () => {
      if (errorEl) limparErroDom(inputEl, errorEl);
      if (id === 'telefone') inputEl.classList.remove('input-error');
    });
  });

  // Cada card de serviço funciona como um toggle de seleção.
  servicoCards().forEach(card => {
    card.addEventListener('click', () => toggleServico(card));
  });

  // Horários disponíveis são seleção única.
  horarioBtns().forEach(btn => {
    btn.addEventListener('click', () => selecionarHorario(btn));
  });

  // Botões com data-step controlam a navegação entre etapas.
  document.querySelectorAll('[data-step]').forEach(btn => {
    btn.addEventListener('click', () => {
      const step = Number(btn.dataset.step);
      irParaStep(step);
    });
  });

  // Botão final de confirmação do agendamento.
  if (dom.confirmar) {
    dom.confirmar.addEventListener('click', confirmarAgendamento);
  }

  // Só a primeira etapa começa navegável por teclado.
  dom.panels.forEach(panel => {
    panel.id === 'panel-1' ? habilitarTab(panel) : desabilitarTab(panel);
  });
  focarPrimeiro(dom.panel1);
});

// Seleciona ou remove um serviço da lista atual.
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
  setVisibilidade(dom.servicoErr, false);
}

// Marca apenas um horário por vez.
function selecionarHorario(btn) {
  horarioBtns().forEach(b => b.classList.remove('selected'));
  btn.classList.add('selected');
  estado.horario = btn.dataset.horario;
  setVisibilidade(dom.horarioErr, false);
}

// Etapa 1: nome, telefone e e-mail.
function validarStep1() {
  const nome = dom.nome.value.trim();
  const telefone = dom.tel.value.trim();
  const email = dom.email.value.trim();
  const emailInput = dom.email;

  let valido = true;

  limparErroDom(dom.nome, dom.nomeErr);
  limparErroDom(dom.tel, dom.telErr);
  limparErroDom(dom.email, dom.emailErr);

  if (!nome) {
    mostrarErroDom(dom.nome, dom.nomeErr);
    valido = false;
  }

  if (!telefone || telefone.length < 14) {
    mostrarErroDom(dom.tel, dom.telErr);
    valido = false;
  }

  if (email && !emailInput.checkValidity()) {
    mostrarErroDom(dom.email, dom.emailErr);
    valido = false;
  }

  return valido;
}

// Etapa 2: data e horário.
function validarStep2() {
  const dataValor = dom.data.value;
  let valido = true;

  if (!dataValida(dataValor)) {
    mostrarErroDom(dom.data, dom.dataErr);
    valido = false;
  } else {
    limparErroDom(dom.data, dom.dataErr);
  }

  if (!estado.horario) {
    setVisibilidade(dom.horarioErr, true);
    valido = false;
  } else {
    setVisibilidade(dom.horarioErr, false);
  }

  return valido;
}

// Etapa 3: pelo menos um serviço precisa estar selecionado.
function validarStep3() {
  if (!estado.servicos.length) {
    setVisibilidade(dom.servicoErr, true);
    return false;
  }
  setVisibilidade(dom.servicoErr, false);
  return true;
}

// Navegação entre steps
function irParaStep(step) {
  if (step === 2 && !validarStep1()) return;
  if (step === 3 && !validarStep2()) return;
  if (step === 4 && !validarStep3()) return;

  if (step === 4) atualizarResumo();

  dom.panels.forEach(p => p.classList.remove('active'));
  const painelAtivo = document.getElementById(`panel-${step}`);
  painelAtivo.classList.add('active');

  // Atualiza TAB: só o painel ativo
  dom.panels.forEach(panel => {
    panel.id === `panel-${step}` ? habilitarTab(panel) : desabilitarTab(panel);
  });

  // Indicadores
  dom.stepIndicators.forEach((stepEl, index) => {
    const currentStep = index + 1;
    stepEl.classList.remove('active', 'done');
    if (currentStep < step) stepEl.classList.add('done');
    if (currentStep === step) stepEl.classList.add('active');
  });

  dom.stepLines.forEach((line, index) => {
    if (line) line.classList.toggle('done', index + 1 < step);
  });

  focarPrimeiro(painelAtivo);
  window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Confirmação final (mock)
function confirmarAgendamento() {
  const nome = dom.nome.value.trim();

  // TODO: substituir pelo fetch para POST /agendamento quando o back-end estiver pronto

  dom.panels.forEach(p => p.classList.remove('active'));
  setVisibilidade(dom.steps, false);
  setVisibilidade(dom.stepTitle, false);
  setVisibilidade(dom.stepTag, false);

  setTexto(dom.sucNome, nome);
  dom.sucesso.classList.add('active');

  window.scrollTo({ top: 0, behavior: 'smooth' });
}
