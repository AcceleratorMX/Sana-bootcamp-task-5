using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server.Ui.Altair;
using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Data.Service;
using MyTodoList.Data.Services;
using MyTodoList.GraphQL.Mutations;
using MyTodoList.GraphQL.Queries;
using MyTodoList.GraphQL.Schemes;
using MyTodoList.Repositories;
using MyTodoList.Repositories.Sql;
using MyTodoList.Repositories.Xml;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

// Додайте послуги до контейнера
builder.Services.AddTransient<JobRepositorySql>();
builder.Services.AddTransient<JobRepositoryXml>();
builder.Services.AddTransient<CategoryRepositorySql>();
builder.Services.AddTransient<CategoryRepositoryXml>();
builder.Services.AddTransient<SwitchRepositoryType>();
builder.Services.AddTransient<RootQuery>();

builder.Services.AddSingleton<ISchema, TodoSchema>(services => new TodoSchema(new SelfActivatingServiceProvider(services)));

builder.Services.AddSingleton(services =>
{
    var sqlRepository = services.GetRequiredService<JobRepositorySql>();
    var xmlRepository = services.GetRequiredService<JobRepositoryXml>();
    return new RepositorySwitcher<Job, int>(sqlRepository, xmlRepository);
});

builder.Services.AddSingleton(services =>
{
    var sqlRepository = services.GetRequiredService<CategoryRepositorySql>();
    var xmlRepository = services.GetRequiredService<CategoryRepositoryXml>();
    return new RepositorySwitcher<Category, int>(sqlRepository, xmlRepository);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new Exception("Connection string is not valid!");
var databaseService = new DatabaseService(connectionString);
builder.Services.AddSingleton(databaseService);

var xmlFilesDirectory = Path.Combine(
    Directory.GetCurrentDirectory(), builder.Configuration.GetValue<string>("XmlFilesDirectory") ??
                                     throw new Exception("XmlFilesDirectory is not valid!"));
builder.Services.AddSingleton(new XmlStorageService(xmlFilesDirectory));

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddGraphQL(options =>
{
    options.AddAutoSchema<ISchema>().AddSystemTextJson();
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseWebSockets();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Todo}/{id?}");

app.UseGraphQL<ISchema>();
app.UseGraphQLAltair(new AltairOptions().GraphQLEndPoint = "/altair");

app.Run();
