var IframeSubmission = Object.inherit({

    init: function (method, url, data, files, context) {
        this.files = files;
        this.context = context;
        var uniqueName = "temporary" + (new Date()).getTime();
        this.initIframe(uniqueName);
        this.initForm(method, url, uniqueName);
        this.addFormInputs(data);
        this.addFiles(files);
        this.submit();
    },

    initIframe: function (uniqueName) {
        var iframe = document.createElement("iframe");
        // Name the <iframe> so the <form> can target it.
        iframe.setAttribute("name", uniqueName);
        iframe.style.display = "none";
        document.body.appendChild(iframe);

        this.iframe = iframe;
    },

    initForm: function (method, url, target) {
        var form = document.createElement("form");
        var isMethodValidForForm = method.match(/^(get|post)$/i);
        if (isMethodValidForForm) {
            form.setAttribute("action", url);
            form.setAttribute("method", method);
        } else {
            form.setAttribute("action", url + "?_method=" + method);
            form.setAttribute("method", "post");
        }
        form.setAttribute("enctype", "multipart/form-data");
        form.setAttribute("target", target);

        this.form = form;
    },

    addFormInputs: function (data) {
        Object.keys(data).forEach(function (key) {
            var value = data[key].toString();
            var input = this.createInput(key, value);
            this.form.appendChild(input);
        }, this);
    },

    createInput: function (key, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", key);
        input.setAttribute("value", value.toString());
        return input;
    },

    addFiles: function (files) {
        files.forEach(this.addFile, this);
    },

    addFile: function (file) {
        // Move the file element into the temporary form.
        // Insert a placeholder so we can move it back to its original location later.
        var placeholder = document.createElement("div");
        file.parentNode.insertBefore(placeholder, file);
        this.form.appendChild(file);
        file.restore = function () {
            placeholder.parentNode.insertBefore(file, placeholder);
            placeholder.parentNode.removeChild(placeholder);
        };
    },

    submit: function () {
        this.request = $.Deferred();

        $(this.iframe).load(function () {
            this.restoreFiles();
            this.removeIframe();
            this.request.resolveWith(this.context, []);
        }.bind(this));

        this.form.submit();
    },

    restoreFiles: function () {
        this.files.forEach(function (file) {
            file.restore();
        });
    },

    removeIframe: function () {
        // Delay iframe removal so when this is called from the load event 
        // we won't break things due to the object still being in use.
        var iframe = this.iframe;
        setTimeout(function () {
            iframe.parentNode.removeChild(iframe);
        }, 1);
    }
    
});