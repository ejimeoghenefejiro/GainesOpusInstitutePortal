using Autofac;
using GainesOpusInstitute.DataEntity;
using GainesOpusInstitute.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Repository
{
    public class AutofacRepoModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GOSContext>().InstancePerLifetimeScope();

            // register dependency convention
            builder.RegisterAssemblyTypes(typeof(IDependencyRegister).Assembly)
                .AssignableTo<IDependencyRegister>()
                .As<IDependencyRegister>()
                .AsImplementedInterfaces().InstancePerLifetimeScope();
            base.Load(builder);

        }

    }
}
