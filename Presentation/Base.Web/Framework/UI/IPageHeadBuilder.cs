namespace Base.Web.Framework.UI
{
    /// <summary>
    /// Page head builder
    /// </summary>
    public partial interface IPageHeadBuilder
    {
        void AddScriptParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle, bool isAsync);
        void AppendScriptParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle, bool isAsync);
        string GenerateScripts(ResourceLocation location);
        void AddInlineScriptParts(ResourceLocation location, string script);
        void AppendInlineScriptParts(ResourceLocation location, string script);
        string GenerateInlineScripts(ResourceLocation location);
        void AddCssFileParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle = false);
        void AppendCssFileParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle = false);
        string GenerateCssFiles(ResourceLocation location);
        void AddCanonicalUrlParts(string part);
        void AppendCanonicalUrlParts(string part);
        string GenerateCanonicalUrls();
        void AddHeadCustomParts(string part);
        void AppendHeadCustomParts(string part);
        string GenerateHeadCustom();
        void AddPageCssClassParts(string part);
        void AppendPageCssClassParts(string part);
        string GeneratePageCssClasses();
        void AddEditPageUrl(string url);
        string GetEditPageUrl();
        void SetActiveMenuItemSystemName(string systemName);
        string GetActiveMenuItemSystemName();
    }
}
