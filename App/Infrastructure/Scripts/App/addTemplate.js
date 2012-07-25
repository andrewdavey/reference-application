var addTemplate = function(id, content) {
    if (document.getElementById(id)) return;

    var script = document.createElement("script");
    script.setAttribute("type", "text/html");
    script.setAttribute("id", id);
    script.textContent = content;

    var head = document.getElementsByTagName("head")[0];
    head.appendChild(script);
};