﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using RD2Store.Domain.Abstract;
using RD2Store.Domain.Entities;
using RD2Store.Domain.Concrete;
using System.Configuration;
using RD2Store.WebUI.Infrastructure.Abstract;
using RD2Store.WebUI.Infrastructure.Concrete;

namespace RD2Store.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel mykernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            mykernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type myserviceType)
        {
            return mykernel.TryGet(myserviceType);
        }

        public IEnumerable<object> GetServices(Type myserviceType)
        {
            return mykernel.GetAll(myserviceType);
        }
        private void AddBindings()
        {
            mykernel.Bind<IProductRepository>().To<EFProductRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsfile"] ?? "false")
            };
            mykernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

            //Authentication
            mykernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}