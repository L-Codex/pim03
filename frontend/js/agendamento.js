// --- Máscara de telefone ---
document.addEventListener('DOMContentLoaded', () => {
  const tel = document.getElementById('telefone');
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
});

const estado = {
  servicoNome: null,
  servicoPreco: null,
  horario: null,
};

// --- Selecionar serviço ---
function selecionarServico(el, nome, preco) {
  document.querySelectorAll('.servico-card').forEach(c => c.classList.remove('selected'));
  el.classList.add('selected');
  estado.servicoNome = nome;
  estado.servicoPreco = preco;
}

// --- Selecionar horário ---
function selecionarHorario(el, horario) {
  document.querySelectorAll('.horario-btn').forEach(b => b.classList.remove('selected'));
  el.classList.add('selected');
  estado.horario = horario;
}

// --- Navegar entre steps ---
function irParaStep(step) {
  // Validações
  if (step === 2 && !estado.servicoNome) {
    alert('Selecione um serviço para continuar.');
    return;
  }

  if (step === 3 && !estado.horario) {
    alert('Selecione um horário para continuar.');
    return;
  }

  if (step === 4) {
    const nome = document.getElementById('nome').value.trim();
    const telefone = document.getElementById('telefone').value.trim();
    let valid = true;

    document.getElementById('nome-err').style.display = 'none';
    document.getElementById('tel-err').style.display = 'none';
    document.getElementById('nome').classList.remove('input-error');
    document.getElementById('telefone').classList.remove('input-error');

    if (!nome) {
      document.getElementById('nome').classList.add('input-error');
      document.getElementById('nome-err').style.display = 'block';
      valid = false;
    }

    if (!telefone) {
      document.getElementById('telefone').classList.add('input-error');
      document.getElementById('tel-err').style.display = 'block';
      valid = false;
    }

    if (!valid) return;

    // Preenche o resumo
    document.getElementById('res-servico').textContent = estado.servicoNome;
    document.getElementById('res-horario').textContent = estado.horario;
    document.getElementById('res-nome').textContent = nome;
    document.getElementById('res-telefone').textContent = telefone;
    document.getElementById('res-total').textContent = `R$ ${estado.servicoPreco.toFixed(2).replace('.', ',')}`;
  }

  // Esconde todos os painéis
  document.querySelectorAll('.step-panel').forEach(p => p.classList.remove('active'));
  document.getElementById(`panel-${step}`).classList.add('active');

  // Atualiza indicadores de step
  for (let i = 1; i <= 4; i++) {
    const stepEl = document.getElementById(`step-ind-${i}`);
    stepEl.classList.remove('active', 'done');

    if (i < step) stepEl.classList.add('done');
    if (i === step) stepEl.classList.add('active');
  }

  // Atualiza linhas
  for (let i = 1; i <= 3; i++) {
    const line = document.getElementById(`line-${i}`);
    if (line) {
      line.classList.toggle('done', i < step);
    }
  }

  window.scrollTo({ top: 0, behavior: 'smooth' });
}

// --- Confirmar agendamento ---
function confirmarAgendamento() {
  const nome = document.getElementById('nome').value.trim();

  // TODO: substituir pelo fetch para POST /agendamento quando o back-end estiver pronto

  // Esconde painéis e steps
  document.querySelectorAll('.step-panel').forEach(p => p.classList.remove('active'));
  document.querySelector('.steps').style.display = 'none';
  document.querySelector('.page-title').style.display = 'none';
  document.querySelector('.page-tag').style.display = 'none';

  // Exibe sucesso
  document.getElementById('suc-nome').textContent = nome;
  document.getElementById('sucesso').classList.add('active');

  window.scrollTo({ top: 0, behavior: 'smooth' });
}
