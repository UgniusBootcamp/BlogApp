document.addEventListener("DOMContentLoaded", function () {
    document.body.addEventListener("submit", async function (e) {
        const form = e.target;
        if (!form.classList.contains("vote-form")) return;

        e.preventDefault();

        const articleId = form.dataset.articleId;
        const container = document.getElementById(`vote-container-${articleId}`);
        const formData = new FormData(form);

        const button = form.querySelector("button[type='submit']");
        if (button) {
            button.disabled = true;
            button.classList.add("opacity-50");
        }

        form.classList.add("voting-in-progress");
        if (form.dataset.submitted === "true") return;
        form.dataset.submitted = "true";

        try {
            const response = await fetch('/ArticleVote/ArticleVote', {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (response.ok) {
                const html = await response.text();
                container.innerHTML = html;
            } else {
                console.error("Vote failed", response.status);
            }
        } catch (err) {
            console.error("Error submitting vote", err);
        }
    });
});