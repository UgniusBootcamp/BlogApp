const fileInput = document.getElementById('file-upload');
const preview = document.getElementById('imagePreview');
const removeBtn = document.getElementById('removeImage');

fileInput.addEventListener('change', function (event) {
    const file = event.target.files[0];

    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.classList.remove('hidden');
            removeBtn.classList.remove('hidden');
        };
        reader.readAsDataURL(file);
    } else {
        resetPreview();
    }
});

removeBtn.addEventListener('click', function () {
    resetPreview();
    fileInput.value = '';
});

function resetPreview() {
    preview.src = '#';
    preview.classList.add('hidden');
    removeBtn.classList.add('hidden');
}