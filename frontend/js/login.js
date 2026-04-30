// Alterna o perfil selecionado visualmente
function setRole(el) {
  document.querySelectorAll('.role-btn').forEach(b => b.classList.remove('active'));
  el.classList.add('active');
}

// Validação e simulação de login
function handleLogin() {
  const login = document.getElementById('login');
  const senha = document.getElementById('senha');
  const loginErr = document.getElementById('login-err');
  const senhaErr = document.getElementById('senha-err');
  const alertEl = document.getElementById('alert');
  const btn = document.getElementById('btn');

  let valid = true;
  alertEl.style.display = 'none';
  login.classList.remove('input-error');
  senha.classList.remove('input-error');
  loginErr.style.display = 'none';
  senhaErr.style.display = 'none';

  if (!login.value.trim()) {
    login.classList.add('input-error');
    loginErr.style.display = 'block';
    valid = false;
  }

  if (!senha.value.trim()) {
    senha.classList.add('input-error');
    senhaErr.style.display = 'block';
    valid = false;
  }

  if (!valid) return;

  btn.classList.add('loading');
  btn.textContent = 'Verificando...';

  // TODO: substituir pelo fetch para POST /login quando o back-end estiver pronto
  setTimeout(() => {
    btn.classList.remove('loading');
    btn.textContent = 'Entrar';
    alertEl.style.display = 'block';
    login.classList.add('input-error');
    senha.classList.add('input-error');
  }, 1200);
}

// Inicialização
document.addEventListener('DOMContentLoaded', () => {
  const form = document.getElementById('login-form');
  const roleButtons = document.querySelectorAll('.role-btn');
  const loginInput = document.getElementById('login');
  const leftPanel = document.querySelector('.left-panel');

  // Clique nos perfis
  roleButtons.forEach(btn => {
    btn.addEventListener('click', () => setRole(btn));
  });

  // Submit do formulário
  form.addEventListener('submit', event => {
    event.preventDefault();
    handleLogin();
  });

  // Limpa erros ao digitar
  ['login', 'senha'].forEach(id => {
    const input = document.getElementById(id);
    const err = document.getElementById(`${id}-err`);
    if (!input || !err) return;
    input.addEventListener('keydown', () => {
      input.classList.remove('input-error');
      err.style.display = 'none';
    });
  });

  // Foco inicial
  if (loginInput) loginInput.focus();

  // Garante TAB apenas em elementos visíveis (painel esquerdo some no mobile)
  if (leftPanel) {
    const focusables = Array.from(
      leftPanel.querySelectorAll('a, button, input, select, textarea, [tabindex]')
    );

    const atualizarTabPainel = () => {
      const visivel = window.getComputedStyle(leftPanel).display !== 'none';
      focusables.forEach(el => {
        el.setAttribute('tabindex', visivel ? '0' : '-1');
      });
    };

    window.addEventListener('resize', atualizarTabPainel);
    atualizarTabPainel();
  }
});
