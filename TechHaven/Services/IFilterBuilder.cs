namespace TechHaven.Services
{
    public interface IFilterBuilder
    {
    }
    /*
     * Nakon sto se napravi ovaj interface i njegova impl
     * U filtermedijatoru se poziva if(min != 0)AddMinPrice() itd
     * Zatim se nakon svih dodavanja radi filterbuilder.Build()
     * Zatim FilterMediator poziva filter.GetFilteredProducts() i prosljedjuje ih u kontroler
     * To je vise manje to, also dodati metodu za AddProducts u builder
     * Proglasiti FilterBuilder i Mediator za servise u Program.cs
     * */
}
