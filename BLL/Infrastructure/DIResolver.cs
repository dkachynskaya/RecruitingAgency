using DAL.Contracts;
using DAL.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
