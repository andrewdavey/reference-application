var addTemplate = function(id, content) {
    var templateExists = document.getElementById(id);
    if (templateExists) return;

    // HTML templates are stored as <script> elements in the <head>.
    // Knockout finds them by ID.
    var script = document.createElement("script");
    script.setAttribute("type", "text/html");
    script.setAttribute("id", id);
    script.textContent = content;

    var head = document.getElementsByTagName("head")[0];
    head.appendChild(script);
};