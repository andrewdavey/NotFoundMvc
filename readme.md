NotFoundMvc

[![daviddesloovere MyGet Build Status](https://www.myget.org/BuildSource/Badge/daviddesloovere?identifier=42e5a458-d3c2-4b20-a5e3-bf0a09d580aa)](https://www.myget.org/)

Created by Andrew Davey - http://aboutcode.net

Picked up by David De Sloovere - https://about.me/DavidDeSloovere

Provides a user-friendly '404' page whenever a controller, action or route is not found in your ASP.NET MVC3/MVC4/MVC5 application.
A view called NotFound is rendered instead of the default ASP.NET error page.

Use NuGet to install NotFound MVC: https://www.nuget.org/packages/NotFoundMvc

Take a look at the sample application for basic usage. Essentially you just have to reference the NotFoundMvc assembly and add a NotFound view to your app.

NotFoundMvc automatically installs itself during web application start-up. It handles all the different ways a 404 HttpException is usually thrown by ASP.NET MVC. This includes a missing controller, action and route.

A catch-all route is added to the end of RouteTable.Routes.
The controller factory is wrapped to catch when controller is not found.
The action invoker of Controller is wrapped to catch when the action method is not found.
