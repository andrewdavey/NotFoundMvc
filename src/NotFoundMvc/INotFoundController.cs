namespace NotFoundMvc
{
    using System.Web.Mvc;

    public interface INotFoundController : IController
    {
        ActionResult NotFound();
    }
}