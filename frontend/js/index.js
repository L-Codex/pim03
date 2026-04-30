// Garante que links do menu só entrem no TAB quando visíveis (mobile)
document.addEventListener('DOMContentLoaded', () => {
  const navCollapse = document.getElementById('navPrincipal');
  if (!navCollapse) return;

  const navLinks = Array.from(navCollapse.querySelectorAll('a'));
  const media = window.matchMedia('(min-width: 768px)');

  function setNavTab(enabled) {
    navLinks.forEach(link => {
      link.setAttribute('tabindex', enabled ? '0' : '-1');
    });
  }

  function atualizarTabMenu() {
    if (media.matches) {
      // Desktop: sempre navegável
      setNavTab(true);
    } else {
      // Mobile: só quando o menu estiver aberto
      setNavTab(navCollapse.classList.contains('show'));
    }
  }

  navCollapse.addEventListener('shown.bs.collapse', () => setNavTab(true));
  navCollapse.addEventListener('hidden.bs.collapse', () => setNavTab(false));
  media.addEventListener('change', atualizarTabMenu);

  atualizarTabMenu();
});
