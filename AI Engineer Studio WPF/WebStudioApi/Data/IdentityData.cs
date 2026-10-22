using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebStudioApi.Data;

public sealed class IdentityData(DbContextOptions<IdentityData> options) : IdentityDbContext(options)
{
}
