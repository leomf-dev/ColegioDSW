using Colegio.BL.BC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Leer connection string
string conn = builder.Configuration.GetConnectionString("ColegioDB");

// Registrar BC con factory (inyecci�n simple)
builder.Services.AddScoped(sp => new AlumnoBC(conn));
builder.Services.AddScoped(sp => new CursoBC(conn));
builder.Services.AddScoped(sp => new MatriculaBC(conn));
builder.Services.AddScoped(sp => new NotaBC(conn));
builder.Services.AddScoped(sp => new DocenteBC(conn));
builder.Services.AddScoped(sp => new SeccionBC(conn));
builder.Services.AddScoped(sp => new HorarioBC(conn));
builder.Services.AddScoped(sp => new DetalleMatriculaBC(conn));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
