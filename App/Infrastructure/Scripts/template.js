define(function () {
    var createTemplateScriptElement = function (id, templateText) {
        var script = document.createElement("script");
        script.setAttribute("id", id);
        script.setAttribute("type", "text/html"); // The browser won't try to execute this "script".
        script.textContent = templateText;
        return script;
    };

    return {
        load: function (name, require, load) {
            // Get the template content as text.
            require(["text!" + name], function (templateText) {
                if (!document.getElementById(name)) {
                    var script = createTemplateScriptElement(name, templateText);
                    document.body.appendChild(script);
                }
                // Tell RequireJS that we're done loading.
                load(templateText);
            });
        }
    };
});