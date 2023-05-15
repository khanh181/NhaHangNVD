using System.Web;
using System.Web.Optimization;

namespace NhaHangNVD
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/assets/libs/jquery/dist/jquery.min.js",
                        "~/Content/dist/js/waves.js",
                        "~/Content/dist/js/sidebarmenu.js",
                        "~/Content/dist/js/custom.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/assets/libs/bootstrap/dist/js/bootstrap.bundle.min.js",
                      "~/Content/assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js",
                      "~/Content/assets/extra-libs/sparkline/sparkline.js",
                      "~/Content/assets/libs/toastr/build/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/dist/css/style.min.css",
                      "~/Content/assets/libs/toastr/build/toastr.min.css"));
        }
    }
}
