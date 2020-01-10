using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Template.Api.Filters
{
    public class ModelStateFeature
    {
        public ModelStateFeature(ModelStateDictionary state)
        {
            this.ModelState = state;
        }

        public ModelStateDictionary ModelState { get; }
    }
}