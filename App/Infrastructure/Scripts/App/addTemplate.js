var addTemplate = function(id, content) {
    if (document.getElementById(id)) return;

    var script = document.createElement("script");
    script.setAttribute("type", "text/html");
    script.setAttribute("id", id);
    script.textContent = content;

    document.body.appendChild(script);
};