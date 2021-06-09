using Elearning.Api.Attributes;
using Elearning.Models.Entities;
using Elearning.Models.Settings;
using Elearning.Services.Lessons;
using Elearning.Services.Authen;
using Elearning.Services.Combobox;
using Elearning.Services.Course;
using Elearning.Services.GroupUsers;
using Elearning.Services.ProgramEducation;
using Elearning.Services.UserCustomer;
using Elearning.Services.Users.Employee;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NTS.Common.Files;
using NTS.Common.RabbitMQ;
using NTS.Common.RedisCache;
using NTS.Common.Users;
using NTS.Common.Utils;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Elearning.Services.Categorys;
using Elearning.Services.Topics;
using Elearning.Services.Questions;
using Elearning.Services.Question;
using Elearning.Services.SideBar;
using Elearning.Services.EmployeeSpecialist;
using Elearning.Services.Comments;
using Elearning.Services.HomeSetting;
using Elearning.Services.Fontend.Expert;
using Elearning.Services.Fontend.Exam;
using Elearning.Services.HomeService;
using Elearning.Services.Fontend.User;
using Elearning.Services.CompleteStatistics;
using Elearning.Services.ReportLearner;
using Elearning.Services.ReportLearnerProvince;
using Elearning.Services.FileTemplate;
using Elearning.Services.NumberOfSubscribersStatistics;
using Elearning.Services.Dashboard;
using Elearning.Services.Fontend.MyCourse;
using Elearning.Api.AppInitialize;
using Elearning.Services.Log;
using Elearning.Services.History;
using Elearning.Mobile.Service.Program;
using Elearning.Services.Mobile.Course;
using Elearning.Mobile.Service.Comment;

using Elearning.Mobile.Service.Lesson;
using Elearning.Mobile.Service.Test;
using Elearning.Mobile.Service.HomeSetting;
using Elearning.Services.HomeLink;
using Elearning.Services.ManagerUnit;
using Elearning.Services.SystemParams;

namespace Elearning.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddDbContext<ElearningContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Redis cache
            services.AddRedisCache(Configuration);

            // RabbitMQ
            services.AddRabbitMQ(Configuration);

            // Upload files
            services.AddNtsUpload(Configuration);

            services.AddDetection();
            services.AddControllersWithViews();

            services.AddMvc(config =>
            {
                config.Filters.Add(new ApiHandleExceptionSystemAttribute());
            })
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                 .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            var appSettingsSection = Configuration.GetSection("AppSetting");
            services.Configure<AppSettingModel>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettingModel>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elearning API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                xmlPath = Path.Combine(AppContext.BaseDirectory, "Elearning.Model.xml");
                c.IncludeXmlComments(xmlPath);
            });

            // configure DI for application services
            //services.AddScoped<ISampleService, SampleService>();
            services.AddScoped<IPasswordUtilsService, PasswordUtilsService>();
            services.AddScoped<IComboboxService, ComboboxService>();
            services.AddScoped<IUserAdminService, UserAdminService>();
            services.AddScoped<IAuthenService, AuthenService>();
            services.AddScoped<INtsUserService, NtsUserService>();
            services.AddScoped<IGroupUserService, GroupUserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IProgramService, ProgramService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISideBarService, SideBarService>();
            services.AddScoped<IEmployeeSpecialistService, EmployeeSpecialistService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IHomeSettingService, HomeSettingService>();
            services.AddScoped<IHomeServiceService, HomeServiceService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExpertService, ExpertService>();
            services.AddScoped<IFileTemplateService, FileTemplateService>();
            services.AddScoped<Elearning.Services.Fontend.Program.IProgramService, Elearning.Services.Fontend.Program.ProgramService>();
            services.AddScoped<Elearning.Services.Fontend.Course.ICourseService, Elearning.Services.Fontend.Course.CourseService>();
            services.AddScoped<Elearning.Services.Fontend.HomeService.IHomeService, Elearning.Services.Fontend.HomeService.HomeService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ICompleteStatisticsService, CompleteStatisticsService>();
            services.AddScoped<IReportLearnerService, ReportLearnerService>();
            services.AddScoped<IReportLearnerProvinceService, ReportLearnerProvinceService>();
            services.AddScoped<INumberOfSubscribersService, NumberOfSubscribersService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMyCourseService, MyCourseService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IMobileCourseService, MobileCourseService>();
            services.AddScoped<IMobileProgramService, MobileProgramService>();
            services.AddScoped<IMobileCommentService, MobileCommentService>();
            services.AddScoped<IMobileLessonService, MobileLessonService>();
            services.AddScoped<IMobileTestService, MobileTestService>();
            services.AddScoped<IMobileHomeSettingService, MobileHomeSettingService>();
            services.AddScoped<IHomeLinkService, HomeLinkService>();
            services.AddScoped<IManageUnitService, ManageUnitService>();
            services.AddScoped<ISystemParamService, SystemParamService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            InitializeDatabase(app);
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM0NDcwQDMxMzgyZTMzMmUzMG9VcjRPaEYvdVBheW9RY1FzNWRxVlpHRXpUVzNjUUorNDZpMlluV0YxSjQ9");

            loggerFactory.AddFile("Logs/log-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elearning API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // App images

            //app.UseStaticFiles();

            app.UseNtsUploadStaticFiles();

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void InitializeDatabase (IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ElearningContext>();
                //context.Database.Migrate();

                //var mongoDbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();
                //InitViolation.Init(mongoDbService);

                var passwordUtilsService = scope.ServiceProvider.GetRequiredService<IPasswordUtilsService>();
                InitUser.Init(context, passwordUtilsService);
                InitPermission.Init(context);
            }
        }
    }
}
