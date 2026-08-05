// wwwroot/js/grid.js
window.focusFirstInput = () => {
    const firstInput = document.querySelector('input[bind-value]');
    firstInput?.focus();
    firstInput?.select();
};
