using Microsoft.Extensions.FileProviders;
using RoutingApp.Constrains;
using System;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Importing Custom Constrain
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("halfYearMonths", typeof(HalfYearMonthsCostrain));
});

var app = builder.Build();

//this line of code enables routing from wwwroute directory/folders which is the default
// In case wwwroot folder is not present in the project throws error
app.UseStaticFiles();


//this line of code enables routing from custome directory/folders for static files.
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath + "\\mywebroot")
});





//this line of code enables routing and selecting the appropriate endpoint based on the url and method
app.UseRouting();
// this is a middleware to catch the income endpoint. This has to be always after UseRouting() method
// It is not necessary to have it and is only for the tutorial perpuses.


/*app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint endPoint = context.GetEndpoint();

    if (endPoint != null)
    {
        await context.Response.WriteAsync($"EndPoint:{endPoint.DisplayName} ");
    }
  
    await next(context);

});
*/



// creating endpoints
app.UseEndpoints(endpoints =>
{

    // Map() Method handles all the HTTP methods (GET,POST)
    endpoints.Map("/map1", async (context) =>
    {
        await context.Response.WriteAsync("In map1");
    });

    // MapGet() Method handles the HTTP GET
    endpoints.MapGet("/map2", async (context) =>
    {
        await context.Response.WriteAsync("In map2");
    }); 
    
    // MapGet() Method handles the HTTP POST
    endpoints.MapPost("/map3", async (context) =>
    {
        await context.Response.WriteAsync("In map3");
    });


    // Using Route Parameters
    endpoints.Map("/files/{filename}.{ext}", async (context) =>
    {
       string? file = Convert.ToString(context.Request.RouteValues["filename"]);
       string? ext = Convert.ToString(context.Request.RouteValues["ext"]);
   

        await context.Response.WriteAsync($"In Files:{file}.{ext}");
    });

    // Using Route Parameters
    endpoints.Map("/employees/{name}", async (context) =>
    {
        string? name = Convert.ToString(context.Request.RouteValues["name"]);

        await context.Response.WriteAsync($"Employee Name:{name}");
    });

    // Using Route Parameters with default value if not supplied
    endpoints.Map("/products/details/{id=1}", async (context) =>
    {
        int? id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product :{id}");
    });


    // Using Route Parameters as optional
    endpoints.Map("/products/price/{id?}", async (context) =>
    {
        if (!context.Request.RouteValues.ContainsKey("id"))
        {
            await context.Response.WriteAsync("Product wihtout id");
            return;
        }
        int? id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product :{id}");
    });
    
    
    // Using Route Parameters as optional with constrains
    endpoints.Map("/report/{reportdate:datetime}", async (context) =>
    {
        if (!context.Request.RouteValues.ContainsKey("reportdate"))
        {
            await context.Response.WriteAsync("No Date Supplied");
            return;
        }
        DateTime repostDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"Report on Date :{repostDate}");
    }); 
    
    // Using Route Parameters with custom constrain
    endpoints.Map("/monthly-report/{month:halfYearMonths}", async (context) =>
    {
        if (!context.Request.RouteValues.ContainsKey("month"))
        {
            await context.Response.WriteAsync("No Month Supplied");
            return;
        }

        string? month = Convert.ToString(context.Request.RouteValues["month"]);

        await context.Response.WriteAsync($"Report of Month :{month}");
    });


    // In case that anything on the routes does not match it does not run the propper endpoint


});



// Handling other url paths that does not match or endpoints 
app.Run(async context => {
    await context.Response.WriteAsync("<p>Other Paths</p>");
});




app.Run();
