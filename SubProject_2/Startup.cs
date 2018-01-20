using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebService.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SubProject_2
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
            services.AddMvc();
            services.AddAutoMapper();

            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IMapper>(CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();

            app.UseMvc();
        }

        public IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostModel>()
                    .ReverseMap();
                cfg.CreateMap<User, UserModel>()
                    .ReverseMap();
                cfg.CreateMap<Question, QuestionModel>()
                    .ReverseMap();
                cfg.CreateMap<Answer, AnswerModel>()
                    .ReverseMap();
                cfg.CreateMap<Comment, CommentModel>()
                    .ReverseMap();
                cfg.CreateMap<Note, NoteModel>()
                    .ReverseMap();
                cfg.CreateMap<Favorite, FavoriteModel>()
                    .ReverseMap();
                cfg.CreateMap<FavoriteList, FavoriteListModel>()
                    .ReverseMap();
                cfg.CreateMap<Result, ResultModel>()
                    .ReverseMap();
                cfg.CreateMap<RelatedWordList, RelatedWordListModel>()
                    .ReverseMap();
                cfg.CreateMap<CoOrcorruingWord, CoOrcorruingWordListModel>()
                    .ReverseMap();
                cfg.CreateMap<Tags, TagModel>()
                    .ReverseMap();
                cfg.CreateMap<TermNetwork, TermNetworkModel>()
                    .ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
