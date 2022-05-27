using System.Web.Http;
using WebService;
using System.Web;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebService
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
              .EnableSwagger(c =>
              {
                  c.DocumentFilter<XLogoDocumentFilter>();
                  c.OperationFilter<AddRequiredAuthorizationHeaderParameter>();
                  c.SingleApiVersion("v1", "Table App Api Documentation")
                      .Description("A sample API for Table App Api features")
                      .TermsOfService("https://amkcambodia.com")
                      .Contact(cc => cc
                         .Name("Toun Chanthoeurn")
                         .Url("https://amkcambodia.com")
                         .Email("Chanthoeurn.Toun@amkcambodia.com"))
                      .License(lc => lc
                         .Name("MFI")
                         .Url("https://amkcambodia.com")
                       );
                  c.IncludeXmlComments(string.Format(@"{0}\bin\WebService.XML",
                                       System.AppDomain.CurrentDomain.BaseDirectory));

              })
              .EnableSwaggerUi(c =>
              {
                  c.EnableApiKeySupport("Tablet", "header");
              });
        }

    }
}
