using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.DataAccess.Helpers.Transaction;

namespace TekhneCafe.DataAccess.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configurations)
        {
            #region EntityFramework Context
            services.AddDbContext<EfTekhneCafeContext>(_ => _.UseSqlServer(configurations.GetConnectionString("EfTekhneCafeContext")));
            #endregion

            #region IOC Scoped Services
            services.AddScoped<IAppUserDal, EfAppUserDal>();
            services.AddScoped<IAttributeDal, EfAttributeDal>();
            services.AddScoped<ICategoryDal, EfCategoryDal>();
            services.AddScoped<IProductAttributeDal, EfProductAttributeDal>();
            services.AddScoped<IOrderDal, EfOrderDal>();
            services.AddScoped<IOrderHistoryDal, EfOrderHistoryDal>();
            services.AddScoped<IProductDal, EfProductDal>();
            services.AddScoped<ITransactionHistoryDal, EfTransactionHistoryDal>();
            services.AddScoped<IOrderProductAttributeDal, EfIOrderProductAttributeDal>();
            services.AddScoped<IOrderProductDal, EfOrderProductDal>();
            services.AddScoped<IOrderHistoryDal, EfOrderHistoryDal>();
            services.AddScoped<ITransactionManagement, EfTransactionManagement>();
            services.AddScoped<INotificationDal, EfNotificationDal>();
            #endregion
        }
    }
}
