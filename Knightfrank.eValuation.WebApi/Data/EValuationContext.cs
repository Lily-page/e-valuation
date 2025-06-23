using Microsoft.EntityFrameworkCore;
using System;

namespace Knightfrank.eValuation.WebApi.Data;

public class EValuationContext : DbContext
{
    public EValuationContext(DbContextOptions<EValuationContext> options) : base(options)
    {
    }

  
}