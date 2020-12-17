namespace Zenject.Extensions.MenuSystem
{
    public enum MenuMode
    {
        Single,
        Additive
    }
    
    public interface IMenuArg
    { 
        MenuMode Mode { get; set; } 
    }
}