namespace Template.Localization
{
    public interface ISharedResources 
    {
        string Get(string key);
        
        string GetAndApplyKeys(string key, params string[] keys);

        string GetAndApplyValues(string key, params object[] values);
    }
}
