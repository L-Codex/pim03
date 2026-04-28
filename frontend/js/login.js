function setRole(el) {
    document.querySelectorAll('.role-btn').forEach(b => b.classList.remove('active'));
    el.classList.add('active');
}

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

document.addEventListener('keydown', e => {
    if (e.key === 'Enter') handleLogin();
});
