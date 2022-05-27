using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace WebService
{
    internal class XLogoDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.info.vendorExtensions["x-logo"] = new { backgroundColor = "#fff", url = "https://www.amkcambodia.com/wp-content/uploads/2019/10/amk_logo.svg", altText = "AMK Logo" };
        }
    }

}