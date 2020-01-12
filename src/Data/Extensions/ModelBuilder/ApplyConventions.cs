using System.Linq;
using Microsoft.EntityFrameworkCore;
using Template.Shared;

namespace Template.Data.Extensions.ModelBuilder
{
    public static partial class ModelBuilderExtensions
    {
        public static Microsoft.EntityFrameworkCore.ModelBuilder ApplyConventions(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            var entities = modelBuilder.Model.GetEntityTypes();

            foreach (var entity in entities)
            {
                var properties = entity.GetProperties();

                foreach (var prop in properties)
                {
                    if (prop.ClrType != typeof(string))
                    {
                        continue;
                    }

                    prop.SetIsUnicode(false);
                    if (prop.GetMaxLength() == null)
                    {
                        prop.SetMaxLength(Constants.Database.DefaultVarcharMaxLength);
                    }
                }

                var cascadeForeignKeys = entity.GetForeignKeys()
                    .Where(c => !c.IsOwnership && c.DeleteBehavior == DeleteBehavior.Cascade);

                foreach (var fk in cascadeForeignKeys)
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

            return modelBuilder;
        }
    }
}
