using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qvastart___1.Data;
using Qvastart___1.Interfaces;
using Qvastart___1.Models;
using Qvastart___1.Services;

namespace Qvastart___1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllersWithViews();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            

            //passing connectionstring to the application db context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>

                options.UseSqlServer(builder.Configuration.GetConnectionString("QvaDb"))

            );

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>() //Registering role-based authorization.
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();



            //Adding Administrator Role via Policy.
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministrationRole",
                    policy => policy.RequireRole("Admin"));
            });


            //configure

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            builder.Services.AddTransient<ICustomEmailSender, EmailSender>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddSingleton(mapper);



            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().
                 AllowAnyHeader());
            });

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
            // Use CORS middleware.
            app.UseCors("AllowAllOrigins");
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //seeding initial data into our system.
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //Roles.
                var roles = new[] { "Admin", "Member" };
                //adding roles
                foreach (var role in roles)
                {
                    //checking if they already exist.
                    if(!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

           

            using (var scope = app.Services.CreateScope())
            {
                var userManager = 
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string QvabbyEmail = "arzonstickman999@gmail.com";
                string QvopoEmail = "tokochincharauli24@gmail.com";
                string QvaPassword = "!Qvabebi69420";

                //Adding Qvabby Account by default if doesn't exists.
                if (await userManager.FindByEmailAsync(QvabbyEmail) == null)
                {
                    var user = new QvaUser
                    {
                        Name = "Saba",
                        LastName = "Salukvadze",
                        UserName = "Qvabby",
                        Email = QvabbyEmail,
                        PhoneNumber = "599022908",
                        XP = 1000,
                        EmailConfirmed = true,
                    };

                    await userManager.CreateAsync(user, QvaPassword);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    var user = await userManager.FindByEmailAsync(QvabbyEmail);
                    var userRoles = await userManager.GetRolesAsync(user);
                    
                    if (!userRoles.Contains("Admin"))
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }

                if (await userManager.FindByEmailAsync(QvopoEmail) == null)
                {
                    var user = new QvaUser
                    {
                        Name = "Toko",
                        LastName = "Chincharauli",
                        UserName = "Qvopo",
                        Email = QvopoEmail,
                        PhoneNumber = "579208222",
                        XP = 1000,
                        EmailConfirmed = true,
                    };

                    await userManager.CreateAsync(user, QvaPassword);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    var user = await userManager.FindByEmailAsync(QvopoEmail);
                    var userRoles = await userManager.GetRolesAsync(user);

                    if (!userRoles.Contains("Admin"))
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }

            }
            
            app.Run();
        }
    }
}