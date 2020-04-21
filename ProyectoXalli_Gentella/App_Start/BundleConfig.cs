using System.Web;
using System.Web.Optimization;

namespace ProyectoXalli_Gentella
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // JS DE LA PLANTILLA GENTELLA
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/fastclick.js",
                      "~/Scripts/nprogress.js",
                      "~/Scripts/Chart.min.js",
                      "~/Scripts/gauge.min.js",
                      "~/Scripts/bootstrap-progressbar.min.js",
                      "~/Scripts/icheck.min.js",
                      "~/Scripts/skycons.js",
                      "~/Scripts/date.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/daterangepicker.min.js",
                      "~/Scripts/custom.min.js"));

            // CSS DE LA PLANTILLA GENTELLA
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/nprogress.css",
                      "~/Content/green.css",
                      "~/Content/bootstrap-progressbar-3.3.4.min.css",
                      "~/Content/daterangepicker.min.css",
                      "~/Content/custom.min.css"));

            // JS DE LA PLANTILLA GENTELLA
            bundles.Add(new ScriptBundle("~/bundles/datatableJS").Include(
                      "~/Scripts/datatable/jquery.dataTables.js",
                      "~/Scripts/datatable/dataTables.bootstrap.js"));

            // CSS DE LA PLANTILLA GENTELLA
            bundles.Add(new StyleBundle("~/Content/datatableCSS").Include(
                      "~/Content/datatable/dataTables.bootstrap.css"));
        }
    }
}
