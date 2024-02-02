
namespace Base.Web.Framework.Localization
{
    /// <summary>
    /// Localizer
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="args">Argumentos para texto</param>
    /// <returns>Localized string</returns>
    public delegate LocalizedString Localizer(string text, params object[] args);
}
