// Estado visual do login. Mantém as referências concentradas para evitar buscas repetidas no DOM.
const dom = {
  form: null,
  login: null,
  senha: null,
  loginErr: null,
  senhaErr: null,
  alert: null,
  btn: null,
  loginInput: null,
  leftPanel: null,
  roleButtons: [],
};

// Dados que alimentam o painel lateral e os perfis.
const FEATURES = [
  'Agendamentos online',
  'Controle de atendimentos',
  'Gestão de funcionários',
  'Relatórios financeiros',
];

function renderFeatures() {
  const container = document.getElementById('features-list');
  if (!container) return;
  container.innerHTML = FEATURES.map(f => `
    <div class="feature d-flex align-items-center">
      <div class="feature-dot"></div>
      ${f}
    </div>
  `).join('');
}


// Limpa o estado visual de erro de um campo.
function limparErro(input, error) {
  if (!input || !error) return;
  input.classList.remove('input-error');
  error.style.display = 'none';
}

// Mostra o estado visual de erro de um campo.
function mostrarErro(input, error) {
  if (!input || !error) return;
  input.classList.add('input-error');
  error.style.display = 'block';
}

// Valida os campos e simula o login.
// O fluxo é propositalmente simples: validação, feedback visual e mock de resposta.
function handleLogin() {
  let valid = true;

  dom.alert.style.display = 'none';
  limparErro(dom.login, dom.loginErr);
  limparErro(dom.senha, dom.senhaErr);

  if (!dom.login.value.trim()) {
    mostrarErro(dom.login, dom.loginErr);
    valid = false;
  }

  if (!dom.senha.value.trim()) {
    mostrarErro(dom.senha, dom.senhaErr);
    valid = false;
  }

  if (!valid) return;

  // Feedback temporário enquanto o back-end não existe.
  dom.btn.classList.add('loading');
  dom.btn.textContent = 'Verificando...';

  // TODO: substituir pelo fetch para POST /login quando o back-end estiver pronto.
  setTimeout(() => {
    dom.btn.classList.remove('loading');
    dom.btn.textContent = 'Entrar';
    dom.alert.style.display = 'block';
    mostrarErro(dom.login, dom.loginErr);
    mostrarErro(dom.senha, dom.senhaErr);
  }, 1200);
}

// Atualiza o `tabindex` do painel lateral para não deixar foco em elementos invisíveis.
function atualizarTabPainel() {
  if (!dom.leftPanel) return;

  const focusables = Array.from(
    dom.leftPanel.querySelectorAll('a, button, input, select, textarea, [tabindex]')
  );
  const visivel = window.getComputedStyle(dom.leftPanel).display !== 'none';

  focusables.forEach(el => {
    el.setAttribute('tabindex', visivel ? '0' : '-1');
  });
}

// Inicialização da tela.
document.addEventListener('DOMContentLoaded', () => {
  // Renderização data-driven antes de vincular eventos.
  renderFeatures();

  dom.form = document.getElementById('login-form');
  dom.login = document.getElementById('login');
  dom.senha = document.getElementById('senha');
  dom.loginErr = document.getElementById('login-err');
  dom.senhaErr = document.getElementById('senha-err');
  dom.alert = document.getElementById('alert');
  dom.btn = document.getElementById('btn');
  dom.loginInput = document.getElementById('login');
  dom.leftPanel = document.querySelector('.left-panel');
  dom.roleButtons = [];

  // Submit do formulário.
  dom.form?.addEventListener('submit', event => {
    event.preventDefault();
    handleLogin();
  });

  // Limpa erros ao digitar, sem deixar a interface “presa” no estado de falha.
  [
    [dom.login, dom.loginErr],
    [dom.senha, dom.senhaErr],
  ].forEach(([input, err]) => {
    if (!input || !err) return;
    input.addEventListener('keydown', () => limparErro(input, err));
  });

  // Foco inicial para acelerar o login no teclado.
  dom.loginInput?.focus();

  // Mantém o painel lateral coerente com a responsividade.
  if (dom.leftPanel) {
    window.addEventListener('resize', atualizarTabPainel);
    atualizarTabPainel();
  }
});
