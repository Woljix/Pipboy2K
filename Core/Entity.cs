namespace Pipboy2K.Core;

public abstract class Entity
{
    public int Id { get; set; }

    public Entity()
    {
        //Id = IdGenerator.GetNextId();
    }
}