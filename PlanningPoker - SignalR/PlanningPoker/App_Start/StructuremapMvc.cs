using System.Web.Http;
using System.Web.Mvc;
using StructureMap;
using PlanningPoker.DependencyResolution;

[assembly: WebActivator.PreApplicationStartMethod(typeof(PlanningPoker.App_Start.StructuremapMvc), "Start")]

namespace PlanningPoker.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
			IContainer container = IoC.Initialize();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
        }
    }
}