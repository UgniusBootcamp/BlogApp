const fileInput = document.getElementById('file-upload');
const preview = document.getElementById('imagePreview');
const removeBtn = document.getElementById('removeImage');
const HasChangedInput = document.getElementById('HasChanged');

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
        HasChangedInput.value = 'true';
    } else {
        resetPreview();
    }
});

removeBtn.addEventListener('click', function () {
    resetPreview();
    fileInput.value = '';
    HasChangedInput.value = 'true';
});

function resetPreview() {
    preview.src = '#';
    preview.classList.add('hidden');
    removeBtn.classList.add('hidden');
}