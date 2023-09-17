using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkOrderApi.Data.Configuration;

public class IdentityUserTokenConfiguraton : IEntityTypeConfiguration<IdentityUserToken<string>> {
    
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.HasNoKey();
    }
}