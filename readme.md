NotFoundMvc

[![daviddesloovere MyGet Build Status](https://www.myget.org/BuildSource/Badge/daviddesloovere?identifier=42e5a458-d3c2-4b20-a5e3-bf0a09d580aa)](https://www.myget.org/)

Created by Andrew Davey - http://aboutcode.net

Picked up by David De Sloovere - https://about.me/DavidDeSloovere

Provides a user-friendly '404' page whenever a controller, action or route is not found in your ASP.NET MVC3/MVC4/MVC5 application.
A view called NotFound is rendered instead of the default ASP.NET error page.

Use NuGet to install NotFound MVC: https://www.nuget.org/packages/NotFoundMvc

Take a look at the sample application for basic usage. 
Essentially you just have to add the NotFoundMvc package and optionally alter the NotFound.cshtml view to your app. You also need to change `BlockViewHandler` in `Views\web.config` to use `NotFoundMvc.NotFoundHandler`.

Starting with v1.4, you can plug in an action to be executed on a 404.

    NotFoundConfig.OnNotFound = (req, uri) =>
    {
        // The current URI, which can be the same as 
        // the original requested URI http://localhost:43825/bin
        // or triggered from the IIS via the system.webServer/httpErrors
        // and look like this http://localhost:43825/notfound?404;http://localhost:43825/bin
        System.Diagnostics.Trace.WriteLine(req.Url.ToString());
    
        // This is the original requested URI http://localhost:43825/bin
        System.Diagnostics.Trace.WriteLine(uri);
    
        // log string w/ NLog
        Log.Warn(CultureInfo.InvariantCulture, "404 {0}", uri);

        // log as error w/ ELMAH
        Elmah.ErrorSignal.FromCurrentContext().Raise(new HttpException(404, uri.ToString()));
    };

NotFoundMvc automatically installs itself during web application start-up. It handles all the different ways a 404 HttpException is usually thrown by ASP.NET MVC. This includes a missing controller, action and route.

A catch-all route is added to the end of RouteTable.Routes.
The controller factory is wrapped to catch when controller is not found.
The action invoker of Controller is wrapped to catch when the action method is not found.
