using DAL.Contracts;
using DAL.Repositories;
using Ninject.Modules;

namespace BLL.Infrastructure
{
    public class DIResolver: NinjectModule
    {
        private readonly string connection;
        public DIResolver(string connectionString)
        {
            connection = connectionString;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connection);
        }
    }
}
