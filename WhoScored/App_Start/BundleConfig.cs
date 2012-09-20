using System.Web.Optimization;

namespace WhoScored.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            // .debug.js, -vsdoc.js and .intellisense.js files 
            // are in BundleTable.Bundles.IgnoreList by default.
            // Clear out the list and add back the ones we want to ignore.
            // Don't add back .debug.js.
            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*intellisense.js");

            // Modernizr goes separate since it loads first
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                            .Include("~/Scripts/modernizr-{version}.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery",
                                         "https://ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js")
                            .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout",
                                         "http://ajax.aspnetcdn.com/ajax/knockout/knockout-2.1.0.js")
                            .Include("~/Scripts/knockout-{version}.debug.js"));

            // jSlibs
            bundles.Add(new ScriptBundle("~/bundles/jslibs")
                            .Include(
                                "~/Scripts/bootstrap.js",
                                "~/Scripts/bootstrap-modal.js",
                                "~/Scripts/bootstrap-progressbar.min.js",
                                "~/Scripts/jquery.unobtrusive-knockout.js",
                                "~/Scripts/linq.min.js"
                            ));
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.min.css", "~/Content/bootstrap-responsive.min.css",
                    "~/Content/DT_bootstrap.css", "~/Content/jquery.dataTables.css"
                ));

            bundles.Add(new Bundle("~/Content/less", new LessTransform(), new CssMinify())
                .Include("~/Content/vertical-buttons.less", 
                "~/Content/who-scored.less", "~/Content/rez-box.less"));
        }
    }
}