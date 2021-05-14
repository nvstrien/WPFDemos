using Caliburn.Micro;
using CaliburnMicroWithSimpleInjectorDemo.ViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroWithSimpleInjectorDemo
{
    // configuration as seen here: https://www.cshandler.com/2015/09/migrating-to-simpleinjector-30-with.html#.WTaj-8a1t9M


    public class Bootstrapper : BootstrapperBase
    {
        public static readonly Container _container = new();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container.Register<IWindowManager, WindowManager>();
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();
            //_container.Register<SequentialResult>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterSingleton(viewModelType, viewModelType));

            _container.Verify();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            // as discussed here: https://stackoverflow.com/questions/32258863/simple-injector-getallinstances-throwing-exception-with-caliburn-micro

            //_container.GetAllInstances(service);

            IServiceProvider provider = _container;

            Type collectionType = typeof(IEnumerable<>).MakeGenericType(service);

            IEnumerable<object> services = (IEnumerable<object>)provider.GetService(collectionType);

            return services ?? Enumerable.Empty<object>();
        }

        protected override object GetInstance(System.Type service, string key)
        {
            return _container.GetInstance(service);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly() };
        }

        // see: https://stackoverflow.com/questions/37631468/caliburn-micro-bootstrapper-buildup-method-throws-exception-when-simple-inject
        // commenting out BuildUp still throws an exception in SimpleInjector.dll 
        protected override void BuildUp(object instance)
        {
            InstanceProducer registration = _container.GetRegistration(instance.GetType(), true);
            registration.Registration.InitializeInstance(instance);
        }
    }
}

