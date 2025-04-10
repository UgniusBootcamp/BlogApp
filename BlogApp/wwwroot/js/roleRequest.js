document.getElementById('btnRequestRole')?.addEventListener('click', function () {
    fetch('/RoleRequest/CreateRoleRequest')
        .then(response => response.text())
        .then(html => {
            document.getElementById('modalPlaceholder').innerHTML = html;
            document.getElementById('roleRequestModal').classList.remove('hidden');
        });
});

document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'closeModal') {
        document.getElementById('roleRequestModal')?.classList.add('hidden');
    }
});
    
