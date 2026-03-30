using ECommecerBll;
using ECommecerDal;
using ECommecerDal.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1;

namespace ECommecer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //添加数据库上下文
            builder.Services.AddDbContext<AppDbContext>(c =>
                c.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
            //注册仓储层
            builder.Services.AddScoped<UserRepository>();
            //注册服务层
            builder.Services.AddScoped<UserService>();
            //注册JwtHelper
            builder.Services.AddScoped<JwtHelper>();
            //添加控制器
            builder.Services.AddControllers();
            //添加 Swagger 基础服务
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //添加 JWT认证
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSecretKeyHere12345678901234567890");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,           // 验证颁发者
                        ValidateAudience = true,         // 验证接收者
                        ValidateLifetime = true,         // 验证过期时间
                        ValidateIssuerSigningKey = true, // 验证签名密钥
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();          
            app.UseCors("AllowAll");        
            app.UseHttpsRedirection();      
            app.UseAuthentication();        
            app.UseAuthorization();         
            app.MapControllers();           
            app.Run();
        }
    }
}