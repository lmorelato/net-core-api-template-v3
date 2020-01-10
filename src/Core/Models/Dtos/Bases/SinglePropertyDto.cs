namespace Template.Core.Models.Dtos.Bases
{
    public class SinglePropertyDto<TField> : BaseDto
    {
        public TField Value { get; set; }
    }
}
