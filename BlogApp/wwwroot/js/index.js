let timeout = null;
document.getElementById('livesearch').addEventListener('keyup', function (e) {
    clearTimeout(timeout);

    timeout = setTimeout(function () {
        LiveSearch()
    }, 300);
});

function LiveSearch() {
    let value = document.getElementById('livesearch').value

    $.ajax({
        type: "GET",
        url: "/Article/ArticleSearch",
        data: { searchString: value },
        datatype: "html",
        success: function (data) {
            $('#result').html(data);
        }
    });

}