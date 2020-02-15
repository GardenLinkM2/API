namespace Union.Backend.Model.DAO
{
    public static class GardenLinkContextMigration
    {
        //TODO: faire une vraie migration
        public static void Migrate(this GardenLinkContext context)
        {
            context.Database.EnsureDeleted(); //Tout cramer et repartir sur des bases saines
            context.Database.EnsureCreated();
        }
    }
}
