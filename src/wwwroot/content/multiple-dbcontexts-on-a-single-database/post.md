# Multiple DbContext's with migrations on a single database

Here is a small problem I ran into while working on my current project. This project already has an Entity Framework [`DbContext`](https://msdn.microsoft.com/en-us/library/system.data.entity.dbcontext(v=vs.113).aspx) class with entities and a database. I'm adding a new feature but want to keep the changes as isolated as possible. So I wanted to create a second `DbContext` that handles the entities specific to my new feature.

Adding the context to the codebase is no hassle, just create the new class and derive it from `DbContext`. The problem is in the [migrations](https://msdn.microsoft.com/en-us/data/jj591621.aspx). Entity Framework has a system that can track changes to the model and apply those changes to the actual database. To keep track of these changes, Entity Framework uses a table called `__MigrationHistory` where it stores all kinds of metadata about the model that it can compare to when you make changes to your entities in code.

![__MigrationHistory table](/content/multiple-dbcontexts-on-a-single-database/dbo_MigHist.png)

If a second `DbContext` is added, and I enable migrations on it, it will create a second `__MigrationHistory` table. That's not going to work because that table already exists and contains migration data from the **other**  `DbContext`!

To get around this we use a feature of the database itself: Schema's. If we tell the new `DbContext` to work on a different `schema` than the rest of the database, it'll be like it's working on its own database while still getting easy access to existing tables. So how do we get the `DbContext` to work on a different schema? Turns out that's really easy:

    public class TheNewContext : DbContext
    {
        <snip>

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TheNewSchema");
        }
    }

Open the class that represents the new `DbContext` and override the `OnModelCreating` method. This gives you access to the modelbuilder where you can set a default schema for this specific context. If you now [enable migrations](https://msdn.microsoft.com/en-us/data/jj591621.aspx#enabling) on the context and [create the database](https://msdn.microsoft.com/en-us/data/jj591621.aspx#generating) from the new model it will live in its own set of tables grouped by the default schema you gave it.

And that's all there is to it. You now have a single database that is accessed by two different `DbContext`'s each with their own migration history.