using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            //services.AddCarter();

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            //app.MapCarter();

            return app;
        }
    }
}
