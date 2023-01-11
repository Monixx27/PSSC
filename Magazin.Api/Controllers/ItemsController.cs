using Magazin.Api.Models;
using Magazin.Data;
using Magazin.Data.IRepositories;
using Magazin.Data.Models;
using Magazin.Domain;
using Magazin.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharp.Choices;
using Magazin.Domain.FacturaModels;
using static Magazin.Domain.Models.CartItemPublishedEvent;

namespace Magazin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        static List<InputItem> Cart = new List<InputItem>();
        private ILogger<ItemsController> logger;
        static DateLivrare Data;
        static CartItemsPublishScucceededEvent PublisedItems;

        public ItemsController(ILogger<ItemsController> logger)
        {
            this.logger = logger;
        }

        [HttpPost("/cos")]
        public IActionResult AddInCart([FromBody] InputItem item)
        {
            Cart.Add(item);
            return Ok(item);
        }

        [HttpGet("/cos")]
        public IActionResult ViewCart()
        {
            return Ok(Cart);
        }

        [HttpGet("/alldbitems")]
        public async Task<IActionResult> GetDbItems([FromServices] IItemRepository itemdb) =>
            await itemdb.TryGetExistingItems().Match(
               Succ: GetAllGradesHandleSuccess,
               Fail: GetAllGradesHandleError
            );

        private ObjectResult GetAllGradesHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        private OkObjectResult GetAllGradesHandleSuccess(List<ItemModel> items) =>
        Ok(items);



        [HttpGet("/publishfactura")]
        public async Task<IActionResult> PublishFactura([FromServices] PublishCartWorkflow publishCartWorkflow, [FromServices] PublishFacturaWorkflow publishFacturaWorkflow)
        {
            if (Cart.Count > 0)
            {
                var unvalidatedItems = Cart.Select(MapInputItemToUnvalidatedItem)
                                          .ToList()
                                          .AsReadOnly();
                PublishCartCommand command = new(unvalidatedItems);
                var result = await publishCartWorkflow.ExecuteAsync(command);
                return  result.Match<IActionResult>(
                        whenCartItemsPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                        whenCartItemsPublishScucceededEvent:  successEvent => RunFacturaWorkFlow(successEvent, publishFacturaWorkflow).Result
                    );

            }
            else
                return BadRequest("No items in cart!");

        }


        private async Task<IActionResult> RunFacturaWorkFlow (CartItemsPublishScucceededEvent successEvent, PublishFacturaWorkflow publishFacturaWorkflow)
        {
            if (Data != null)
            {
                var unvalidatedData = MapInputDataToUnvalidatedData(Data);

                PublishFacturaCommand command = new(unvalidatedData, successEvent);
                var factura = await publishFacturaWorkflow.ExecuteAsync(command);
                return factura.Match<IActionResult>(
                    whenFacturaPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                    whenFacturaPublishScucceededEvent: successEvent => Ok(successEvent)
                );
            }
            else
            {
                return BadRequest("Introduce your data first!");
            }
        }


        private static UnValidatedItem MapInputItemToUnvalidatedItem(InputItem item) => new UnValidatedItem(
            id: item.ItemId,
            quantity: item.Quantity
            );


        [HttpPost("/date")]
        public IActionResult AddData([FromBody] DateLivrare data)
        {
            Data = data;
            return Ok(Data);
        }

        [HttpGet("/date")]
        public IActionResult ViewData()
        {
            return Ok(Data);
        }

        private static Unvalidatedfactura MapInputDataToUnvalidatedData(DateLivrare data) => new Unvalidatedfactura(
            Adresa: data.Adresa,
            TipLivrare: data.TipLivrare,
            Cumparator: data.Cumparator
            );
    }
}
