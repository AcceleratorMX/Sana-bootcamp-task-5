using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server.Ui.Altair;
using GraphQL.Types;
using MyTodoList.Data.Service;
using MyTodoList.Data.Services;
using MyTodoList.GraphQL.Mutations;
using MyTodoList.GraphQL.Queries;
using MyTodoList.GraphQL.Schemes;
using MyTodoList.Repositories;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

// Додайте послуги до контейнера
builder.Services.AddTransient<JobRepositorySql>();
builder.Services.AddTransient<JobRepositoryXml>();
builder.Services.AddSingleton<RepositoryTypeService>();
builder.Services.AddTransient<SwitchRepositoryTypeMutation>();
builder.Services.AddTransient<RootQuery>();

builder.Services.AddSingleton<ISchema, TodoSchema>(services => new TodoSchema(new SelfActivatingServiceProvider(services)));

// Реєстрація JobRepositorySwitcher з використанням фабрики
builder.Services.AddTransient<JobRepositorySwitcher>(sp => new JobRepositorySwitcher(
    sp.GetRequiredService<JobRepositorySql>(),
    sp.GetRequiredService<JobRepositoryXml>(),
    sp.GetRequiredService<RepositoryTypeService>(),
    sp.GetRequiredService<ILogger<JobRepositorySwitcher>>()
));

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
