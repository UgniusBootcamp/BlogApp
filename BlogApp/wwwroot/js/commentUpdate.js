document.addEventListener("DOMContentLoaded", function () {
    document.body.addEventListener("submit", async function (e) {
        const form = e.target;
        if (!form.classList.contains("comment-edit-form")) return;

        e.preventDefault();

        const commentId = form.dataset.commentId;
        const formData = new FormData(form);

        const button = form.querySelector("button[type='submit']");
        if (button) {
            button.disabled = true;
            button.classList.add("opacity-50");
        }

        form.classList.add("editing-in-progress");
        if (form.dataset.submitted === "true") return;
        form.dataset.submitted = "true";

        try {
            const response = await fetch('/Comment/EditComment', {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (response.ok) {
                const html = await response.text();
                const commentContainer = form.closest('.my-6');
                commentContainer.outerHTML = html;
            } else {
                console.error("Edit failed", response.status);
                if (button) {
                    button.disabled = false;
                    button.classList.remove("opacity-50");
                }
                form.dataset.submitted = "false";
            }
        } catch (err) {
            console.error("Error submitting edit", err);
            if (button) {
                button.disabled = false;
                button.classList.remove("opacity-50");
            }
            form.dataset.submitted = "false";
        }
    });
});

function enableCommentEdit(commentId) {
    const contentDiv = document.getElementById(`comment-content-${commentId}`);
    const editForm = document.getElementById(`comment-edit-form-${commentId}`);

    contentDiv.classList.add('hidden');
    editForm.classList.remove('hidden');

    const textarea = editForm.querySelector('textarea');
    if (textarea) {
        textarea.focus();
        textarea.selectionStart = textarea.value.length;
        textarea.selectionEnd = textarea.value.length;
    }
}

function cancelCommentEdit(commentId) {
    const contentDiv = document.getElementById(`comment-content-${commentId}`);
    const editForm = document.getElementById(`comment-edit-form-${commentId}`);

    contentDiv.classList.remove('hidden');
    editForm.classList.add('hidden');

    const originalContent = contentDiv.textContent.trim();
    editForm.querySelector('textarea').value = originalContent;
    editForm.querySelector('[x-data]').__x.$data.content = originalContent;
    editForm.dataset.submitted = "false";

    const submitButton = editForm.querySelector("button[type='submit']");
    if (submitButton) {
        submitButton.disabled = false;
        submitButton.classList.remove("opacity-50");
    }
}
