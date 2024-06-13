using GraphQL;
using GraphQL.DataLoader;
using GraphQL.MicrosoftDI;
using GraphQL.Server.Ui.Altair;
using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services;
using MyTodoList.Data.Services.DataLoader;
using MyTodoList.Data.Services.DataLoader.Abstract;
using MyTodoList.GraphQL.Mutations;
using MyTodoList.GraphQL.Queries;
using MyTodoList.GraphQL.Schemas;
using MyTodoList.Repositories;
using MyTodoList.Repositories.Abstract;
using MyTodoList.Repositories.Sql;
using MyTodoList.Repositories.Xml;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<JobRepositorySql>();
builder.Services.AddTransient<JobRepositoryXml>();
builder.Services.AddTransient<CategoryRepositorySql>();
builder.Services.AddTransient<CategoryRepositoryXml>();


builder.Services.AddSingleton<IRepositorySwitcher<Job, int>>(s => new RepositorySwitcher<Job, int>(
    s.GetRequiredService<JobRepositorySql>(),
    s.GetRequiredService<JobRepositoryXml>(),
    s.GetRequiredService<IHttpContextAccessor>()
));

builder.Services.AddSingleton<IRepositorySwitcher<Category, int>>(s => new RepositorySwitcher<Category, int>(
    s.GetRequiredService<CategoryRepositorySql>(),
    s.GetRequiredService<CategoryRepositoryXml>(),
    s.GetRequiredService<IHttpContextAccessor>()
));

builder.Services.AddSingleton<ICustomDataLoader<int, Category>, CategoryDataLoader>();
builder.Services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
builder.Services.AddSingleton<DataLoaderDocumentListener>();

builder.Services.AddSingleton<ISchema, TodoSchema>(services =>
    new TodoSchema(new SelfActivatingServiceProvider(services)));
builder.Services.AddTransient<RootQuery>();
builder.Services.AddTransient<RootMutation>();

builder.Services.AddGraphQL(options =>
{
    options.AddAutoSchema<ISchema>()
        .AddSystemTextJson()
        .AddDataLoader();
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