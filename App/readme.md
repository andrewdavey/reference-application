# Single Page Application using KnockoutJS and RequireJS

This is a single-page web application that behaves like a real page-based application.

1. A browser makes a text/html request to the server.
2. Server returns `app.html` which contains just enough infrastructure to create the client-side Application.
3. The Application object requests JSON data for the current URL.
4. The JSON data contains a reference to a client-side script module that contains view models, HTML templates and an `init` function.
5. RequireJS is used to download the module.
6. The module's `init` function is called to create a view model object that will control the page.
7. The view model to data bound to the DOM using KnockoutJS.

## Page Navigation

1. The Application object intercepts `<a>` click events.
2. Instead of performing a normal page navigation, a JSON request is made to download the page data for the clicked URL.
3. The Application object repeats the page module loading behavior it used when initially loading.
4. The new page URL is pushed into the browser's history using the History API. This allows the back button to work as expected.

## Master-detail pages

Content pages of the application will share common surrounding UI. Traditionally, in server-side ASP.NET, this handled using master pages in Web Forms or layouts in MVC. This client-side application has a similar approach.

The JSON data for a page can specify a `parent` URL. This URL will return the JSON data for a parent paage. The Application infrastructure treats a parent page in the same way as a content page. It loads a module (using RequireJS) which contains an `init` function, view models and HTML templates.

The view model created by the `init` function must contain a `content` property. The content page's main view model will be assigned to the parent's `content` property.

The application data binds the parent view model to the DOM. So the parent HTML template will include HTML that displays the content.

The parent-child chain can be repeated. For example, the outer application frame (navigation bar, etc) can contain a vehicles master page, which in turn contains a vehicle details page.
