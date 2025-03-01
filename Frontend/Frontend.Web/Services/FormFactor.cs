using Frontend.Shared.Services;

namespace Frontend.Web.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor()
    {
        return "Frontend.Web";
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}
