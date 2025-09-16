var builder = DistributedApplication.CreateBuilder(args);

// Add the user-facing web application
builder.AddProject<Projects.HurricaneResources_Web_User>("user-app")
    .WithExternalHttpEndpoints();

// Add the admin web application  
builder.AddProject<Projects.HurricaneResources_Web_Admin>("admin-app")
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();
